LinkedCollection = Backbone.Collection.extend({
    initialize: function (options) {
        if (!options.collection)
            throw "No main collection supplied.";
        
        this.collection = options.collection;
    }
    

})