CategoryModel = Backbone.Model.extend({
   defaults: {
       Name: "",
       isActive: false
   },
   
   idAttribute: "ID"
});

CategoriesCollection = Backbone.Collection.extend({
    model: CategoryModel,
});

CategoryView = Backbone.View.extend({
    
});

CategoriesView = Backbone.View.extend({
    
});