
VideoModel = Backbone.Model.extend({
    defaults: {
        ID: null,
        Title: "",
        Description: "",
        SourceMP4Url: "",
        SourceOGGUrl: "",
        SourceWEBMUrl: ""
    },

    idAttribute: "ID"
});

VideoListModel = Backbone.Model.extend({
    defaults: {
        Category: "not specified"
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

VideoView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#video-template').html();
        if (!this.model) this.model = new VideoModel();
        this.model.bind("change", this.render, this);
    },

    events: {
        "click #edit": "edit"
    },
    
    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    },
    
    edit: function (e) {
        window.router.vent.trigger("video:edit", this.model);
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
        console.log("rendering video list...");
        var $content = _.template(this.template, { Category: this.model.get("Category") });
        this.$el.html($content);

        // render videos
        var $container = $('#videos');
        this.model.get("Videos").each(function (video) {
            var videoView = new VideoView({ model: video, parent: this });
            $container.append(videoView.render().el);
        });
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
            multiple: false,
            //showFileList: false,
            //complete: onUploadComplete,
            //error: onUploadError,
            //success: onUploadSuccess,
            //upload: onUpload
        });

        this.$el.popover({
            selector: "[class*=_popover]",
            trigger: "hover"
        });
        
        return this;
    }
});