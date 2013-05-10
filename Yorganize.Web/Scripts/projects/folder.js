
/* FOLDER MODEL*/

FolderModel = Backbone.Model.extend({
    defaults: {
        Name: "new folder",
        Position: 0,
        Type: null,
        ParentID: null,

        itemType: "folder"
    },

    Projects: new ProjectsCollection(),
    idAttribute: "ID",

    parse: function (response, options) {
        this.Projects = new ProjectsCollection(response.Projects);
        this.Projects.forEach(function (project) {
            project.Actions = new ActionsCollection(project.get("Actions"));
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
    // if self is true, includes the folder itself into the contents list
    getContents: function (self) {
        if (!this.collection) return null;

        var folders = this.collection.filter(function (folder) {
            var match = folder.get("ParentID") == this.get("ID") && folder.get("ID") != null;
            return match || (self && folder.get("ID") == this.get("ID"));
        }, this);

        [].push.apply(folders, this.Projects.models);

        folders.sort(function(a, b) {
            return a.get("Position") - b.get("Position");
        });

        return folders;
    }

});

/* FOLDERS COLLECTION*/

FoldersCollection = Backbone.Collection.extend({
    model: FolderModel,
    url: "Projects/GetData"
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

        this.model.destroy();
    }
});