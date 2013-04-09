/* ROUTER */
var VideoRouter = Backbone.Router.extend({

    initialize: function (options) {
        this.data_loaded = false;
        this.vent = new Backbone.Wreqr.EventAggregator();
    },

    load_data: function (category) {

        console.log("loading data for", category);
        // fetch models and collections
        this.categoriesView = new CategoriesView({ el: '#categories-region' });

        this.categoriesView.collection.fetch({
            reset: true,
            success: function () {

                if (category)
                    window.router.categoriesView.setActive(category);
            }
        });

        addEventHandlers(this.vent);
        
        return true;
    },

    set_urls: function (/*params*/) {
    },

    routes: {
        "": "index",
        "category/:category": "index"
    },

    index: function (category) {

        // this.set_urls();
        this.data_loaded = this.data_loaded || this.load_data(category);

        if (!this.data_loaded)
            return;

        this.categoriesView.setActive(category);

        // create views 
        this.videosView = new VideosView({ el: '#video-list' });
        this.videosView.model.set({ Category: category }, { silent: true });
        this.videosView.model.fetch({
            wait: true,

            error: function (model, request, options) {
                if (request.status == 200 && request.responseText == "") // request ok, but no model returned
                    videosView.render();
            }
        });
    },

});

function addEventHandlers(vent) {
    // display upload view for new video
    vent.on("video:upload", function () {
        var content = new EditVideoView().render().el;
        $('#edit-video').hide().html(content).fadeIn("slow");
    });

    // display edit view for existing video
    vent.on("video:edit", function (video) {
        var content = new EditVideoView({ model: video }).render().el;
        $('#edit-video').hide().html(content).fadeIn("slow");
    });
}

$(function () {
    window.router = new VideoRouter();
    Backbone.history.start();
});