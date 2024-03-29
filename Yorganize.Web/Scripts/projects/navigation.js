﻿
/* NAVIGATION ITEM VIEW */

NavigationItemView = Backbone.View.extend({
    tagName: "li",

    initialize: function (options) {
        this.template = $('#navigation-item-template').html();
        this.parent = options.parent;
        this.model.bind("change", this.render, this);
        this.model.bind("destroy", this.remove, this);
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
        console.log("selecting ->", this.model.get("Name"));
        window.router.vent.trigger("navigate", this.parent.model.id, this.model.getParentId(), this.model.id);
        e.preventDefault();
    },

    navigate: function (e) {
        var folder = this.parent.folder ? this.parent.folder.id : null;
        var selected = this.parent.selected ? this.parent.selected.id : null;
        window.router.vent.trigger("navigate", this.model.id, folder, selected);
        e.preventDefault();
        e.stopPropagation();
    }

});

/* NAVIGATION VIEW */

NavigationView = Backbone.View.extend({
    initialize: function (options) {
        this.template = $('#navigation-template').html();
        this.folder = options.folder;
        this.selected = options.selected;
        // this.model is the current nav folder
    },

    events: {
        "click #sel-nav": "select", // select current folder
        "click #new-folder": "newFolder",
        "click #new-project": "newProject"
    },

    render: function () {
        console.log("rendering navigation...");

        var navModel = this.model.toJSON();
        navModel.IsSelected = this.model.id == this.selected.id;

        var $content = _.template(this.template, navModel);
        this.$el.html($content);

        var contents = this.model.getContents(); // get folders and projects

        var $container = this.$('#items');

        if (this.selected && this.selected.id == this.model.id) {
            this.$el.addClass("active");
        }

        _.each(contents, function (item) {
            var itemView = new NavigationItemView({
                model: item,
                parent: this,
                className: this.selected && this.selected.id == item.id ? "active" : ""
            }, this);

            //if (this.selected && this.selected.id == item.id)
            // itemView.className = "active";
            $container.append(itemView.render().el);
        }, this);
    },

    setModels: function (nav, folder, selected) {
        if (this.model) // unbind previous events
            this.model.off(null, null, this);

        // set models
        this.model = nav;
        this.folder = folder;
        this.selected = selected;
        
        // bind events to new model
        this.model.on("change", this.render, this);
        this.model.on("contents:changed", this.render, this);
        this.model.on("destroy", this.destroyed, this);
    },

    select: function (e) {
        window.router.vent.trigger("navigate", this.model.id, this.model.id, this.model.id);
        e.preventDefault();
    },

    newFolder: function (e) {
        window.router.vent.trigger("new-folder", this.model);
        e.preventDefault();
    },

    newProject: function (e) {
        window.router.vent.trigger("new-project", this.model);
        e.preventDefault();
    },
    
    destroyed: function(model) {
        window.router.vent.trigger("navigate:nav-destroyed", model);
    }

});