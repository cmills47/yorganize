// MODELS

VideoModel = Backbone.Model.extend({
    defaults: {
        ID: null,
        Title: "",
        CategoryID: 0,
        Description: "",
        SourceMP4Url: "",
        SourceOGGUrl: "",
        SourceWEBMUrl: ""
    },

    idAttribute: "ID",
    
    methodToUrl: {
        "read": "",
        "create": "",
        "update": "",
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
    
    videos: function() {
        return this.get("Videos");
    }
}),

VideosCollection = Backbone.Collection.extend({
    model: VideoModel
});

//VIEWS

VideoView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#video-template').html();
        if (!this.model) this.model = new VideoModel();
        this.model.bind("change", this.render, this);
        this.model.bind("destroy", this.destroyView, this);
    },

    events: {
        "click #edit": "editVideo",
        "click #remove": "removeVideo",
        "click #play-video": "playVideo",
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    },

    editVideo: function (e) {
        window.router.vent.trigger("video:edit", this.model);
        e.preventDefault();
    },
    
    removeVideo: function(e) {
        if (confirm("Are you sure you want to remove this video?"))
            this.model.destroy();
        e.preventDefault();
    },

    playVideo: function (e) {
        window.router.vent.trigger("video:play", this.model);
        e.preventDefault();
    },
    
    destroyView: function(e) {
        this.remove();
        successMessage("Video has been removed.");
    }

});

VideosView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#video-list-template').html();
        if (!this.model) this.model = new VideoListModel();
        this.model.bind("change", this.render, this);
    },

    render: function () {
        var $content = _.template(this.template, { Category: this.model.get("Category") });
        this.$el.html($content);

        // render videos
        var $container = $('#videos');
        
        var videos = this.model.videos();
        if (videos && videos.length > 0)
            videos.each(function (video) {
                var videoView = new VideoView({ model: video, parent: this });
                $container.append(videoView.render().el);
            });
        else
            this.$el.append("<h3 class='muted'>There are no videos in this category. </h3>");
    },

    // adds a new video or updates an existing video model
    update: function (model) {
        var videos = this.model.videos();
        videos.add(model, { merge: true });
        this.render();
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
        
        if (this.model.get("ID")) {
            var $player = $("#flowplayer");
            $player.flowplayer({
                // configuration for this player
                autoPlay: true,

                // video will be buffered when splash screen is visible
                autoBuffering: true
            });

            //$api = flowplayer($player);
        }
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