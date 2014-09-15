!function ($) {
    "use strict";

    var simpleTabSelector = '[data-control=simpleTabs]'
    , SimpleTabs = function (element, options) {
        var self = this;
        var $el = $(element).addClass("simpleTabs-container");
        if (!options) {
            options = {};
        }
        $el.on('click', 'ul.tabs li a', function (e) {
            e.stopImmediatePropagation();
            e.preventDefault();
            self.selectTab.call(self, this);
        });
        $el.find('ul.tabs li a').each(function () {
            var id = $(this).attr('href');
            if (!options.startActiveId) {
                options.startActiveId = id;
            }
            if (options.startActiveId.indexOf('#') !== 0) {
                options.startActiveId = '#' + options.startActiveId;
            }
            if (options.startActiveId == id) {
                $(this).addClass('selected');
                $el.find(id).show();
                self.currentTab = this;
            } else {
                $(this).removeClass('selected');
                $el.find(id).hide();
            }
        });
        self.settings = options;
        self.settings.$element = $el;
    };

    SimpleTabs.prototype = {
        constructor: SimpleTabs,
        getAll: false,
        currentTab: undefined,
        selectTab: function (tab) {
            var id = $(tab).attr('href');
            var curId = $(this.currentTab).attr('href');
            if (this.currentTab !== id) {
                var lastTab = this.currentTab;
                $(this.currentTab).removeClass('selected');
                this.settings.$element.find(curId).hide();
                this.currentTab = tab;
                $(this.currentTab).addClass('selected');
                this.settings.$element.find(id).show();
                this.settings.$element.trigger('tabChanged', { oldTab: lastTab, newTab: this.currentTab });
            }
        }
    };


    /* FINDER PLUGIN DEFINITION
     * ========================== */

    function opts_from_el(el, prefix) {
        // Derive options from element data-attrs
        var data = $(el).data(),
			out = {}, inkey,
			replace = new RegExp('^' + prefix.toLowerCase() + '([A-Z])'),
			prefix = new RegExp('^' + prefix.toLowerCase());
        for (var key in data)
            if (prefix.test(key)) {
                inkey = key.replace(replace, function (_, a) { return a.toLowerCase(); });
                out[inkey] = data[key];
            }
        return out;
    }

    var old = $.fn.simpleTabs;

    $.fn.simpleTabs = function (option) {
        var retVal = undefined;
        var rootArgs = arguments;
        var eachReturn = this.each(function () {
            var $this = $(this)
              , data = $this.data('simpleTabs'),
				options = typeof option == 'object' && option;
            if (!data) {
                var elopts = opts_from_el(this, 'simpleTabs'),
					// Options priority: js args, data-attrs, defaults
					opts = $.extend({}, defaults, elopts, options);
                $this.data('simpleTabs', (data = new SimpleTabs(this, opts)));
            }
            if (typeof option == 'string' && typeof data[option] == 'function') {
                var args = Array.prototype.slice.call(rootArgs, 1);
                retVal = data[option](args);
            }

            retVal = data;
        });
        if (retVal) {
            return retVal;
        }
        return eachReturn;
    };

    $.fn.simpleTabs.Constructor = SimpleTabs;

    var defaults = $.fn.simpleTabs.defaults = {};

    /* FINDER NO CONFLICT
     * ==================== */

    $.fn.simpleTabs.noConflict = function () {
        $.fn.simpleTabs = old;
        return this;
    };


    /* APPLY TO STANDARD FINDER ELEMENTS
     * =================================== */
    $(simpleTabSelector).simpleTabs();

}(window.jQuery);