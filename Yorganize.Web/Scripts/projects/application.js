/* ROUTER */
var ProjectsRouter = Backbone.Router.extend({

    initialize: function (options) {
        console.log("initializing router...");
        this.data_loaded = false;

        // create collections and models
        this.folders = new FoldersCollection();

        // create views 
        this.navigationView = new NavigationView({ el: "#region-navigation" });
        this.breadcrumbView = new BreadcrumbView({ el: "#region-breadcrumb" });
        this.actionsView = new ActionsView({ el: "#region-actions" });

        // add event handlers
        this.vent = new Backbone.Wreqr.EventAggregator();
        addEventHandlers(this, this.vent);

    },

    load_data: function (router, folder, selected, callback) {
        // load data (fetch folders collection)
        this.folders.fetch({
            success: function (collection, response, options) {
                //console.log(collection);
                console.log("data loaded.");
                router.data_loaded = true;
                if (callback)
                    callback(folder, selected, collection);
            }, reset: true
        });

        return false;
    },

    routes: {
        "": "folder",
        "folder/:folder": "folder",
        "folder/:folder/selected/:selected": "folder",
        "selected/:selected": "selected"
    },

    folder: function (folder, selected) {
        this.set_up = false;
        console.log("mathed route: folder [folder:" + folder + ", selected:" + selected + "]");

        // load data
        this.data_loaded = this.data_loaded || this.load_data(this, folder, selected, this.setup);

        // do route setup if required
        if (this.data_loaded && !this.set_up)
            this.setup(folder, selected, this.folders);

    },

    selected: function (selected) {
        this.folder(null, selected);
    },

    setup: function (folder, selected, collection) {

        // folders loaded 


        // get current folder

        if (!folder) // if no folder selected
            folder = collection.findWhere({ ID: null }); // get root folder
        else
            folder = collection.findWhere({ ID: folder }); // find selected folder 


        // find selected item (folder or project)
        if (selected)
            selected = _.findWhere(folder.getContents(), { id: selected });

        console.log("setting up...", folder, selected);

        // set current folder on navigation view
        router.navigationView.model = folder;
        router.navigationView.selected = selected;
        router.navigationView.render();
        
        // set parrent folder on breadcrumb view
        router.breadcrumbView.model = folder.getParent();
        router.breadcrumbView.render();

        // set current folder on actions view
        router.actionsView.model = selected ? selected : folder;
        router.actionsView.render();

        this.set_up = true;
    }

});

function addEventHandlers(router, vent) {
    vent.on("navigate", function (folder, selected) {
        var route = "";
        if (folder) {
            route += "folder/" + folder;
            if (selected)
                route += "/";
        }

        if (selected)
            route += "selected/" + selected;

        console.log("navigating to: " + route);
        router.navigate(route, { trigger: true });
    });
}

$(function () {
    window.router = new ProjectsRouter();
    Backbone.history.start();
});