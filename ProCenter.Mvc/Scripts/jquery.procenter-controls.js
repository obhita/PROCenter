(function ($) {

    $.fn.placeholder = function () {
        if (typeof document.createElement("input").placeholder == 'undefined') {
            $('[placeholder]').each(function () {
                var input = $(this);
                var placeholder = input.attr('placeholder');
                input.wrap("<div data-placeholder-wrapper='" + placeholder + "'/>");
                if (input.val()) {
                    input.parent().attr("data-placeholder-hide", true);
                }
                input.focus(function() {
                    input.parent().attr("data-placeholder-hide", true);
                }).blur(function() {
                    if (input.val() == '') {
                        input.parent().removeAttr("data-placeholder-hide");
                    }
                });
            });
        }
    };

    $.fn.valInt = function() {
        return parseInt(this.val(), 10);
    };

    $.fn.scaleControl = function (options) {
        var settings =
		{
		    disabled: false,
		    vertical: false,
		    showLabels: false
		};

        var inClick = false;

        var methods = {
            click: function (e) {
                var element = $(e.target).parent();

                if (settings.vertical) {
                    element.closest(".ui-scale-option").nextAll().andSelf()
                        .addClass("ui-scale-active");

                    element.closest(".ui-scale-option").prevAll()
                        .removeClass("ui-scale-active");
                }
                else {
                    element.closest(".ui-scale-option").prevAll().andSelf()
                        .addClass("ui-scale-active");

                    element.closest(".ui-scale-option").nextAll()
                        .removeClass("ui-scale-active");
                }

                var value = element.attr("data-value");

                if (!e.data.hasChanged) {
                    inClick = true;
                    $(e.data.selectBox).val(value).trigger("change");
                    inClick = false;
                }
            },
            change: function (e) {
                var value = $(this).val();
                methods.setValue(value, e.data.container, e.data.selectBox);
            },
            setValue: function (value, container, selectBox) {
                var event = { "target": null, "data": {} };

                container.children().each(function () {
                    if ($(this).attr('data-value') == value) {
                        event.target = this.firstChild;
                        return false;
                    }
                    return true;
                });
                if (!inClick) {
                event.data.selectBox = selectBox;
                event.data.hasChanged = true;
                methods.click(event);
                }
            },
            mouseOverDiv: function (e) {
                methods.mouseOver(e, $(e.target));
            },
            mouseOverSpan: function (e) {
                methods.mouseOver(e, $(e.target).parent());
            },
            leaveDiv: function (e) {
                methods.mouseLeave(e, $(e.target));
            },
            leaveSpan: function (e) {
                methods.mouseLeave(e, $(e.target).parent());
            },
            mouseOver: function (e, target) {
                if (settings.vertical) {
                    target.nextAll().andSelf().addClass("ui-scale-hover");
                    target.nextAll().last().addClass("ui-scale-hover-first");
                    if (target.nextAll().length == 0) {
                        target.addClass("ui-scale-hover-first");
                    }
                    target.addClass("ui-scale-hover-last");
                }
                else {
                    target.prevAll().andSelf().addClass("ui-scale-hover");
                    target.prevAll().last().addClass("ui-scale-hover-first");
                    if (target.prevAll().length == 0) {
                        target.addClass("ui-scale-hover-first");
                    }
                    target.addClass("ui-scale-hover-last");
                }
            },
            mouseLeave: function (e, target) {
                if (settings.vertical) {
                    target.nextAll()
                        .andSelf()
                        .removeClass("ui-scale-hover ui-scale-hover-first ui-scale-hover-last");
                }
                else {
                    target.prevAll()
                        .andSelf()
                        .removeClass("ui-scale-hover ui-scale-hover-first ui-scale-hover-last");
                }
            },

        };

        return this.each(function () {
            var self = $(this);

            if ('select-one' !== this.type ||
                self.prop('hasProcessed')) {
                return;
            }

            if (options) {
                $.extend(settings, options);
            }

            self.hide();
            self.prop('hasProcessed', true);

            var div = $("<div/>").prop({
                title: this.title,
                className: "ui-scale"
            }).insertAfter(self);
            var classes = self.attr('class').split(" ");
            for (var i = 0, l = classes.length; i < l; ++i) {
                if (classes[i].indexOf("scaleControl") != 0) {
                    div.addClass(classes[i]);
                }
            }
            if (settings.vertical) {
                div.css("display", "inline-block");
            }

            var optionCount = $('option', self).length;
            var optionWidth = (100 / optionCount) + "%";
            var count = 0;
            var optionDivs = "";
            $('option', self).each(function () {
                if (this.value != "") {
                    var valueLink = "<div data-value='" + this.value + "' title='" + $(this).text() + "' class='ui-scale-option";
                    if (settings.vertical) {
                        valueLink += " vertical' style='display:block'>";
                    } else {
                        valueLink += ("' style='float:left;width:" + optionWidth + "'>");
                    }
                    valueLink += ("<div tabindex='0'>" + count++ + "</div></div>");
                    if (settings.vertical) {
                        optionDivs = valueLink + optionDivs;
                    } else {
                        optionDivs += valueLink;
                    }
                }
                    });
            div.html(optionDivs);
            if (true !== settings.disabled && self.prop("disabled") !== true) {
                div.on('mouseover', '.ui-scale-option div', methods.mouseOverSpan);
                div.on('mouseout', '.ui-scale-option div', methods.leaveSpan);
                div.on('click', '.ui-scale-option div', { "selectBox": self }, methods.click);
                div.on('keypress', '.ui-scale-option div', function(event) {
                        if (event.keyCode == 32) {
                            valueDiv.trigger("click");
                            return false;
                        }
                    });
                    }

            if (settings.showLabels) {
                var labeldiv = $("<div/>").prop({
                    title: this.title,
                    className: "ui-scale-labels"
                });
                if (settings.vertical) {
                    labeldiv.css("float", "left");
                    labeldiv.css("min-width", "0");
                    labeldiv.insertAfter(self);
                }
                else {
                    labeldiv.css("clear", "both");
                    labeldiv.insertAfter(div);
                }

                var labels = "";
                $('.ui-scale-option', div).each(function () {
                    var label = "<span class='ui-scale-option-label";
                    if (settings.vertical) {
                        label += " vertical' style='display: block'>";
                    } else {
                        label += ("' style='float:left;width:" + optionWidth + "'>");
                    }
                    label += ($(this).prop("title") + "</span>");
                    if (settings.vertical) {
                        labels = label + labels;
                    } else {
                        labels += label;
                    }
                });
                labeldiv.html(labels);
            }

            if (0 != $('option:selected', self).size()) {
                if (self.val() != "") {
                    methods.setValue(self.val(), div, self);
                }
            }

            if (true !== settings.disabled && self.prop("disabled") !== true) {
                $(div).bind("mouseover", methods.mouseOverDiv)
                    .bind("mouseout", methods.leaveDiv);
            }

            self.bind("change", { "selectBox": self, "container": div }, methods.change);
        });

    };

    $.fn.shareHeightContext = function () {
        return this.each(function () {
            var self = $(this);
            if (self == undefined) return;
            var sharedItems = $("div[data-role='sharedheightitem']", self);

            var height = Math.max.apply(null, sharedItems.map(function () {
                return $(this).height();
            }).get());

            sharedItems.each(function () {
                $(this).height(height);
            });
        });
    };

    $.fn.focusContainer = function () {
        return this.each(function () {
            var self = $(this);
            self.on('focusin','*',function () {
                self.addClass("ui-state-focus");
            }).on('focusout','*',function () {
                self.removeClass("ui-state-focus");
            });
        });
    };

    $.getTotalHeaderHeight = function (accordion) {
        var headers = $(accordion).find('h3');
        return $(headers[0]).height() * headers.length;
    };

    $.fixSizes = function () {
    };

    $.setformDirty = function () {
        if ($.rules.init && !window.assessmentFormDirty) {
        window.assessmentFormDirty = true;
            var form = $('#assessmentForm');
            form.find(".save").removeClass("hidden");
            form.find(".cancel").removeClass("hidden");

            $("#submitButton").hide();
        }
    };

    $.setformClean = function () {
        if (window.assessmentFormDirty) {
            window.assessmentFormDirty = false;
            var form = $('#assessmentForm');
            form.find(".save").addClass("hidden");
            form.find(".cancel").addClass("hidden");
        }
    };

    function initializeForm() {
        var form = $('.assessment-wrapper');

        $.rules.Initialize(form);

        $("#assessmentFooter").on('click', ':submit', function () {
            window.assessmentFormDirty = false;
            $("#nextRoute").val($(this).data("route-info"));
        });

        $("#assessmentFooter").on('click', 'a:contains("Cancel")', function () {
            window.assessmentFormDirty = false;
            $("#nextRoute").val($(this).data("route-info"));
        });

        form.on('change', ':input', function () {
            $.setformDirty();
        });

        form.find(".PsychologicalSection .InterviewerRating .fieldsetgroup").shareHeightContext();
        
        form.find('div.multiselect-wrapper.checkall').handleCheckAll();
        
        form.find('.UInt32>input[type="number"]').prop('min', '0').addClass("positive-integer");
        form.find('.primitive>input[type="number"], .primitive>input[type="text"]').prop('min', '0').addClass("positive-integer");
        form.find('.MoneyDto>input[id$="_Amount"]').formatCurrency().toNumber().addClass("positive").blur(function () {
            $(this).formatCurrency().toNumber();
        });
        form.find(".positive").numeric({ negative: false });
        form.find(".positive-integer").numeric({ decimal: false, negative: false });

        form.on('keydown', function (e) {
            if (e.which == 13 && e.target.nodeName != 'TEXTAREA') {
                e.preventDefault();
            }
        });

        form.find('.scaleControl-vertical').scaleControl({ vertical: true });
        form.find('.scaleControl-vertical-labels').scaleControl({ vertical: true, showLabels: true });
        form.find('.scaleControl-labels').scaleControl({ showLabels: true });
        form.find('.scaleControl').scaleControl();
    }

    $.getFormUrl = function() {
        var section = $('#sectionWrapper').attr("class");
        var subSection = $('#subSectionWrapper').attr("class");
        if (subSection) {
            subSection = "/" + subSection;
        } else {
            subSection = "";
        }
        return window.procenter.urlformat.replace("{section}", section).replace("/{subsection}", subSection);
    };

    $.initializing = function (active, urlformat) {
        window.procenter = { urlformat: urlformat };
        window.assessmentFormDirty = false;
        var navigation = $('#assessmentNavigation');

        navigation.accordion({
            active: active,
            heightStyle: "content",
            activate: function(event, ui) {
                ui.newPanel.height("auto");
                if (ui.newPanel.height() + $.getTotalHeaderHeight(this) > $('#rootTabs #tabContent').height()) {
                    $(this).accordion("option", "heightStyle", "fill");
                } else {
                    $(this).accordion("option", "heightStyle", "content");
                }
                $(this).accordion("refresh");
            }
        });
        
        $.fixSizes();
        $(window).resize(function () {
            $.fixSizes();
        });

        $('.question-root').focusContainer();

        navigation.on('click','.navigation-button',function (e) {
            window.assessmentFormDirty = false;
            $("#nextRoute").val($(this).data("route-info"));
            var form = $("#assessmentForm");
            var isValid = form.valid();
            if (isValid) {
                form.submit();
            }
        });

        $(window).bind('beforeunload', function () {
            if (window.assessmentFormDirty) {
                return "You will lose all pending changes...";
            }
            return undefined;
        });

        initializeForm();
        $.navigateErrors();
    };

    $.navigateErrors = function () {
        var init = true;
        var form = $('#assessmentForm');

        function updateCount(errors) {
            if (errors) {
                var message = errors == 1 ? "You missed 1 field. It has been highlighted." : "You missed " + errors + " fields. They have been highlighted.";
                $("#summary").text(message);
                if ($("#errorSummary").hasClass("hidden")) {
                    $("#errorSummary").removeClass("hidden");
                    $("#submitPanel").addClass("hidden");
                    $.fixSizes();
                }
            } else if (!$("#errorSummary").hasClass("hidden")) {
                $("#errorSummary").addClass("hidden");
                $.fixSizes();
            }
        }

        var validator = form.validate("unobtrusiveValidation");

        validator.settings.ignore = "";

        validator.settings.showErrors = function (element, errors) {
            if (init) {
                init = false;
                var error = this.errorList[0];
                if ( error && error.element) {
                    if ($(error.element).is(':focusable')) {
                        error.element.focus();
                    } else {
                        $(error.element.parentElement).find(':focusable').first().focus();
                    }
                }
            }

            updateCount(this.numberOfInvalids());
            this.defaultShowErrors();
        };
        
        form.validateDelegate('[type="checkbox"], [type="radio"]', "focusin focusout keyup",
            function (event) {
                var validator = $.data(this[0].form, "validator"),
                    eventType = "on" + event.type.replace(/^validate/, "");
                if (validator.settings[eventType]) {
                    validator.settings[eventType].call(validator, this[0], event);
                }
            });
        
        form.find('select.scaleControl,select.scaleControl-vertical,select.scaleControl-labels,.scaleControl-vertical-labels').each(function () {
            $(this).parent().on("focusin focusout keyup click", '.ui-scale-option div',
                $.proxy(function (event) {
                     $(this).trigger(event.type);
                }, this));
        });
        form.find('div.yesno-radiobuttons :radio').each(function () {
            $(this).parent().on("focusin focusout",':focusable',
                $.proxy(function (event) {
                    $(this).trigger(event.type);
                }, this));
        });

        function getNextError(invalid, name) {
            var keys = Object.keys(invalid);
            var curIndex = keys.indexOf(name);
            if (curIndex == keys.length - 1) {
                curIndex = -1;
            }
            return keys[curIndex + 1];
        }

        function getPreviousError(invalid, name) {
            var keys = Object.keys(invalid);
            var curIndex = keys.indexOf(name);
            if (curIndex == 0) {
                curIndex = keys.length;
            }
            return keys[curIndex - 1];
        }

        function setErrorFocus(elementName) {
            var error = validator.findByName(elementName);
            if (error.is(':focusable')) {
                error.focus();
            } else {
                error.parent().find(':focusable').first().focus();
            }
        }

        $("#nextError").click(function () {
            var nextElementName = getNextError(validator.invalid, validator.lastActive.name);
            setErrorFocus(nextElementName);
        });

        $("#previousError").click(function () {
            var previousElementName = getPreviousError(validator.invalid, validator.lastActive.name);
            setErrorFocus(previousElementName);
        });
    };
    
    $.fn.clearable = function () {
        var input = $(this);
        if(!input[0].hasOwnProperty("onsearch"))//only for IE10 or greater
        {
            input.on('mouseup', function () {
                setTimeout(function () {
                    input.trigger('search');
                }, 5);
            });
        }
    };

})(jQuery);