/* ROUTER */
var VideoRouter = Backbone.Router.extend({

    initialize: function (options) {
        this.categories_loaded = this.videos_loaded = false;

        // create views 
        this.playerView = new VideoPlayerView({ el: '#video-player' });
        this.categoriesView = new CategoriesView({ el: '#categories-region' });
        this.videosView = new VideosView({ el: '#video-list' });

        this.vent = new Backbone.Wreqr.EventAggregator();
        addEventHandlers(this.vent);
    },

    load_categories: function (category) {
        this.categoriesView.collection.fetch({
            reset: true,
            data: { cache: false },
            success: function () {
                if (category)
                    window.router.categoriesView.setActive(category);
                else
                    window.router.categoriesView.navigateDefault(); // navigate to default category
            }
        });

        return true;
    },

    load_videos: function (video) {

        // fetch videos for current category
        this.videosView.model.fetch({
            wait: true,
            data: { cache: false },
            view: this.videosView,
            error: function (model, request, options) {
                if (request.status == 200 && request.responseText == "") // request ok, but no model returned
                    videosView.render();
            },
            success: function (model, request, options) {
                // find video and trigger play
                if (video)
                    options.view.play(video);
                else
                    window.router.videosView.navigateDefault(); // navigate to default video
            }
        });

        return true;
    },

    routes: {
        "": "index",
        "category/:category": "index",
        "category/:category/video/:video": "index"
    },

    index: function (category, video) {

        // load categories
        this.categories_loaded = this.categories_loaded || this.load_categories(category);

        if (!this.categories_loaded)
            return;

        if (!category) { // render empty videos view if no category specified and return
            this.videosView.render();
            return;
        }

        // set category data
        this.categoriesView.setActive(category);
        this.videosView.model.set({ Category: category }, { silent: true });

        // load videos
        this.videos_loaded = this.load_videos(video);

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