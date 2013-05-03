ProjectModel = Backbone.Model.extend({
    defaults:
    {
        Name: "new project",
        Position: 0,
        Type: null,

        FolderID: null,
        Actions: new ActionsCollection(),

        itemType: "project"
    },

    idAttribute: "ID",

    parse: function (response, options) {
        console.log("parsing project item...");
        response.Actions = new ActionsCollection(response.Actions);
        return response;
    },

    getParentId: function () {
        return this.get("FolderID");
    },
    
    // returns a combined list of folders and projects directly under the current folder
    getContents: function () {
        return [this];
    }
});

ProjectsCollection = Backbone.Collection.extend({
    model: ProjectModel
});