$(document).ready(function() {
    var myRules = {
        _6125030_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="6125030_Value"]');
            }
            var val = $('input[name="6125030_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('[name="6125031_Value"]').val("").closest('.question-root').hide();
            } else {
                $('[name="6125031_Value"]').closest('.question-root').show();
            }
        },
        _6125032_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('#6125032_Value');
            }
            var val = $("#6125032_Value option:selected").text().trim();
            if (val === undefined || val === "" || val != "Other") {
                $('[name="6125033_Value"]').val("").closest('.question-root').hide();
            } else {
                $('[name="6125033_Value"]').closest('.question-root').show();
            }
        },
    };

    $("[name=6125034_Value]").attr("min", 2);
    $("[name=6125034_Value]").attr("max", 100);
    $("[name=6125034_Value]").attr("maxlength", 3);
    $("[name=6125035_Value]").attr("min", 1);
    $("[name=6125035_Value]").attr("maxlength", 3);

    $("[name=6125035_Value],[name=6125034_Value]").on('keypress', function (e) {
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