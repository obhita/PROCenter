(function ($) {
    var defaults = {
        type: "POST",
        url: '',
        forceValueName: false,
        traditional: true,
        validate: false,
        ignoreSaveSelectors: '[data-ajaxform-ignore],.multiselect-container,button.multiselect',
        ignorePostSelectors: '[data-ajaxform-post-ignore],.multiselect-container,button.multiselect'
    };

    function getValue(element) {
        if (element.type == "checkbox") {
            return $(element).is(":checked");
        }
        if ($(element).is("[multiple]")) {
            var items = [];
            $('option', $(element)).each(function () {
                if (!!$(this).prop('selected')) {
                    items.push(this.value);
                }
            });
            return items;
        }
        return element.value;
    }

    function clearInputValue(container) {
        container.find(":input").each(function () {
            var $input = this;
            if ($input.type !== "hidden") {
                if ($input.type == "radio") {
                    $(this).removeAttr('checked').trigger('change');
                } else {
                    $(this).val(null).trigger('change');
                }
            }
        });
    }

    function shouldSave(element, options) {
        if ($(element).is(':not('+options.ignoreSaveSelectors +')') && $(element).closest(options.ignoreSaveSelectors).length === 0) {
            return true;
        }
        return false;
    }

    function shouldPost(element, options) {
        if ($(element).is(':not(' + options.ignorePostSelectors + ')') && $(element).closest(options.ignorePostSelectors).length === 0) {
            return true;
        }
        return false;
    }

    var methods = {
        save: function (element, settings) {
            if (methods.checkChanged(element)) {
                var $input = $(element);
                var $statusContainer = $input.parent();
                var checkStop = 0;
                while ($statusContainer.children(".ajax-loading-indicator").length === 0 && checkStop < 5) {
                    $statusContainer = $statusContainer.parent();
                    checkStop++;
                }
                var $container = $input.parents('[data-ajax-container]');
                if ($container.length === 0) {
                    $container = $input;
                }
                else if ($container.data("ajax-container-status") === true) {
                    $statusContainer = $statusContainer.add($container);
                }

                var params = {};
                var lastType = "";
                var valid = true;

                if ($container.is(":input")) {
                    if (!settings.forceValueName || $container[0].name !== $input.attr('name')) {
                        if (lastType != "checkbox" || $container[0].type != "hidden" || !params.hasOwnProperty(this.name)) {
                            params[$container[0].name] = getValue(this);
                        }
                    }
                    lastType = $container[0].type;
                    if (settings.validate) {
                        valid = valid & $container.valid();
                    }
                } else {
                    $container.find(":input").each(function () {
                        if(shouldPost(this, settings)) {
                            if (!settings.forceValueName || this.name !== $input.attr('name')) {
                                if (lastType != "checkbox" || this.type != "hidden" || !params.hasOwnProperty(this.name)) {
                                    params[this.name] = getValue(this);
                                }
                            }
                            lastType = this.type;
                            if (settings.validate) {
                                valid = valid & $(this).valid();
                            }
                        }
                    });
                }
                if (settings.forceValueName) {
                    var value = getValue($input[0]);
                    if (Array.isArray(value)) {
                        params.values = value;
                    } else {
                        params.value = value;
                    }
                }

                if (!$input.is('a.btn.nonResponse')) {
                    $container.find('a.btn.nonResponse').each(function () {
                        $(this).removeClass('active');
                    });
                    if ($input.is('[data-is-lookup]')) {
                        valid = valid & $input.valid();
                        params.IsLookup = 'true';
                    }
                } else {
                    clearInputValue($container);
                    $container.find('a.btn.nonResponse').each(function () {
                        $(this).removeClass('active');
                    });
                    $(element).addClass('active');
                    params.NonResponseType = element.innerText;
                }

                var url = settings.url;
                settings.data = params;
                if (valid) {
                    $statusContainer.attr("data-ajax-status", "loading");
                    $.ajax(url, settings)
                        .done(function (data) {
                            if (!data.error) {
                                $statusContainer.attr("data-ajax-status", "success");
                                var errorProperty = $(element).data("ajaxForm-error");
                                if (!errorProperty) {
                                    // try this way with the id
                                    errorProperty = $(element).attr('id');
                                }
                                if (errorProperty) {
                                    $('#' + errorProperty).addClass('valid').removeClass('input-validation-error');
                                    $('[data-valmsg-for="' + errorProperty + '"]').html("").addClass('field-validation-valid').removeClass('field-validation-error');
                                    $(element).removeData("ajaxForm-error");
                                }
                            } else if (data.errors) {
                                $statusContainer.attr("data-ajax-status", "fail");
                                for (var i = 0; i < data.errors.length; i++) {
                                    var error = data.errors[i];
                                    var errorSpan = "<span for='" + error.Properties[0] + "'>" + error.Message + "</span>";
                                    $('#' + error.Properties[0]).removeClass('valid').addClass('input-validation-error');
                                    $('[data-valmsg-for="' + error.Properties[0] + '"]').html(errorSpan).removeClass('field-validation-valid').addClass('field-validation-error');
                                    $(element).data("ajaxForm-error", error.Properties[0]);
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
            $element.data("initialValue", getValue(element));
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

            var settings = $.extend({},defaults);
            if (options) {
                $.extend(settings, options);
            }
            $self.on('focusin', ':input:not(:radio,select,:checkbox,:button)', function () {
                if (shouldSave(this, settings)) {
                    methods.setInitialValue(this);
                }
            });
            $self.on('focusout', ':input:not(:radio,select,:checkbox,:button)', function () {
                if (shouldSave(this, settings)) {
                    methods.save(this, settings);
                }
            });
            $self.on('click', ':radio,:checkbox', function () {
                if (shouldSave(this, settings)) {
                    methods.save(this, settings);
                }
            });
            $self.on('change', 'select', function () {
                if (shouldSave(this, settings)) {
                    var $select = $(this);
                    var timeoutId = $select.data('ajax-change-timeout');
                    if (timeoutId) {
                        clearTimeout(timeoutId);
                    }
                    $select.data('ajax-change-timeout', setTimeout(function() {
                        $select.removeData('ajax-change-timeout');
                        methods.save($select[0], settings);
                    }, 700));
                }
            });
            $self.on('click', 'a.btn.nonResponse', function () {
                if (shouldSave(this, settings) && !$(this).hasClass('active')) {
                    methods.save(this, settings);
                }
            });
            $self.on('input', ':input.text-box', function () {
                if (shouldSave(this, settings)) {
                    var $input = $(this);
                    $input.typing({
                        start: function() { console.log('starting'); },
                        stop: function() {
                            methods.save($input, settings);
                            $input.trigger('focusin');
                        },
                        delay: 200
                    });
                }
            });
        });
    };
})(jQuery);