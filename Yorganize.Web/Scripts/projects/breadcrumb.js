BreadcrumbModel = Backbone.Model.extend({
    defaults: {
        Parent: null,
        Selected: null
    }
});

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
        var parentName = parent ? parent.get("Name") : null;
        var selectedName = selected ? selected.get("Name") : "nothing selected";

        var $content = _.template(this.template, { Parent: parentName, Selected: selectedName });
        this.$el.html($content);

        return this;
    },

    navigate: function (e) {
        var folder = this.parent.folder ? this.parent.folder.id : null;
        var selected = this.parent.selected ? this.parent.selected.id : null;
        window.router.vent.trigger("navigate", this.model.id, folder, selected);
        e.preventDefault();
        e.stopPropagation();
    },

    select: function (e) {
        e.preventDefault();
    }
});