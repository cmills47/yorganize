/* ROUTER */
var ProjectsRouter = Backbone.Router.extend({

    initialize: function () {
        console.log("initializing router...");
        this.data_loaded = false;

        // create collections and models
        this.folders = new FoldersCollection();

        // create views 
        this.navigationView = new NavigationView({ el: "#region-navigation" });
        this.breadcrumbView = new BreadcrumbView({ el: "#region-breadcrumb", parent: this.navigationView });
        this.actionsView = new ActionsView({ el: "#region-actions" });

        // add event handlers
        this.vent = new Backbone.Wreqr.EventAggregator();
        addEventHandlers(this, this.vent);
    },

    load_data: function (router, nav, folder, selected, callback) {
        // load data (fetch folders collection)
        this.folders.fetch({
            success: function (collection, response, options) {
                //console.log(collection);
                console.log("data loaded.");
                router.data_loaded = true;
                if (callback)
                    callback(nav, folder, selected, collection);
            }, reset: true
        });

        return false;
    },

    routes: {
        "": "nav", // root folder without selected item
        "nav/:nav": "nav", // specific folder without selected item
        "nav/:nav/folder/:folder/selected/:selected": "nav", // specific folder with selected item from another folder
        "selected/:selected": "selected", // root folder with selected item
        "nav/:nav/selected/:selected": "navselected", // specific folder with selected item from root
        "folder/:folder/selected/:selected": "folderselected", // root folder
    },

    nav: function (nav, folder, selected) {
        this.set_up = false;
        console.log("mathed route: " + "nav: " + nav + " folder:" + folder + " selected:" + selected);

        // load data
        this.data_loaded = this.data_loaded || this.load_data(this, nav, folder, selected, this.setup);

        // do route setup if required
        if (this.data_loaded && !this.set_up)
            this.setup(nav, folder, selected, this.folders);
    },

    selected: function (selected) {
        this.nav(null, null, selected);
    },

    navselected: function (nav, selected) {
        this.nav(nav, null, selected);
    },

    folderselected: function (folder, selected) {
        this.nav(null, folder, selected);
    },

    setup: function (nav, folder, selected, collection) {

        // folders loaded 

        // set navigation to nav folder or root folder
        if (!nav)
            nav = collection.findWhere({ ID: null }); // get root nav folder
        else
            nav = collection.findWhere({ ID: nav }); // find selected nav folder 

        if (!nav)
            throw "Navigation folder not found.";

        // set selection to folder or selected

        if (!folder) // if no folder selected
            folder = collection.findWhere({ ID: null }); // get root folder
        else
            folder = collection.findWhere({ ID: folder }); // find selected folder 

        if (!folder)
            throw "Selection folder not found.";

        // find selected item (folder or project)
        if (selected)
            selected = _.findWhere(folder.getContents(true), { id: selected });

        console.log("setting up...");

        // set current folder on navigation view
        router.navigationView.model = nav;
        router.navigationView.folder = folder;
        router.navigationView.selected = selected;
        router.navigationView.render();

        // set parrent folder on breadcrumb view

        router.breadcrumbView.model.set({ Parent: nav.getParent() });
        router.breadcrumbView.model.set({ Selected: selected });
        router.breadcrumbView.render();

        // set current folder on actions view
        router.actionsView.model = selected ? selected : folder;
        router.actionsView.render();

        this.set_up = true;
    }

});

function addEventHandlers(router, vent) {
    vent.on("navigate", function (nav, folder, selected) {

        var route = "";
        if (nav) {
            route += "nav/" + nav;
            if (folder || selected)
                route += "/";
        }
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