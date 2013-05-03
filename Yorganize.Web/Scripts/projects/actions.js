ActionModel = Backbone.Model.extend({
    defaults: {
        Name: "new action",
        Position: 0,
        Type: null,
        Status: null,
        ProjectID: null,

        itemType: "action"
    },

    idAttribute: "ID"
});

ActionsCollection = Backbone.Collection.extend({
    model: ActionModel
});

ActionView = Backbone.View.extend({
    initialize: function (options) {
        this.template = $('#actions-action-template').html();
    },

    render: function () {
        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        return this;
    }
});

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
            renderItems(this.$el, contents, this.ident + 30);

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

        // render actions
        var $container = this.$('#actions');
        this.model.Actions.forEach(function (action) {
            var actionView = new ActionView({ model: action });
            $container.prepend(actionView.render().el);
        });

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




