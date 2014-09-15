(function ($) {
    var RenderHeight = function (element, options) {
        var $element = $(element);
        var initialValue = $element.val();
        var label1 = 'ft';
        if (options != undefined) {
            label1 = options[0];
        }
        var label2 = 'in';
        if (options != undefined) {
            label2 = options[1];
        }
        var defaults = {
            templates: {
                wrapper: '<div class="height-container"></div>',
                heightFeet: '<input type="text" pattern="[0-9]*" data-ajaxform-ignore class="number heightFeet" aria-label="" /><span class="heightFeet">' + label1 + '</span>',
                heightInches: '<input type="text" pattern="[0-9]*" data-ajaxform-ignore class="number heightInches" aria-label="" /><span class="heightInches">' + label2 + '</span>'
            }
        };

        var settings = $.extend({}, defaults, options);
        var $heightTotalInches = $element.after(settings.templates.wrapper).next();
        var $heightFeet = $(settings.templates.heightFeet).appendTo($heightTotalInches);
        var $heightInches = $(settings.templates.heightInches).appendTo($heightTotalInches);
        var $heightFeetInches = $heightFeet.add($heightInches);
        if ($element.attr("disabled")) {
            $heightFeet.attr("disabled", true);
            $heightInches.attr("disabled", true);
        }
        setAriaLabels();

        $heightFeetInches.on('focusout, change', function () {
            var totalInches = Number($heightFeet.val()) * 12 + Number($heightInches.val());
            if (totalInches <= 0) {
                totalInches = null;
                $heightFeet.val(null);
                $heightInches.val(null);
            }
            if ($heightFeet.val() == null || $heightFeet.val() == 0) {
                totalInches = null;
            }
            if ($heightFeet.val() != null && $heightFeet.val() > 0 && ($heightInches.val() == null || $heightInches.val() == "")) {
                $heightInches.val(0);
            }
            $element.val(totalInches);
            $element.trigger('focusout');
        });

        function setAriaLabels() {
            var label = $(".heightTotalInches").attr("aria-label");
            $heightFeet.attr("aria-label", label + " " + label1);
            $heightInches.attr("aria-label", label + " " + label2);
        }

        function setInitialValues() {
            if (initialValue) {
                $heightTotalInches.val(initialValue);
                var feet = Math.floor(initialValue / 12);
                var inches = initialValue % 12;
                if (feet <= 0) {
                    feet = null;
                }
                if (feet <= 0 && inches <= 0) {
                    inches = null;
                }
                if (feet > 0 && inches <= 0) {
                    inches = 0;
                }
                $heightFeet.val(feet);
                $heightInches.val(inches);
            } 
        };

        setInitialValues();
    };
     
    $.fn.renderHeight = function (options) {
        return this.each(function () {
            RenderHeight(this, options);
        });
    };
}(jQuery));