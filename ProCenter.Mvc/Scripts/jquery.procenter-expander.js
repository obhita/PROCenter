(function ($) {
    var fixSizes = window.fixSizes || function () { };
    var defaults = {
        direction: 'down',
        reverseDirectionMap: {
            up: 'down',
            down: 'up',
            right: 'left',
            left: 'right'
        },
        minWidth: 540,
        initialContent: undefined,
        templates: {
            main: "<div class='expand-content'></div><div class='expander-summary'></div><div class='expander-button'></div><div class='clear'></div>",
            summary: "<div class='left-summary'></div><div class='right-summary'></div><div class='clear'></div>"
        }
    };

    var setupSlide = function (type, height, width) {
        var attrs = {};
        if (height) {
            ['height', 'marginTop', 'marginBottom', 'paddingTop', 'paddingBottom'].forEach(function (prop) {
                attrs[prop] = type;
            });
        }
        if (width) {
            ['width', 'marginLeft', 'marginRight', 'paddingLeft', 'paddingRight'].forEach(function (prop) {
                attrs[prop] = type;
            });
        }
        return attrs;
    }

    var slide = function ($element, direction, complete) {
        var props;
        if (direction == 'left') {
            props = setupSlide("hide", false, true);
        } else if (direction == 'right') {
            props = setupSlide("show", false, true);
        } else if (direction == 'up') {
            props = setupSlide("hide", true, false);
        } else if (direction == 'down') {
            props = setupSlide("show", true, false);
        }
        $element.animate(props, 500, 'easeOutExpo', complete);
    }

    var isMinWidth = function()
    {
        return ($(window).width() <= defaults.minWidth);
    }    

    var methods = {
        init: function($container, settings) {
            if (!$container.data("expanderInit")) {
                $container.data("expanderSettings", settings);
                $container.data("expanderInit", true);
                $container.on('click', '.expander-button,.expander-summary', function() { methods.toggleExpand.call($container[0]); });
            }
            $(window).on("resize", function() {
                if (isMinWidth()) {
                    methods.collapse.call($container);
                }
            });
            $container.addClass('expander-root');
            var content = settings.initialContent;
            if (!content) {
                content = $container.html();
            }
            if (!content || content.match(/^[ \n]*$/) !== null) {
                $container.hide();
            } else {
                $container.show();
            }
            $container.html(settings.templates.main);
            $container.find('.expand-content').html(content);
            var $summary = $container.find('.expander-summary');
            $summary.hide();
            $summary.html(settings.templates.summary);
            var leftSummary = '';
            var rightSummary = '';
            $container.find('[data-expander-summary]:not([data-expander-summary-location]),[data-expander-summary-location="left"]').each(function () {
                var summary = $(this).data("expander-summary");
                leftSummary += "<div style='padding-top:5px;cursor:pointer;'>" + summary + "</div>";
            });
            $container.find('[data-expander-summary-location="right"]').each(function () {
                var summary = $(this).data("expander-summary");
                rightSummary += "<span>" + summary + "</span>";
            });
            $summary.find('.left-summary').html(leftSummary);
            $summary.find('.right-summary').html(rightSummary);
            $container.find('.expander-button').addClass(settings.direction);
            methods.expand.call($container[0]);
        },
        toggleExpand: function () {
            var $content = $(this).find('.expand-content');
            if ($content.is(".expanded")) {
                methods.collapse.call(this);
            } else {
                methods.expand.call(this);
            }
        },
        expand: function () {
            var $container = $(this);
            var settings = $container.data("expanderSettings");
            var $content = $(this).find('.expand-content');
            if (!$content.is(".expanded")) {
                $container.find('.expander-summary').hide(200);
                slide($content, settings.direction);
                $content.addClass('expanded');
            }
            if (isMinWidth()) {
                $("#assessmentNavigationExpander").addClass("overflow");
                $("#assessmentNavigationExpander").width("100%");
            }
        },
        collapse: function (ifMin) {
            if (ifMin && !isMinWidth()) {
                return;
            }
            var $container = $(this);
            var settings = $container.data("expanderSettings");
            var $content = $(this).find('.expand-content');
            if ($content.is(".expanded")) {
                if (settings.direction === 'left' || settings.direction === 'right') {
                    $container.find('.expander-summary').show();
                } else {
                    $container.find('.expander-summary').show(200);
                }
                slide($content, settings.reverseDirectionMap[settings.direction]);
                $content.removeClass('expanded');
            }
            if (isMinWidth()) {
                $("#assessmentNavigationExpander").removeClass("overflow");
                $("#assessmentNavigationExpander").width("auto");
            }
        }
    };
    $.fn.expander = function (options) {
        var settings = $.extend({}, defaults);
        var method = undefined;
        if (typeof options === "object") {
            $.extend(settings, options);
        }
        else if (typeof options === "string") {
            method = options;
        }
        var args = Array.prototype.slice.call(arguments, 1);
        return this.each(function() {
            var $self = $(this);
            if (method && methods[method]) {
                args.splice(0, 0, settings);
                return methods[method].apply(this, args);
            } else {
                return methods.init($self, settings);
            }
        });
    };

    $("[data-control='expander']").expander();
})(jQuery);