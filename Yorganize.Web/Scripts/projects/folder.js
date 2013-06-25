
/* FOLDER MODEL*/

FolderModel = Backbone.Model.extend({
    defaults: {
        Name: "new folder",
        Position: 0,
        Type: null,
        ParentID: null,

        itemType: "folder",
        expanded: true
    },

    Projects: null,
    idAttribute: "ID",

    initialize: function () {
        // on contents changed event trigger the same event on parent
        this.on("contents:changed", function () {
            var parent = this.getParent();
            if (parent && parent.id != this.id)
                parent.trigger("contents:changed");
        }, this);
    },

    parse: function (response, options) {
        this.Projects = new ProjectsCollection(response.Projects, { parent: this });
        this.Projects.forEach(function (project) {
            var actions = project.get("Actions");
            project.Actions = new ActionsCollection(actions, { parent: project });
            project.unset("Actions", { silent: true });
        }, this);
        delete response.Projects; // remove Projects from response once used

        return response;
    },

    methodToUrl: {
        "create": "Projects/CreateFolder",
        "update": "Projects/UpdateFolder",
        "delete": "Projects/DeleteFolder",
        "read": "Projects/GetFolder",
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
        return this.get("ParentID");
    },

    getParent: function () {
        return this.collection.findWhere({ ID: this.getParentId() });
    },

    // returns a combined list of folders and projects directly under the current folder
    // if <self> is true, includes the folder itself into the contents list
    // if <filter> is specified, further filters the folders list
    getContents: function (self, filter) {
        if (!this.collection) return null;

        var folders = this.collection.filter(function (folder) {
            var match = folder.get("ParentID") == this.get("ID") && folder.get("ID") != null;
            match = match || (self && folder.get("ID") == this.get("ID"));
            match = match && (!filter || filter(folder));
            return match;
        }, this);

        var projects = filter ? _.filter(this.Projects.models, filter) : this.Projects.models;
        [].push.apply(folders, projects);

        folders.sort(function (a, b) {
            return a.get("Position") - b.get("Position");
        });

        return folders;
    },

    toggle: function () {
        this.set({ expanded: !this.get("expanded") });
    }

});

/* FOLDERS COLLECTION*/

FoldersCollection = Backbone.Collection.extend({
    model: FolderModel,
    url: "Projects/GetData",
    comparator: function (a, b) {
        return a.get("Position") - b.get("Position");
    },

    initialize: function () {

        this.on("add", function (model) {
            model.getParent().trigger("contents:changed");
        });

        this.on("remove", function (model) {
            model.getParent().trigger("contents:changed");
        });

        this.on("destroy", function (model, collection, options) {

            var parent = this.findWhere({ ID: model.getParentId() });

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

/* EDIT FOLDER VIEW */

EditFolderView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#edit-folder-template').html();
        this.model.on("destroy", this.remove, this);
    },

    events: {
        "click #remove-folder": "removeFolder"
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        this.$('#folder-name').blur("Name", _.bind(this.updateModel, this));

        return this;
    },

    updateModel: function (e) {
        var field = e.data;
        var value = e.target.value;

        this.model.set(field, value);
    },

    removeFolder: function (e) {
        e.preventDefault();

        if (!confirm("Are you sure you want to delete this folder?"))
            return;

        this.model.destroy({
            wait: true
        });
    }
});