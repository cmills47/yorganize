BreadcrumbView = Backbone.View.extend({
    initialize: function (options) {
        this.template = $('#breadcrumb-template').html();
        this.parent = options.parent;
        // this.model.bind("change", this.render, this);
    },

    events: {
        "click #nav-parent": "navigate"
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    },

    navigate: function (e) {
        window.router.vent.trigger("navigate", this.model.id);
        e.preventDefault();
        e.stopPropagation();
    }
});

NavigationItemView = Backbone.View.extend({
    tagName: "li",

    initialize: function (options) {
        this.template = $('#navigation-item-template').html();
        this.parent = options.parent;
        this.model.bind("change", this.render, this);
    },

    events: {
        "click #sel-item": "select",
        "click #nav-item": "navigate"
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    },

    select: function (e) {
        window.router.vent.trigger("navigate", this.model.getParentId(), this.model.id);
        e.preventDefault();
    },

    navigate: function (e) {
        window.router.vent.trigger("navigate", this.model.id);
        e.preventDefault();
        e.stopPropagation();
    }

});

NavigationView = Backbone.View.extend({
    initialize: function (options) {
        this.template = $('#navigation-template').html();
        this.selected = options.selected;
        // this.model.bind("change", this.render, this);
    },

    render: function () {
        console.log("rendering navigation...");

        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        var contents = this.model.getContents(); // get folders and projects

        var $container = this.$('#items');

        _.each(contents, function (item) {
            var itemView = new NavigationItemView({
                model: item,
                parent: this,
                className: this.selected && this.selected.id == item.id ? "active" : ""
            });

            //if (this.selected && this.selected.id == item.id)
            itemView.className = "active";
            $container.append(itemView.render().el);
        }, this);
    },

});