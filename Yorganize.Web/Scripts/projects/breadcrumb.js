
/* BREADCRUMB MODEL */

BreadcrumbModel = Backbone.Model.extend({
    defaults: {
        Nav: null,     // the currently nav folder
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
        "click #sel-parent": "select",
        "click #edit-current": "edit"
    },

    render: function () {
        console.log("rendering breadcrumb...");

        var parent = this.model.get("Parent").get("Name");
        var selected = this.model.get("Selected").get("Name");
        var isRoot = this.model.get("Nav").id == this.model.get("Parent").id;
        var isSelected = this.model.get("Parent").id == this.model.get("Selected").id;

        var $content = _.template(this.template, { Parent: parent, Selected: selected, IsRoot: isRoot, IsSelected: isSelected });
        this.$el.html($content);

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
    },

    edit: function (e) {
        var selected = this.model.get("Selected");
        if (selected && selected.id)
            window.router.vent.trigger("edit:open", selected);
        e.preventDefault();
    },

    setModels: function (nav, sel, render) {
        var parent = this.model.get("Parent");
        var selected = this.model.get("Selected");

        // remove previous bindings if set
        if (parent)
            parent.off("change", this.render, this);

        if (selected)
            selected.off("change", this.render, this);

        // set current models
        this.model.set({ Nav: nav, Parent: nav.getParent(), Selected: sel }, { silent: !render });

        // add bindings for current models
        if (parent)
            parent.on("change", this.render, this);

        if (selected)
            selected.on("change", this.render, this);
    }
});