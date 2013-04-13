CategoryModel = Backbone.Model.extend({
    defaults: {
        ID: null,
        Name: "",
        isActive: false,
        isEditing: false
    },

    idAttribute: "ID",

    methodToUrl: {
        "read": "",
        "create": "Video/CreateCategory",
        "update": "Video/UpdateCategory",
        "delete": "Video/RemoveCategory",
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
        "click .category-navigate": "navigate",
        "click #save": "save",
        "click #cancel": "cancel",
        "click #edit": "edit",
        "click #remove": "removeCategory"
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
        if (e)
            e.preventDefault();
    },

    save: function (e) {
        var self = this;
        this.model.save({ Name: this.$el.find('#category-name').val() },
            {
                wait: true,
                success: function (model) {
                    model.set({ isEditing: false });
                    self.navigate();
                }
            });
        e.preventDefault();
    },

    cancel: function (e) {
        console.log("cancel");
        this.model.destroy();
        e.preventDefault();
    },

    edit: function (e) {
        this.model.set({ isEditing: true });
        e.preventDefault();
    },

    removeCategory: function (e) {
        this.model.destroy();
    },

    destroyView: function () {
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

            if (!this.defaultView) // store first category view as default
                this.defaultView = categoryView;
        }, this);
    },

    setActive: function (category) {
        var found = false;
        _.each(this.collection.models, function (model) {
            if (model.get("Name") == category) {
                found = true;
                model.set({ isActive: true });
                // show upload button
                this.$('#upload-video').show();
            } else
                model.set({ isActive: false });
        }, this);

        if (!found)
            this.$('#upload-video').hide(); // category does not exist
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
    },

    // navigate to default category if exists
    navigateDefault: function () {
        if (this.defaultView)
            this.defaultView.navigate();
    }
});
