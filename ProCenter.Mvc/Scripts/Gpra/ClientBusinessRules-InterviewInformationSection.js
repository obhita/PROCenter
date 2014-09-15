$(document).ready(function () {
    var myRules = {
        _0000003_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('#0000003_Value');
            }
            var val = $('#0000003_Value').val();
            if (val === undefined || val !== "R00000_0") {
                $('input[name="0000004_Value"]').closest('.question-root').show();
                $('.left-menu_0003000').hide();
            } else {
                $('input[name="0000004_Value"]').removeAttr('checked').closest('.question-root').hide();
                $('.left-menu_0003000').show();
            }
        }
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
});