/* ROUTER */
var ProjectsRouter = Backbone.Router.extend({

    initialize: function (options) {
        this.data_loaded = false;

        // create collections and models
        this.folders = new FoldersCollection();

        // create views 
        this.navigationView = new NavigationView({ el: "#region-navigation" });

        // add event handlers
        this.vent = new Backbone.Wreqr.EventAggregator();
        addEventHandlers(this.vent);
    },

    load_data: function (callback) {
        // load data (fetch folders collection)
        var router = this;
        this.folders.fetch({
            success: function (collection, response, options) {
                //console.log(collection);
                if (callback)
                    callback(collection);
            }, reset: true
        });

        return true;
    },

    routes: {
        "": "index",
        "folder/:folder": "index",
        "folder/:folder/project/:project": "index"
    },

    index: function (folder, project) {

        // load data
        this.data_loaded = this.data_loaded || this.load_data(function (collection) {
            // folders loaded 

            // if no folder selected
            if (!folder) {
                folder = collection.findWhere({ ID: null }); // get root folder
                router.navigationView.model = folder;
                router.navigationView.render();
                //console.log(folder);
                /*
                router.navigationItems.reset(collection.filter(function(folder) {
                    return (folder.get("ParentID") == null); // if no route get root folders
                }));
  */
            }
        });

        if (!this.categories_loaded)
            return;

        // render views
        // ...

        // set route data
        // ..

    }

});


function addEventHandlers(vent) {

}

$(function () {
    window.router = new ProjectsRouter();
    Backbone.history.start();
});