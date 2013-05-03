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
            project.unset("Actions");
        }, this);
        response.Projects = null;

        return response;
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

        return folders;
    }

});

FoldersCollection = Backbone.Collection.extend({
    model: FolderModel,
    url: "Projects/GetData"
});