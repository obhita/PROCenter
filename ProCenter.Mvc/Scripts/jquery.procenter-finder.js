!function ($) {
    "use strict";

    var finderSelector = '[data-control=finder],.finder-wrapper'
    , Finder = function (element, options) {
        var self = this;
        var $el = $(element).on('click.finder.data-api', self.open)
            .on('blur.finder.data-api', function() {
                if (self.settings.autoClose) {
                    self.close.call($el[0]);
                    var value = $el.val();
                    if (!value || value == "") {
                        self.clearSelected();
                    }
                }
            }).on('keyup', function() {
                if (self.selectedData) {
                    self.selectedData = self.selectedElement = self.$selectedElement = null;
                    $el.trigger('selectionChanged', null);
                }
            });
        $el.on('selectionChanged', selectionChanged);
        self.settings = options;
        self.settings.$element = $el;
        if (self.settings.isServerSide) {
            self.data = new Array();
            var $wrapper = self.settings.$wrapper = $el.wrap(self.settings.templates.wrapper).parent();
            var $btn = $(self.settings.templates.allButton).appendTo($wrapper);
            var $popover = self.settings.$popover = $(self.settings.templates.popover).appendTo($wrapper);
            var $list = self.settings.$list = $(self.settings.templates.list).appendTo($popover);
            self.settings.$loading = $(self.settings.templates.loading).appendTo($popover);
            $list.on('mousedown', '.finder-item .finder-value', function () { self.select.apply($el[0], [$(this)]); });
            $list.on('DOMMouseScroll mousewheel', function (ev) {
                var $this = $(this),
                    scrollTop = this.scrollTop,
                    scrollHeight = this.scrollHeight,
                    height = $this.height(),
                    delta = (ev.type == 'DOMMouseScroll' ?
                        ev.originalEvent.detail * -40 :
                        ev.originalEvent.wheelDelta),
                    up = delta > 0;

                var prevent = function () {
                    ev.stopPropagation();
                    ev.preventDefault();
                    ev.returnValue = false;
                    return false;
                }

                if (!up && -delta > scrollHeight - height - scrollTop) {
                    // Scrolling down, but this will take us past the bottom.
                    $this.scrollTop(scrollHeight);
                    return prevent();
                } else if (up && delta > scrollTop) {
                    // Scrolling up, but this will take us past the top.
                    $this.scrollTop(0);
                    return prevent();
                }
            });
            $list.scroll(function (e) {
                e.stopImmediatePropagation();
                e.preventDefault();
                var scrollHeight = this.scrollHeight;
                var scrollOffset = $list.scrollTop();
                if (scrollOffset + $list.height() >= scrollHeight) {
                    self.nextPage.call($el[0]);
                }
            });
            $el.on('keyup', self.refresh);
            $btn.on('click', function (e) {
                e.stopImmediatePropagation();
                self.clearSelected();
                self.getAll = true;
                self.open.call($el[0]);
            });
        }

        var initialData = $el.data('finder-initial-data');
        if (initialData) {
            var initialSelected = initialData;// JSON.parse(initialData);
            selectInternal(self, initialSelected);
            if (self.settings.display) {
                $el.val(getChainedValue(self.selectedData, self.settings.display));
            } else {
                $el.val(self.selectedData);
            }
        }
    };

    Finder.prototype = {
        constructor: Finder,
        getAll: false,
        currentPage: 0,
        open: function () {
            var data = getFinder(this);
            var $el = data.settings.$element;
            if (!data.settings.$wrapper.hasClass('open')) {

                if ($el.is('.disabled, :disabled')) return;

                data.settings.$wrapper.addClass('open');

                $el.focus();

                data.refresh();
            }
        },
        close: function () {
            var data = getFinder(this);
            var $el = data.settings.$element;
            if (data.settings.$wrapper.hasClass('open')) {

                if ($el.is('.disabled, :disabled')) return;

                data.settings.$wrapper.removeClass('open');
                clearResults.call(this);

                var value = $el.val();
                if (!value || value == "") {
                    if (data.selectedData) {
                        data.selectedData = data.selectedElement = data.$selectedElement = null;
                        $el.trigger('selectionChanged', null);
                    }
                }
                if (!data.selectedData) {
                    $el.val('');
                }
            }
        },
        select: function ($element) {
            var data = getFinder(this);
            var selected = undefined;
            if (data.settings.isServerSide) {
                var index = $element.parent().index();
                selected = data.data[index];
            } else {
                selected = $element[0];
            }
            selectInternal(data,selected);
            $element.addClass("selected");
            if (data.$selectedElement) {
                data.$selectedElement.removeClass("selected");
            }
            data.$selectedElement = $element;
            data.selectedElement = $element[0];
        },
        updateResults: function() {
            var data = getFinder(this);
            data.settings.$list.empty();
            for (var i = 0; i < data.data.length; i++) {
                var item = data.data[i];
                var value = item;
                if (data.settings.display) {
                    value = getChainedValue(item,data.settings.display);
                }
                var $listItem = $(data.settings.templates.item).appendTo(data.settings.$list);
                var $value = $listItem.find('.finder-value');
                if ($value.length == 0) {
                    $value = $listItem;
                }
                $value.text(value);
            }
        },
        refresh: function() {
            var data = getFinder(this);
            var $el = data.settings.$element;
            data.currentPage = 0;
            clearResults.call(data);

            var value = $el.val();
            if (data.getAll || (value && value.length > 0)) {
                getServerData.call(data);
            }
        },
        nextPage: function() {
            var data = getFinder(this);
            if (data.totalCount > data.data.length) {
                getServerData.apply(data, [data.currentPage + 1]);
            }
        },
        clearSelected: function () {
            var data = getFinder(this);
            var $el = data.settings.$element;
            if (data.selectedData) {
                data.selectedData = null;
                data.$selectedElement = undefined;
                data.selectedElement = undefined;
                $el.trigger('selectionChanged', [null]);
            }
        }
    };
    
    function selectInternal(data, selected) {
        var parameters = new Array();
        parameters.push(selected);
        data.selectedData = selected;
        data.settings.$element.trigger('selectionChanged', parameters);
    }

    function getChainedValue(item, path) {
        var parts = path.split(".");
        var value = item;
        for (var i = 0; i < parts.length; i++) {
            value = value[parts[i]];
        }
        return value;
    }

    function getFinder(obj) {
        if ( obj.constructor && obj.constructor == Finder) {
            return obj;
        }
        return $(obj).data('finder');
    }

    function getServerData(page) {
        var data = getFinder(this);
        var $el = data.settings.$element;
        var value = $el.val();
        data.settings.$wrapper.attr('data-finder-ajax', "loading");
        var parameters = {
            page: page || 0,
            pageSize: 20,
            search: value
        };
        $.get(data.settings.url, parameters)
            .done(function(results) {
                data.settings.$wrapper.removeAttr('data-finder-ajax');
                data.data.push.apply(data.data,results.Data);
                data.currentPage = parameters.page;
                data.totalCount = results.TotalCount;
                data.updateResults.call(data.settings.$element[0]);
                if (results.TotalCount == 0) {
                    data.settings.$loading.text('No results found.');
                }
            })
            .fail(function(error) {
                data.settings.$wrapper.attr('data-finder-ajax', "fail");
                data.settings.$wrapper.attr('data-finder-ajax-error-message', error);
            })
            .complete(function() { data.getAll = false; });
    }

    function selectionChanged() {
        var data = getFinder(this);
        if (data) {
            var $el = data.settings.$element;
            if (!data.selectedData) {
                if (!$el.is(":focus")) {
                    $el.val('');
                }
            } else {
                if (data.settings.isServerSide) {
                    if (data.settings.display) {
                        $el.val(getChainedValue(data.selectedData, data.settings.display));
                    } else {
                        $el.val(data.selectedData);
                    }
                }
            }
        }
    }
    
    function clearResults() {
        var data = getFinder(this);
        data.settings.$list.empty();
        data.settings.$loading.text('');
        data.data.length = 0;
        data.currentPage = 0;
        data.totalCount = 0;
    }


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

    var old = $.fn.finder;

    $.fn.finder = function (option) {
        var retVal = undefined;
        var eachReturn = this.each(function () {
            var $this = $(this)
              , data = $this.data('finder'),
				options = typeof option == 'object' && option;
            if (!data) {
                var elopts = opts_from_el(this, 'finder'),
					// Options priority: js args, data-attrs, defaults
					opts = $.extend({}, defaults, elopts, options);
                $this.data('finder', (data = new Finder(this, opts)));
            }
            if (typeof option == 'string' && typeof data[option] == 'function') {
                var args = Array.prototype.slice.call(arguments, 1);
                retVal = data[option].apply($this, args);
            }
          
            retVal = data;
        });
        if (retVal) {
            return retVal;
        }
        return eachReturn;
    };

    $.fn.finder.Constructor = Finder;

    var defaults = $.fn.finder.defaults = {
        autoClose: true,
        isServerSide: true,
        display: undefined,
        pageSize: 20,
        totalCount: 0,
        templates: {
            wrapper: "<div class='finder-wrapper'></div>",
            popover: "<div class='finder-popover'></div>",
            list: "<ul class='finder-list'></ul>",
            item: "<li class='finder-item'><a href='#' class='finder-value'></a></li>",
            loading: "<div class='finder-loading-indicator'></div>",
            allButton: "<a class='finder-all-button'></a>"
        }
    };

    /* FINDER NO CONFLICT
     * ==================== */

    $.fn.finder.noConflict = function () {
        $.fn.finder = old;
        return this;
    };


    /* APPLY TO STANDARD FINDER ELEMENTS
     * =================================== */
    $(finderSelector).finder();

    //$(document)
    //  //.on('click.finder.data-api', clearList)
    //  .on('click.finder.data-api', '.finder form', function (e) { e.stopPropagation(); })
    //  //.on('click.finder.data-api', finderSelector, Finder.prototype.op)
    //  .on('keydown.finder.data-api', finderSelector + ', [role=menu]', Finder.prototype.keydown);

}(window.jQuery);