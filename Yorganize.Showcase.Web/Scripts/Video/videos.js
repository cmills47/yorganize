VideoSourceModel = Backbone.Model.extend({
    defaults: {
        Format: "",
        Url: ""
    },

    idAttribute: "ID",
});

VideoSourceCollection = Backbone.Collection.extend({
    model: VideoSourceModel,
});

VideoModel = Backbone.Model.extend({
    defaults: {
        Title: "",
        Description: "",
        Sources : new VideoSourceCollection()
        },
    
    idAttribute: "ID"
});

VideosCollection = Backbone.Collection.extend({
    model: VideoModel,
});

VideoView = Backbone.View.extend({

});

VideosView = Backbone.View.extend({

});