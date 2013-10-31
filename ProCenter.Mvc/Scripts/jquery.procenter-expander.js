(function ($) {
    var fixSizes = window.fixSizes || function () { };
    var defaults = {
        initialContent: undefined,
        templates: {
            main: "<div class='expand-content'></div><div class='expander-summary'></div><div class='expander-button'></div><div class='clear'></div>",
            summary: "<div class='left-summary'></div><div class='right-summary'></div><div class='clear'></div>"
    }
    };

    var methods = {
        init: function ($container, settings) {
            if (!$container.data("expanderInit")) {
                $container.data("expanderInit", true);
                $container.on('click', '.expander-button,.expander-summary', function () { methods.toggleExpand.call($container[0]); });
            }
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
                leftSummary += "<span>" + summary + "</span>";
            });
            $container.find('[data-expander-summary-location="right"]').each(function () {
                var summary = $(this).data("expander-summary");
                rightSummary += "<span>" + summary + "</span>";
            });
            $summary.find('.left-summary').html(leftSummary);
            $summary.find('.right-summary').html(rightSummary);
            methods.expand.call($container[0]);
        },
        toggleExpand: function () {
            var $container = $(this);
            var $content = $(this).find('.expand-content');
            $container.find('.expander-summary').slideToggle();
            $content.slideToggle(400, function () { fixSizes(); });
            $content.toggleClass('expanded');
        },
        expand: function () {
            var $container = $(this);
            var $content = $(this).find('.expand-content');
            $container.find('.expander-summary').slideUp();
            $content.slideDown(400, function () { fixSizes(); });
            $content.addClass('expanded');
        },
        collapse: function () {
            var $container = $(this);
            var $content = $(this).find('.expand-content');
            $container.find('.expander-summary').slideDown();
            $content.slideUp(400, function () { fixSizes(); });
            $content.removeClass('expanded');
        }
    };
    $.fn.expander = function (options) {
        var settings = defaults;
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
})(jQuery);