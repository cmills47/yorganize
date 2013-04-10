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

    idAttribute: "ID"
});

VideoListModel = Backbone.Model.extend({
    defaults: {
        Category: "no category specified"
    },

    url: function () {
        return "Video/GetVideoList?category=" + this.get("Category");
    },

    parse: function (response) {
        response.Videos = new VideosCollection(response.Videos);
        return response;
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
    },

    events: {
        "click #edit": "edit",
        "click #play-video": "play"
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    },

    edit: function (e) {
        window.router.vent.trigger("video:edit", this.model);
        e.preventDefault();
    },

    play: function (e) {
        window.router.vent.trigger("video:play", this.model);
        e.preventDefault();
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
        var videos = this.model.get("Videos");
        if (videos && videos.length > 0)
            videos.each(function (video) {
                var videoView = new VideoView({ model: video, parent: this });
                $container.append(videoView.render().el);
            });
        else
            this.$el.append("<h4 class='muted'>There are no videos in this category. </h4>");
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

        console.log("render...");
        if (this.model.get("ID")) {
            console.log(this.model.id);
            $("#flowplayer").flowplayer({
                // configuration for this player
                splash: false
            });
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
        this.view = options.view;


        //$('#loader').show();
        return true;
    },

    showResponse: function (response, statusText, xhr, $form) {

        console.log("success:", response, statusText);
        /*
        $('#loader').hide();

        if (!response.valid) {
            this.view.display_error(response.message);
            return false;
        }

        // update model
        this.view.model.set(response.model);

        // display message - do not display message before setting the model because it will be erase by the render
        this.view.display_message(response.message);
        */
        return true;
    },

    errorRequest: function (response) {
        /*
        this.view.display_error(response.statusText);
        $('#loader').hide();
        */
        return false;
    }

});