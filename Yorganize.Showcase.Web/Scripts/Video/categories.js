CategoryModel = Backbone.Model.extend({
    defaults: {
        Name: "",
        isActive: false,
        isEditing: false
    },

    idAttribute: "ID",
   
});

CategoriesCollection = Backbone.Collection.extend({
    model: CategoryModel,
    url: "Video/GetVideoCategories"
});

CategoryView = Backbone.View.extend({
    tagName: "li",

    initialize: function () {
        this.model.bind('change', this.render, this);
        this.model.bind('destroy', this.destroyView, this);
    },

    events: {
        "click a": "navigate",
        "click #save": "save",
        "click #cancel": "cancel"
    },

    render: function () {
        this.template = this.model.get("isEditing") ? $('#edit-category-template').html() : $('#category-template').html();
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
    },
    
    save: function (e) {
        this.model.set({ isEditing: false }, {silent: false});
        e.preventDefault();
    },
    
    cancel: function (e) {
        console.log("cancel");
        this.model.destroy();
        e.preventDefault();
    },
    
    destroyView: function() {
        this.remove();
    }
    
    
});

CategoriesView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#categories-template').html();
        if (!this.collection) this.collection = new CategoriesCollection();
        this.collection.bind('reset', this.render, this);
        this.collection.bind('add', this.render, this);
    },

    events: {
        "click #upload-video": "showUploadView",
        "click #add-category": "addCategory"
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
    },

    addCategory: function (e) {
        window.router.vent.trigger("category:add");
        e.preventDefault();
    }
});
