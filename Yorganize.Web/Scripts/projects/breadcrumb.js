
/* BREADCRUMB MODEL */

BreadcrumbModel = Backbone.Model.extend({
    defaults: {
        Parent: null,  // navigation folder's parent
        Selected: null // selected item
    }
});

/* BREADCRUMB VIEW */

BreadcrumbView = Backbone.View.extend({
    initialize: function (options) {
        this.template = $('#breadcrumb-template').html();
        this.parent = options.parent; // the navigation view
        if (!this.model)
            this.model = new BreadcrumbModel();
        this.model.bind("change", this.render, this);
    },

    events: {
        "click #nav-parent": "navigate",
        "click #sel-parent": "select"
    },

    render: function () {
        var parent = this.model.get("Parent");
        var selected = this.model.get("Selected");

        if (selected)
            selected.bind("change", this.render, this); // todo: set parent and selected via methods that will bind and unbind prev events

        var parentName = parent ? parent.get("Name") : null;
        var selectedName = selected ? selected.get("Name") : "nothing selected";

        var $content = _.template(this.template, { Parent: parentName, Selected: selectedName });
        this.$el.html($content);

       // console.log("breadcrumb:", parent, selected);

        return this;
    },

    navigate: function (e) {
        var parent = this.model.get("Parent");
        var folder = this.parent.folder ? this.parent.folder.id : null;
        var selected = this.parent.selected ? this.parent.selected.id : null;
        window.router.vent.trigger("navigate", parent.id, folder, selected);
        e.preventDefault();
        e.stopPropagation();
    },

    select: function (e) {
        var parent = this.model.get("Parent");
        window.router.vent.trigger("navigate", this.parent.model.id, parent.getParentId(), parent.id);
        e.preventDefault();
    }
});