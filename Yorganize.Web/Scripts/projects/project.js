ProjectModel = Backbone.Model.extend({
    defaults :
    {
        Type: "project",
        Name: "new project",
        Position: 0,
        FolderID: null,
        
        itemType: "project"
    },
    
    idAttribute: "ID",
    
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