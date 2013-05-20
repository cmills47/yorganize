
/* ACTION MODEL */

ActionModel = Backbone.Model.extend({
    defaults: {
        Name: "new action",
        Position: 0,
        Type: null,
        Status: null,
        ProjectID: null,

        itemType: "action"
    },

    idAttribute: "ID",

    methodToUrl: {
        "create": "Projects/CreateAction",
        "update": "Projects/UpdateAction",
        "delete": "Projects/DeleteAction",
        "read": "Projects/GetAction",
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

    getParent: function () {
        return this.collection.parent;
    }
});

/* ACTIONS COLLECTION */

ActionsCollection = Backbone.Collection.extend({
    model: ActionModel,
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
    }
});

/* ACTION VIEW*/

ActionView = Backbone.View.extend({
    initialize: function (options) {
        this.template = $('#actions-action-template').html();
    },

    events: {
        "click #edit-action": "editAction",
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    },

    editAction: function (e) {
        window.router.vent.trigger("edit:open", this.model);
        e.preventDefault();
    }
});

/* ACTION FOLDER VIEW */

ActionFolderView = Backbone.View.extend({
    initialize: function (options) {
        this.template = $('#actions-folder-template').html();
        this.ident = this.options.ident || 0;
        this.model.bind("change", this.render, this);
    },

    events: {
        "click #edit-folder": "editFolder"
    },

    render: function () {
        this.model.set("ident", this.ident, { silent: true });
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        var contents = this.model.getContents(); // get folders and projects
        if (contents)
            renderItems(this.$el, contents, this.ident + 30);

        return this;
    },

    editFolder: function (e) {
        window.router.vent.trigger("edit:open", this.model);
        e.preventDefault();
    }
});

/* ACTION PROJECT VIEW */

ActionProjectView = Backbone.View.extend({
    initialize: function (options) {
        this.template = $('#actions-project-template').html();
        this.ident = this.options.ident || 0;
        this.model.bind("change", this.render, this);
    },

    events: {
        "click #new-action": "newAction",
        "click #edit-project": "editProject"
    },

    render: function () {
        this.model.set("ident", this.ident, { silent: true });
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        // render actions
        var $container = this.$('#actions');
        this.model.Actions.forEach(function (action) {
            var actionView = new ActionView({ model: action });
            $container.append(actionView.render().el);
        });

        return this;
    },

    newAction: function (e) {
        window.router.vent.trigger("new-action", this.model);
        e.preventDefault();
    },

    editProject: function (e) {
        window.router.vent.trigger("edit:open", this.model);
        e.preventDefault();
    }

});

/* ACTIONS VIEW */

ActionsView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#actions-template').html();
        // model can be a folder or a project - use 'setModel' function to set models for this view
    },

    events: {
        "click #new-folder": "newFolder",
        "click #new-project": "newProject"
    },

    render: function () {
        console.log("rendering actions...");

        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        console.log("rendering actions for ->", this.model.id, this.model.get("Name"));

        var contents = this.model.getContents(); // get folders and projects

        var $container = this.$('#items');
        $container.empty();
        renderItems($container, contents, 0);
    },

    setModel: function (model) {
        if (this.model) // unbind previous events
            this.model.off("contents:changed", this.render, this);
        this.model = model;
        // bind events to new model
        this.model.on("contents:changed", this.render, this);
    },

    newFolder: function (e) {
        window.router.vent.trigger("new-folder", this.model);
        e.preventDefault();
    },

    newProject: function (e) {
        window.router.vent.trigger("new-project", this.model);
        e.preventDefault();
    },
});

function renderItems($container, items, ident) {
    _.each(items, function (item) {
        var itemView;
        switch (item.get("itemType")) {
            case "folder":
                itemView = new ActionFolderView({ model: item, ident: ident });
                break;
            case "project":
                itemView = new ActionProjectView({ model: item, ident: ident });
                break;
            default:
                throw "unknown type: " + item.get("Type");
        }
        console.log("rendering " + item.get("itemType") + " " + item.get("Name") + " " + ident);
        $container.append(itemView.render().el);

        // checkboxes
        $('input[type=checkbox]').iCheck({
            labelHover: false,
            cursor: true,
            checkboxClass: 'icheckbox_square-blue',
        });
    });
}

/* EDIT ACTION VIEW */

EditActionView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#edit-action-template').html();
        this.details = new EditActionDetailsView({ model: this.model });
        this.files = new EditActionFilesView({ model: this.model });
        this.active = "details"; // the active view: details (default) / files
        this.model.on("destroy", this.close, this);
    },

    events: {
        "click #remove-action": "removeAction",
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
                throw "The action has no active view.";
        }

        return this;
    },

    // removes details and files sub-views and then also removes the current view
    close: function () {
        this.files.remove();
        this.details.remove();
        this.remove();
    },

    removeAction: function (e) {
        e.preventDefault();

        if (!confirm("Are you sure you want to delete this action?"))
            return;

        this.model.destroy();
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

/* EDIT ACTION DETAILS VIEW */

EditActionDetailsView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#edit-action-details-template').html();
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    }
});

/* EDIT PROJECT FILES VIEW */

EditActionFilesView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#edit-action-files-template').html();
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    }

});