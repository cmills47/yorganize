
/* PROJECTS ROUTER */

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
        this.editView = null;

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

    /* ROUTE SETUP */

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

        selected = _.findWhere(folder.getContents(true), { id: selected ? selected : null });

        console.log("setting up... " /*, nav, folder, selected*/);

        // set current folder on navigation view
        router.navigationView.setModels(nav, folder, selected);
        router.navigationView.render();

        // set parrent folder on breadcrumb view

        router.breadcrumbView.model.set({ Parent: nav.getParent() });
        router.breadcrumbView.model.set({ Selected: selected });
        router.breadcrumbView.render();

        // set current folder on actions view
        router.actionsView.setModel(selected ? selected : folder);
        router.actionsView.render();

        this.set_up = true;
    },


    edit: function (model) {

        // unbinds and closes current editing view (if any)
        if (this.editView) {
            this.editView.remove();
        }

        var $region = $('#region-edit');

        if (model) {
            switch (model.get("itemType")) {
                case "folder":
                    this.editView = new EditFolderView({ model: model })
                        .render();
                    break;
                case "project":
                    this.editView = new EditProjectView({ model: model })
                        .render();
                    break;
                case "action":
                    this.editView = new EditActionView({ model: model })
                    .render();
                    break;
                default:
                    errorMessage("Invalid item type supplied for editing");
                    break;
            }

            $region.html(this.editView.el);
        } else $region.html("");

    }

});

/* ROUTER EVENT HANDLERS */

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
    
    vent.on("edit:open", function (model) {
        router.edit(model);
    });

    vent.on("new-folder", function (parent) {
        var folder = new FolderModel({
            Name: "new folder",
            ParentID: parent.id
        });

        folder.save(null, {
            success: function (model, response, options) {
                parent.collection.add(folder);
                vent.trigger("edit:open", folder);
            },
            error: function (model, request, options) {
                errorMessage("Failed to create new folder.");
            }
        });
    });

    vent.on("new-project", function (parent) {
        var project = new ProjectModel({
            Name: "new project",
            FolderID: parent.id
        });

        project.Folder = parent;

        project.save(null, {
            success: function (model, response, options) {
                parent.Projects.add(project);
                vent.trigger("edit:open", project);
            },
            error: function (model, request, options) {
                errorMessage("Failed to create new project.");
            }
        });
    });

    vent.on("new-action", function (parent) {
        var action = new ActionModel({
            Name: "new action",
            ProjectID: parent.id
        });

        action.save(null, {
            success: function (model, response, options) {
                parent.Actions.add(action);
                vent.trigger("edit:open", action);
            },
            error: function (model, request, options) {
                errorMessage("Failed to create new action.");
            }
        });
    });
    
}

/* ROUTER REGISTRATION */

$(function () {
    window.router = new ProjectsRouter();
    Backbone.history.start();
});