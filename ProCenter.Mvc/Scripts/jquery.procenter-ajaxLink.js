(function ($) {
    $.fn.ajaxLink = function (options) {
        var defaults = {
            type: "POST",
            url: '',
            success: function () { },
            fail: function () {},
            getParams: function(){
                return {};
            },
            getData: function () {
                return {};
            },
            traditional : true
        };
        return this.each(function () {
            var settings = defaults;
            if (options) {
                $.extend(settings, options);
            }
            var $self = $(this);
            $self.click(function(e) {
                var $link = $(this);

                e.stopPropagation();
                e.preventDefault();
                var url = $link.attr("href");
                if (url === undefined || url === "") {
                    url = $link.data("href");
                }
                if (url === undefined || url === "") {
                    url = settings.url;
                }
                if (url === undefined || url === "") {
                    throw "href is required.";
                }

                var loadElement = $('#' + $link.data("loadelementid"));

                var paramsObject = settings.getParams.call($link[0]);
                var data = settings.getData.call($link[0]);
                if (paramsObject !== false && data !== false) {
                    loadElement.fadeIn();
                    var params = $.param(paramsObject);
                    settings.data = data;

                    if (params !== "") {
                        if (url.indexOf("?") !== -1) {
                            url += "&" + params;
                        } else {
                            url += "?" + params;
                        }
                    }

                    $.ajax(url, settings).always(function () { loadElement.fadeOut(); });
                }
            });
        });
    };
})(jQuery);