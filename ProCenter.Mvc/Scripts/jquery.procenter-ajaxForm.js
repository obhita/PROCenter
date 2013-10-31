(function ($) {
    var defaults = {
        type: "POST",
        url: '',
        forceValueName: false,
        traditional: true,
        validate: false
    };

    function getValue(element) {
        if (element.type == "checkbox") {
            return $(element).is(":checked");
        }
        return element.value;
    }

    var methods = {
        save: function (element, options) {
            if (methods.checkChanged(element)) {
                var $input = $(element);
                var $statusContainer = $input.parent();
                var $container = $input.parents('[data-ajax-container]');
                if ($container.length === 0) {
                    $container = $input;
                }
                else if ($container.data("ajax-container-status") === true) {
                    $statusContainer = $statusContainer.add($container);
                }
                var settings = defaults;
                if (options) {
                    $.extend(settings, options);
                }

                var params = {};
                var lastType = "";
                var valid = true;
                $container.find(":input").each(function() {
                    if (!settings.forceValueName || this.name !== $input.attr('name')) {
                        if (lastType != "checkbox" || this.type != "hidden" || !params.hasOwnProperty(this.name) ) {
                            params[this.name] = getValue(this);
                        }
                    }
                    lastType = this.type;
                    if (settings.validate) {
                        valid = valid & $(this).valid();
                    }
                });
                if (settings.forceValueName) {
                    params.value = $input.val();
                }

                if ($input.attr('name').endsWith(".Code")) {
                    params.IsLookup = 'true';
                }

                var url = settings.url;
                settings.data = params;
                if (valid) {
                    $statusContainer.attr("data-ajax-status", "loading");
                    $.ajax(url, settings)
                        .done(function (data) {
                            if (!data.error) {
                                $statusContainer.attr("data-ajax-status", "success");
                            } else if (data.errors) {
                                $statusContainer.attr("data-ajax-status", "fail");
                                for (var i = 0; i < data.errors.length; i++) {
                                    var error = data.errors[i];
                                    var errorSpan = "<span for='" + error.Properties[0] + "'>" + error.Message + "</span>";
                                    $('#' + error.Properties[0]).removeClass('valid').addClass('input-validation-error');
                                    $('[data-valmsg-for="' + error.Properties[0] + '"]').html(errorSpan).removeClass('field-validation-valid').addClass('field-validation-error');
                                }
                            }
                        })
                        .fail(function (error) {
                            $statusContainer.attr("data-ajax-status", "fail");
                        })
                        .always(function () {
                            setTimeout(function () {
                                $statusContainer.removeAttr("data-ajax-status");
                            }, 8000);
                        });
                }
            }
        },
        setInitialValue : function(element) {
            var $element = $(element);
            $element.data("initialValue", $element.val());
        },
        checkChanged : function(element) {
            var $element = $(element);
            var initalValue = $element.data("initialValue");
            return initalValue === undefined || initalValue !== $element.val();
        }
    };
    String.prototype.endsWith = function (suffix) {
        return this.indexOf(suffix, this.length - suffix.length) !== -1;
    };
    $.fn.ajaxForm = function(options) {
        return this.each(function() {
            var $self = $(this);
            if (!options.url || options.url === '') {
                options.url = $self.data('ajax-url');
            }
            $self.on('focusin', ':input:not(:radio,select)', function () {
                if ($(this).closest('[data-ajaxform-ignore]').length === 0) {
                    methods.setInitialValue(this);
                }
            });
            $self.on('focusout', ':input:not(:radio,select)', function () {
                if ($(this).closest('[data-ajaxform-ignore]').length === 0) {
                    methods.save(this, options);
                }
            });
            $self.on('click', ':radio,:checkbox', function () {
                if ($(this).closest('[data-ajaxform-ignore]').length === 0) {
                    methods.save(this, options);
                }
            });
            $self.on('change', 'select', function () {
                if ($(this).closest('[data-ajaxform-ignore]').length === 0) {
                    methods.save(this, options);
                }
            });
        });
    };
})(jQuery);