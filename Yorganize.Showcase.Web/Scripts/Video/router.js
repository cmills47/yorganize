/* ROUTER */
var VideoRouter = Backbone.Router.extend({

    initialize: function (options) {
        this.data_loaded = false;
        this.vent = new Backbone.Wreqr.EventAggregator();
    },

    load_data: function (category) {

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

    routes: {
        "": "index",
        "category/:category": "index"
    },

    index: function (category) {

        this.data_loaded = this.data_loaded || this.load_data(category);

        if (!this.data_loaded)
            return;

        // create views 
        this.playerView = new VideoPlayerView({ el: '#video-player' });

        this.videosView = new VideosView({ el: '#video-list' });

        if (!category) {
            this.videosView.render();
            return;
        }

        // set category data

        this.categoriesView.setActive(category);
        this.videosView.model.set({ Category: category }, { silent: true });
        this.videosView.model.fetch({
            wait: true,

            error: function (model, request, options) {
                if (request.status == 200 && request.responseText == "") // request ok, but no model returned
                    videosView.render();
            }
        });
    }

});

function addEventHandlers(vent) {
    vent.on("category:add", function () {
        router.categoriesView.collection.add(new CategoryModel({ Name: "category", isActive: false, isEditing: true }));
    });

    // display upload view for new video
    vent.on("video:upload", function (categoryID) {
        var content = new EditVideoView({ model: new VideoModel({ CategoryID: categoryID }) }).render().el;
        $('#edit-video').hide().html(content).fadeIn("slow");
    });

    // display edit view for existing video
    vent.on("video:edit", function (video) {
        var content = new EditVideoView({ model: video }).render().el;
        $('#edit-video').hide().html(content).fadeIn("slow");
    });

    // play video
    vent.on("video:play", function (video) {
        // console.log("playing", video);

        window.router.playerView.model.set(video.toJSON());
    });

    // video uploaded/updated
    vent.on("video:updated", function (video) {
        window.router.videosView.update(video);
    });
}

$(function () {
    window.router = new VideoRouter();
    Backbone.history.start();
});