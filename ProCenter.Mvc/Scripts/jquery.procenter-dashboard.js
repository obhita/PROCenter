(function ($) {
    var defaults = {
        expandedClass: "expanded",
        collapsed: function() {
        },
        templates: {
            closeButton: "<button type='button' style='display: none' class='dashboard-close btn fs1' data-icon='&#xe083;' title='Collapse widget'></button>"
        }
    };
    var fixSizes = window.fixSizes || function () { };

    var methods = {
        init: function ($container, settings) {
            if ($container) {
                $container.find('.widget-header').prop('title', 'Double click to expand');
            }
            if (!$container.data("dashboard-init")) {
                $container.addClass("dashboard");
                $container.data("dashboard-init", true);
                $container.on('dblclick', '.widget-header,.widget-header *', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    methods.toggleExpand.apply($(this).parent('.widget')[0],[settings]);
                    if (document.selection && document.selection.empty) {
                        document.selection.empty();
                    } else if (window.getSelection) {
                        var sel = window.getSelection();
                        sel.removeAllRanges();
                    }
                });
                var $closeButton = $(settings.templates.closeButton);
                $closeButton.click(function () {
                    methods.collapse.apply($container.find('.widget.' + settings.expandedClass)[0], [settings]);
                });
                $container.append($closeButton);
            }
        },
        toggleExpand: function (settings) {
            var $widget = $(this);
            if (!$widget || $widget.length == 0) {
                return;
            }
            if ($widget.hasClass(settings.expandedClass)) {
                methods.collapse.apply(this, [settings]);
                $widget.find('.widget-header').prop('title', 'Double click to expand');
            } else {
                methods.expand.apply(this, [settings]);
                $widget.find('.widget-header').prop('title', 'Double click to collapse');
            }
        },
        expand: function (settings, collapseCallback) {
            var $widget = $(this);
            if (collapseCallback) {
                $widget.data("CollapseCallback",collapseCallback);
            }
            $widget.addClass(settings.expandedClass);
            var $dashboard = $widget.closest('.dashboard');
            $dashboard.find('.widget:not(.expanded)').hide();
            var height = $dashboard.height();
            var width = $dashboard.width();
            var position = {
                top: $widget.position().top,
                left: $widget.position().left,
                bottom: parseInt($widget.css("bottom")),
                right: parseInt($widget.css("right"))
            };
            $widget.data("dashboard-original-height",$widget.height());
            $widget.data("dashboard-original-width", $widget.width());

            $widget.animate(
                {
                    height: height - position.top - position.bottom,
                    width: width - position.left - position.right
                },
                200,
                "swing",
                function () {
                    $widget.height('auto');
                    $widget.width('auto');
                    $dashboard.find('.dashboard-close').fadeIn();
                    $widget.trigger('expanded');
                    fixSizes();
                    $(".editor-content").find(":input:not(:button):not(:hidden):enabled[data-val=true]:first").focus();
                }
            );
        },
        collapse: function (settings) {
            var $widget = $(this);
            var $dashboard = $widget.closest('.dashboard');
            var height = $widget.data("dashboard-original-height");
            var width = $widget.data("dashboard-original-width");

            $dashboard.find('.widget').fadeIn();
            $dashboard.find('.dashboard-close').hide();
            $widget.animate(
                {
                    height: height,
                    width: width
                },
                200,
                "swing",
                function () {
                    $widget.removeClass(settings.expandedClass);
                    $widget.height('');
                    $widget.width('');
                    var collapseCallBack = $widget.data("CollapseCallback");
                    if (collapseCallBack && Object.prototype.toString.call(collapseCallBack) === '[object Function]') {
                        collapseCallBack($widget[0]);
                    }
                    $widget.data("CollapseCallback", null);
                    $widget.trigger('collapsed');
                    fixSizes();
                }
            );
        }
    };

    $.fn.dashboard = function (options) {
        var settings = defaults;
        var method = undefined;
        if (typeof options === "object") {
            $.extend(settings, options);
        }
        else if (typeof options === "string") {
            method = options;
        }
        var args = Array.prototype.slice.call(arguments, 1);
        return this.each(function () {
            var $self = $(this);
            if (method && methods[method]) {
                var widget = args[0];
                var methodArgs = args.slice(1);
                methodArgs.unshift(settings);
                return methods[method].apply(widget, methodArgs);
            } else {
                return methods.init($self, settings);
            }
        });
    };
})(jQuery);