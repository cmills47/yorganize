
/* ACTION MODEL */

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

/* ACTIONS COLLECTION */

ActionsCollection = Backbone.Collection.extend({
    model: ActionModel
});

/* ACTION VIEW*/

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

/* ACTION FOLDER VIEW */

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

/* ACTION PROJECT VIEW */

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

/* ACTIONS VIEW */

ActionsView = Backbone.View.extend({
    initialize: function () {
        this.template = $('#actions-template').html();
        // model can be a folder or a project
    },

    render: function () {
        console.log("rendering actions...");

        var $content = _.template(this.template, this.model.toJSON());
        this.$el.html($content);

        var contents = this.model.getContents(); // get folders and projects

        var $container = this.$('#items');
        renderItems($container, contents, 0);
        
    }
});

function renderItems($container, items, ident) {
    $container.empty();
    _.each(items, function (item) {
        var itemView;
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

/*  EDIT ACTION VIEW */

EditActionView = Backbone.View.extend({
    initialize: function () {

    },

    render: function () {

        return this;
    }
});