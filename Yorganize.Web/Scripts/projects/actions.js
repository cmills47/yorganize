ActionFolderView = Backbone.View.extend({
    initialize: function (options) {
        this.template = $('#actions-folder-template').html();
        this.ident = this.options.ident || 0;
    },

    render: function () {
        this.model.set("ident", this.ident);
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        var contents = this.model.getContents(); // get folders and projects
        if (contents)
            renderItems(this.$el, contents, this.ident+30);

        return this;
    }
});

ActionProjectView = Backbone.View.extend({
    initialize: function (options) {
        this.template = $('#actions-project-template').html();
        this.ident = this.options.ident || 0;
    },

    render: function () {
        this.model.set("ident", this.ident);
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    }
});

ActionsView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#actions-template').html();
        //  this.model.bind("change", this.render, this);
    },

    render: function () {
        console.log("rendering actions...");

        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        var contents = this.model.getContents(); // get folders and projects

        var $container = this.$('#items');
        $container.empty();
        renderItems($container, contents, 0);
    }
});

function renderItems($container, items, ident) {
    _.each(items, function (item) {
        var itemView = null;
        switch (item.get("itemType")) {
            case "folder":
                itemView = new ActionFolderView({ model: item, ident: ident });
                break;
            case "project":
                itemView = new ActionProjectView({ model: item, ident: ident });
                break;
            default:
                throw "unknown type: " + item.get("Type");
        }

        $container.append(itemView.render().el);
        
        // checkboxes
        $('input[type=checkbox]').iCheck({
            labelHover: false,
            cursor: true,
            checkboxClass: 'icheckbox_square-blue',
        });
    });
}




