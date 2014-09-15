$(document).ready(function () {
    $(".btn-group ul.multiselect-container input[value='M10001_7']").on("change", function() {
        if ($(this).is(':checked')) {
            $('[name="7125038_Value"]').val("").closest('.question-root').show();
        } else {
            $('[name="7125038_Value"]').val("").closest('.question-root').hide();
        }
    });
    var myRules = {
        _7125031_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector("[name='7125031_Value.Code']");
            }
            var val = [];
            $("#7125031_Value option:selected").each(function () {
                val.push($(this).val());
            });
            if (val.indexOf("M10001_7") === -1) {
                $('[name="7125038_Value"]').val("").closest('.question-root').hide();
            } else {
                $('[name="7125038_Value"]').closest('.question-root').show();
            }
        },
        _7125022_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector("[name='7125022_Value.Code']");
            }

            var val = $("#7125022_Value option:selected").val();
            if (val === undefined || val === "" || val != "F10001_4") {
                $('[name="7125041_Value"]').val("").closest('.question-root').hide();
            } else {
                $('[name="7125041_Value"]').closest('.question-root').show();
            }
        },
        _7125009_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector("input[name='7125009_Value']");
            }

            var val = $('input[name="7125009_Value"]').val();
            if (val === undefined || val === "" || val == 0) {
                $('[name="7125010_Value"]').val("").closest('.question-root').hide();
            } else {
                $('[name="7125010_Value"]').closest('.question-root').show();
            }
        },
    };

    $("[name=7125009_Value]").attr("min", 0);
    $("[name=7125009_Value]").attr("max", 7);
    $("[name=7125009_Value]").attr("maxlength", 1);

    $("[name=7125010_Value]").attr("min", 0);
    $("[name=7125010_Value]").attr("max", 5000);
    $("[name=7125010_Value]").attr("maxlength", 4);

    $("[name=7125011_Value]").attr("min", 0);
    $("[name=7125011_Value]").attr("max", 10);
    $("[name=7125011_Value]").attr("maxlength", 2);

    $("[name=7125023_Value]").attr("min", 1900);
    $("[name=7125023_Value]").attr("max", (new Date).getFullYear());
    $("[name=7125023_Value]").attr("maxlength", 4);

    $("[name='7125009_Value'],[name='7125010_Value'],[name='7125011_Value'],[name='7125023_Value']").on('keypress', function (e) {
        var charCode = e.which || e.keyCode;
        if (charCode > 31 && (charCode <= 47 || charCode > 57))
            return false;
        if (!checkMaxLength(this)) return false;
        if (e.shiftKey) return false;
        return true;
    });

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
});

function checkMaxLength(input) {
    var text = "";
    var selectedLength = 0;
    if (window.getSelection) {
        text = window.getSelection().toString();
    } else if (document.selection && document.selection.type != "Control") {
        text = document.selection.createRange().text;
    }
    selectedLength = text.length;
    var maxLength = $(input).attr("maxlength");
    if (maxLength != null) {
        if (input.value.length - selectedLength >= maxLength) {
            return false;
        }
    }
    return true;
}
