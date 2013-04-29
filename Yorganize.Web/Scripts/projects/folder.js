FolderModel = Backbone.Model.extend({
    defaults: {
        Type: "folder",
        Name: "new folder",
        Position: 0,
        ParentID: null,
        Projects: new ProjectsCollection(),
        
        itemType: "folder"
    },

    idAttribute: "ID",

    parse: function (response, options) {
        response.Projects = new ProjectsCollection(response.Projects);
        return response;
    },

    getParentId: function () {
        return this.get("ParentID");
    },

    getParent: function () {
        return this.collection.findWhere({ ID: this.getParentId() });
    },

    // returns a combined list of folders and projects directly under the current folder
    getContents: function () {
        if (!this.collection) return null;

        var folders = this.collection.filter(function (folder) {
            return folder.get("ParentID") == this.get("ID") && folder.get("ID") != null;
        }, this);

        [].push.apply(folders, this.get("Projects").models);

        return folders;
    }

});

FoldersCollection = Backbone.Collection.extend({
    model: FolderModel,
    url: "Projects/GetData"
});