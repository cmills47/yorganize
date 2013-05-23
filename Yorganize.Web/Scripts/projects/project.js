
/* PROJECT MODEL */

ProjectModel = Backbone.Model.extend({
    defaults:
    {
        Name: "new project",
        Position: 0,
        Type: null,
        FolderID: null,

        itemType: "project"
    },

    Actions: null,
    idAttribute: "ID",

    initialize: function () {
        this.on("contents:changed", function () {
            var parent = this.getParent();
            if (parent && parent.id != this.id)
                parent.trigger("contents:changed");
        }, this);
    },

    parse: function (response, options) {
        this.Actions = new ActionsCollection(response.Actions, { parent: this });
        delete response.Actions; // remove Actions from response once used

        return response;
    },

    methodToUrl: {
        "create": "Projects/CreateProject",
        "update": "Projects/UpdateProject",
        "delete": "Projects/DeleteProject",
        "read": "Projects/GetProject",
    },

    sync: function (method, model, options) {
        options = options || {};
        options.url = model.methodToUrl[method.toLowerCase()];

        switch (method) {
            case 'delete':
                options.url += '/' + this.id;
                break;
        }

        Backbone.sync(method, model, options);
    },

    getParentId: function () {
        return this.get("FolderID");
    },

    getParent: function () {
        return this.collection.parent;
    },

    // returns itself instead of contents
    getContents: function () {
        return [this];
    }
});

/* PROJECT COLLECTION */

ProjectsCollection = Backbone.Collection.extend({
    model: ProjectModel,
    comparator: function (a, b) {
        return a.get("Position") - b.get("Position");
    },

    initialize: function (models, options) {
        this.parent = options.parent;

        this.on("add", function (model) {
            model.getParent().trigger("contents:changed");
        });

        this.on("remove", function (model) {
            model.getParent().trigger("contents:changed");
        });
        
        this.on("destroy", function (model, collection) {
            
            var parent = collection.parent; // the parent folder

            // update greater siblings positions
            _.each
            (
                parent.getContents(false, function (item) {
                    return item.get("Position") > model.get("Position");
                }), // for each greater sibling
                function (sibling) {
                    sibling.set("Position", sibling.get("Position") - 1);
                } // decrement position
            );
        });
    }
});

/* EDIT PROJECT VIEW */

EditProjectView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#edit-project-template').html();
        this.details = new EditProjectDetailsView({ model: this.model });
        this.files = new EditProjectFilesView({ model: this.model });
        this.active = "details"; // the active view: details (default) / files
        this.model.on("destroy", this.close, this);
    },

    events: {
        "click #remove-project": "removeProject",
        "click #edit-details": "editDetails",
        "click #edit-files": "editFiles"
    },

    render: function () {
        var $content = _.template(this.template, { active: this.active });
        this.$el.html($content);
        var $container = this.$('#container');
        switch (this.active) {
            case "details":
                $container.html(this.details.render().el);
                break;
            case "files":
                $container.html(this.files.render().el);
                break;
            default:
                throw "The project has no active view.";
        }

        return this;
    },

    // removes details and files sub-views and then also removes the current view
    close: function () {
        this.files.remove();
        this.details.remove();
        this.remove();
    },

    removeProject: function (e) {
        e.preventDefault();

        if (!confirm("Are you sure you want to delete this project?"))
            return;

        this.model.destroy({
            wait: true
        });
    },

    editDetails: function (e) {
        if (this.active != "details") {
            this.active = "details";
            this.render();
        }
        e.preventDefault();
    },

    editFiles: function (e) {
        if (this.active != "files") {
            this.active = "files";
            this.render();
        }
        e.preventDefault();
    }
});

/* EDIT PROJECT DETAILS VIEW */

EditProjectDetailsView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#edit-project-details-template').html();
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    }
});

/* EDIT PROJECT FILES VIEW */

EditProjectFilesView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#edit-project-files-template').html();
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    }

});