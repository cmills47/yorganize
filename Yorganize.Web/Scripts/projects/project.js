ProjectModel = Backbone.Model.extend({
    defaults :
    {
        Type: "project",
        Name: "new project",
        Position: 0
    }
});

ProjectsCollection = Backbone.Collection.extend({
   model: ProjectModel 
});