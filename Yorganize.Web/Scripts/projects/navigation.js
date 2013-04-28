
NavigationItemView = Backbone.View.extend({
    tagName: "li",
    initialize: function() {
        this.template = $('#navigation-item-template').html();
    },
    
    render: function() {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    }
});

NavigationView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#navigation-template').html();

        if (!this.model)
            this.model = new FolderModel();

        this.model.bind("change", this.render, this);
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        var contents = this.model.getContents(); // get folders and projects

        var $container = this.$('#items');
        _.each(contents, function (item) {
            var itemView = new NavigationItemView({model: item});
            $container.append(itemView.render().el);
        });
    }
});