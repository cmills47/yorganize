// MODELS

VideoModel = Backbone.Model.extend({
    defaults: {
        ID: null,
        Title: "",
        Order: 0,
        CategoryID: 0,
        Description: "",
        SourceMP4Url: null,
        SourceOGGUrl: null,
        SourceWEBMUrl: null
    },

    idAttribute: "ID",
    hasSources: function () {
        return this.get("SourceMP4Url") || this.get("SourceOGGUrl") || this.get("SourceWEBUrl");
    },

    methodToUrl: {
        "read": "",
        "create": "",
        "update": "Video/UpdateVideo",
        "delete": "Video/RemoveVideo",
    },

    sync: function (method, model, options) {
        options = options || {};
        options.url = model.methodToUrl[method.toLowerCase()];

        switch (method) {
            case 'delete':
                options.url += '/' + this.id;
                break;
        }

        Backbone.sync(method, model, options);
    }
});

VideoListModel = Backbone.Model.extend({
    defaults: {
        Category: "No category selected"
    },

    url: function () {
        return "Video/GetVideoList?category=" + this.get("Category");
    },

    parse: function (response) {
        response.Videos = new VideosCollection(response.Videos);
        return response;
    },

    videos: function () {
        return this.get("Videos");
    }
}),

VideosCollection = Backbone.Collection.extend({
    model: VideoModel
});

//VIEWS

VideoView = Backbone.View.extend({
    tagName: "li",
    className: "span3",

    initialize: function (options) {
        this.parent = options.parent;
        this.template = $('#video-template').html();
        if (!this.model) this.model = new VideoModel();
        this.model.bind("change", this.render, this);
        this.model.bind("destroy", this.destroyView, this);
    },

    events: {
        "click #edit": "editVideo",
        "click #remove": "removeVideo",
        "click #play-video": "playVideo",
        "click #move": "moveVideo"
    },

    render: function () {
        this.el.id = this.model.get('ID');
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    },

    moveVideo: function (e) {
        e.preventDefault();
    },

    editVideo: function (e) {
        window.router.vent.trigger("video:edit", this.model);
        e.preventDefault();
    },

    removeVideo: function (e) {
        var view = this;
        if (confirm("Are you sure you want to remove this video?")) {
            this.model.destroy({
                success: function (model, response, options) {
                    view.parent.trigger("videoRemoved", model);
                }
            });
        }

        e.preventDefault();
    },

    playVideo: function (e) {
        window.router.vent.trigger("video:play", this.model);
        e.preventDefault();
    },

    destroyView: function (e) {
        this.remove();
        successMessage("Video has been removed.");
    }

});

VideosView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#video-list-template').html();
        if (!this.model) this.model = new VideoListModel();
        this.model.bind("change", this.render, this);
        this.on("videoRemoved", this.videoRemoved, this);
    },

    render: function () {
        var $content = _.template(this.template, { Category: this.model.get("Category") });
        this.$el.html($content);

        // render videos
        var $container = $('#videos');
        var self = this;
        var videos = this.model.videos();
        if (videos && videos.length > 0)
            videos.each(function (video) {
                var videoView = new VideoView({ model: video, parent: self });
                $container.append(videoView.render().el);
            });
        else
            this.$el.append("<h3 class='muted'>There are no videos in this category. </h3>");

        // apply sorting
        $container = this.$('#videos.sortable');
        $container.sortable({
            handle: "#move",
            helper: "clone",
            update: _.bind(function (evt, ui) {
                var id = $(ui.item).attr('id');
                var position = $(ui.item).index();
                this.move(id, position);
            }, this)
        });
    },

    // adds a new video or updates an existing video model
    update: function (model) {
        var videos = this.model.videos();
        videos.add(model, { merge: true });
        this.render();
    },

    move: function (id, position) {
        var order = position + 1;
        var videos = this.model.videos();
        var model = videos.get(id);

        // get affected items
        var unordered = _.filter(videos.models, function (item) {
            var itemOrder = item.get("Order");

            if (order < model.get("Order")) // order decreased
                return itemOrder >= order && itemOrder < model.get("Order");
            else if (order > model.get("Order")) // order increased
                return itemOrder > model.get("Order") && itemOrder <= order;
            else return false;
        });

        // update items order
        if (order < model.get("Order")) // order decreased
            _.each(unordered, function (item) {
                item.set({ Order: item.get("Order") + 1 });
            });
        else _.each(unordered, function (item) {
            item.set({ Order: item.get("Order") - 1 });
        });

        // save model
        model.set({ Order: order });
        model.save();
    },

    videoRemoved: function (model) {
        // update order 
        var videos = this.model.videos();
        // decrement order for models that have same parent and greater order number
        _.each(
            _.filter(videos.models, function (item) { return item.get("Order") > model.get("Order"); }, this),
           function (model) { // inner model
               model.set({ Order: model.get("Order") - 1 });
           }, this);
        //TODO: show no videos message if collection is empty
    }
});

VideoPlayerView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#video-player-template').html();
        if (!this.model) this.model = new VideoModel();
        this.model.bind("change", this.render, this);
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);


        /*
        var $player = $("#flowplayer");
        $player.flowplayer({
            // configuration for this player
            autoPlay: true,

            // video will be buffered when splash screen is visible
            autoBuffering: true
        });

        //$api = flowplayer($player);
        */
        /*
        if (this.model.hasSources())
        _V_("video-player-object").ready(function () {

            var myPlayer = this;

            // EXAMPLE: Start playing the video.
            myPlayer.play();

        });
    */
        return this;
    }
});

EditVideoView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#edit-video-template').html();
        if (!this.model) this.model = new VideoModel();
        this.model.bind("change", this.render, this);
    },

    events: {
        "click #cancel": "cancel",
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        // create upload control for post image
        this.$('input[type=file]').kendoUpload({
            async: {
                saveUrl: '',
                // removeUrl: "remove",
                autoUpload: false
            },
            multiple: false
            //showFileList: false,
        });

        this.$el.tooltip({
            selector: "[class*=tooltip]"
        });

        // bind upload form
        var options = {
            dataType: 'json',
            beforeSubmit: this.beginRequest,
            success: this.showResponse,
            // error: this.errorRequest,
            clearForm: false,
            view: this
        };

        var $form = $(this.el).find('form');
        validateForm($form).ajaxForm(options);

        return this;
    },

    beginRequest: function (formData, jqForm, options) {
        var view = options.view;
        view.showProgress();

        return true;
    },

    showResponse: function (response, statusText, xhr, $form) {

        // update status
        successMessage("Video sucessfuly uploaded/updated.");

        // remove this view
        this.view.remove();

        // update model
        if (statusText == "success")
            window.router.vent.trigger("video:updated", response);

        return true;
    },

    errorRequest: function (response) {
        console.log("error:", response);
        return false;
    },

    cancel: function (e) {
        this.remove();
        e.preventDefault();
    },

    showProgress: function () {

        this.$('#edit-form').hide();
        this.$('#progress').show();
    }

});