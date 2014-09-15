$(document).ready(function () {
    var myRules = {
        _0000211_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('#0000211_Value');
            }
            var val = $('#0000211_Value').val();
            if (val === undefined || val !== "S00000_8") {
                $('input[name="0000212_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000212_Value"]').closest('.question-root').show();
            }
        }
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
});