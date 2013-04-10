CategoryModel = Backbone.Model.extend({
    defaults: {
        Name: "",
        isActive: false
    },

    idAttribute: "ID"
});

CategoriesCollection = Backbone.Collection.extend({
    model: CategoryModel,
    url: "Video/GetVideoCategories"
});

CategoryView = Backbone.View.extend({
    tagName: "li",

    initialize: function () {
        this.template = $('#category-template').html();
        this.model.bind('change', this.render, this);
    },

    events: {
        "click a": "navigate"
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        if (this.model.get("isActive"))
            this.$el.addClass("active");
        else this.$el.removeClass("active");

        return this;
    },

    navigate: function (e) {
        window.router.navigate("category/" + this.model.get("Name"), true);
        e.preventDefault();
    }
});

CategoriesView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#categories-template').html();
        if (!this.collection) this.collection = new CategoriesCollection();
        this.collection.bind('reset', this.render, this);
    },
    
    events: {
        "click #upload-video": "showUploadView"
    },
    
    render: function () {
        // render category region
        var $content = _.template(this.template, {});
        this.$el.html($content);

        // render categories
        var $container = this.$('#categories');
        this.collection.each(function (category) {
            var categoryView = new CategoryView({ model: category, parent: this });
            $container.append(categoryView.render().el);
        });
    },

    setActive: function (category) {
        _.each(this.collection.models, function (model) {
            if (model.get("Name") == category)
                model.set({ isActive: true });
            else
                model.set({ isActive: false });
        });
    },
    
    getActive: function () { //TODO with filter
       
        return this.collection.findWhere({ isActive: true });
    },
    
    showUploadView: function (e) {
        var activeCategory = this.getActive();
        if (activeCategory)
            window.router.vent.trigger("video:upload", activeCategory.id);
        e.preventDefault();
    }
});