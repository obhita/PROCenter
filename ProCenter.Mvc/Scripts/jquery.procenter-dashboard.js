(function ($) {
    var fixSizes = window.fixSizes || function () { };
    var defaults = {
        collapsed: function() {
        },
        templates: {
            closeButton: "<button type='button' style='display: none' class='dashboard-close btn fs1' data-icon='&#xe083;' title='Collapse widget'></button>"
        }
    };

    var methods = {
        init: function ($container, settings) {
            if ($container) {
                $container.find('.widget-header').prop('title', 'Double click to expend');
            }
            if (!$container.data("dashboard-init")) {
                $container.addClass("dashboard");
                $container.data("dashboard-init", true);
                $container.on('dblclick', '.widget-header,.widget-header *', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    methods.toggleExpand.call($(this).parent('.widget'));
                    if (document.selection && document.selection.empty) {
                        document.selection.empty();
                    } else if (window.getSelection) {
                        var sel = window.getSelection();
                        sel.removeAllRanges();
                    }
                });
                var $closeButton = $(settings.templates.closeButton);
                $closeButton.click(function () { methods.collapse.call($container.find('.expanded')[0]); });
                $container.append($closeButton);
            }
        },
        toggleExpand: function () {
            var $widget = $(this);
            if (!$widget || $widget.length == 0) {
                return;
            }
            if ($widget.hasClass('expanded')) {
                methods.collapse.call(this);
                $widget.find('.widget-header').prop('title', 'Double click to expend');
            } else {
                methods.expand.call(this);
                $widget.find('.widget-header').prop('title', 'Double click to collapse');
            }
        },
        expand: function (settings, collapseCallback) {
            var $widget = $(this);
            if (collapseCallback) {
                $widget.data("CollapseCallback",collapseCallback);
            }
            $widget.addClass('expanded');
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
                function() {
                    $widget.height('auto');
                    $widget.width('auto');
                    $dashboard.find('.dashboard-close').fadeIn();
                    $widget.trigger('expanded');
                    fixSizes();
                }
            );
        },
        collapse: function () {
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
                    $widget.removeClass('expanded');
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
                var methodArgs = args.slice(0);
                var widget = methodArgs[0];
                methodArgs[0] = settings;
                return methods[method].apply(widget, args);
            } else {
                return methods.init($self, settings);
            }
        });
    };
})(jQuery);