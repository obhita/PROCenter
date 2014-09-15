!function ($) {
    "use strict";

    var InfoButtons = function (element, options) {
        var self = this;
        var $el = $(element);

        if (!$.fn.popover) {
            throw 'InfoButtons plugin requires a popover plugin to function.';
        }

        self.settings = options;
        self.settings.$element = $el;

        $('body').on('click', function(e) {
            $('.info-buttons-btn').each(function() {
                if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                    $(this).popover('hide');
                }
            });
        });

        $el.find('li').each(function() {
            var $li = $(this);
            var infoContainer = $('#' + $li.data('id'));
            if (infoContainer.length > 0) {
                var infoButton = $(self.settings.buttonTemplate).data('content',$li.text());
                infoContainer.prepend(infoButton);
                infoButton.popover({
                    placement: function (tip, popElement, offsetTop) {
                        var $element, above, actualHeight, actualWidth, below, boundBottom, boundLeft, boundRight, boundTop, elementAbove, elementBelow, elementLeft, elementRight, isWithinBounds, left, pos, right;
                        isWithinBounds = function (elementPosition) {
                            return boundTop < elementPosition.top && boundLeft < elementPosition.left && boundRight > (elementPosition.left + actualWidth) && boundBottom > (elementPosition.top + actualHeight);
                        };
                        if (!offsetTop) {
                            offsetTop = 0;
                        }
                        $element = $(popElement);
                        pos = $.extend({}, $element.position(), {
                            width: popElement.offsetWidth,
                            height: popElement.offsetHeight
                        });
                        actualWidth = 283;
                        actualHeight = 117;
                        boundTop = 0;
                        boundLeft = 0;
                        boundRight = boundLeft + $element.scrollParent().width();
                        boundBottom = boundTop + $element.scrollParent().height();
                        elementAbove = {
                            top: pos.top - actualHeight,
                            left: pos.left + pos.width / 2 - actualWidth / 2
                        };
                        elementBelow = {
                            top: pos.top + pos.height,
                            left: pos.left + pos.width / 2 - actualWidth / 2
                        };
                        elementLeft = {
                            top: (pos.top + pos.height - offsetTop) / 2 - actualHeight / 2,
                            left: pos.left - actualWidth
                        };
                        elementRight = {
                            top: pos.top + pos.height / 2 - actualHeight / 2,
                            left: pos.left + pos.width
                        };
                        above = isWithinBounds(elementAbove);
                        below = isWithinBounds(elementBelow);
                        left = isWithinBounds(elementLeft);
                        right = isWithinBounds(elementRight);
                        if (above) {
                            return "top";
                        } else {
                            if (below) {
                                return "bottom";
                            } else {
                                if (left) {
                                    return "left";
                                } else {
                                    if (right) {
                                        return "right";
                                    } else {
                                        return "left shift";
                                    }
                                }
                            }
                        }
                    }
                });
            }
        });
    };

    InfoButtons.prototype = {
        constructor: InfoButtons,
    };


    /* INFOBUTTONS PLUGIN DEFINITION
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

    var old = $.fn.infoButtons;

    $.fn.infoButtons = function (option) {
        var retVal = undefined;
        var outerArgs = arguments;
        var eachReturn = this.each(function () {
            var $this = $(this)
              , data = $this.data('infoButtons'),
				options = typeof option == 'object' && option;
            if (!data) {
                var elopts = opts_from_el(this, 'infoButtons'),
					// Options priority: js args, data-attrs, defaults
					opts = $.extend({}, defaults, elopts, options);
                $this.data('infoButtons', (data = new InfoButtons(this, opts)));
            }
            if (typeof option == 'string' && typeof data[option] == 'function') {
                var args = Array.prototype.slice.call(outerArgs, 1);
                retVal = data[option].apply($this, args);
            }

            retVal = data;
        });
        if (retVal) {
            return retVal;
        }
        return eachReturn;
    };

    $.fn.infoButtons.Constructor = InfoButtons;

    var defaults = $.fn.infoButtons.defaults = {
        buttonTemplate: '<a href="#" class="info-buttons-btn" data-toggle="popover"><i class="info-icon"></i></a>'
    };

    /* INFOBUTTONS NO CONFLICT
     * ==================== */

    $.fn.infoButtons.noConflict = function () {
        $.fn.infoButtons = old;
        return this;
    };
    
}(window.jQuery);