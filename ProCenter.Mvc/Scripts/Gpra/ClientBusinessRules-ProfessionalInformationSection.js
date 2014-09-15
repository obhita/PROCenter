$(document).ready(function () {
    var myRules = {
        _0000148_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('#0000148_Value');
            }
            var val = $('#0000148_Value').val();
            if (val === undefined || val !== "J00000_3") {
                $('input[name="0000149_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000149_Value"]').closest('.question-root').show();
            }
        },
        _0000151_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('#0000151_Value');
            }
            var val = $('#0000151_Value').val();
            if (val === undefined || val !== "L00000_7") {
                $('input[name="0000152_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000152_Value"]').closest('.question-root').show();
            }
        },
        _0000159_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000159_Value"]');
            }
            var val = $('input[name="0000159_Value"]').val();
            if (val === undefined || val === "" || val < 1) {
                $('input[name="0000160_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000160_Value"]').closest('.question-root').show();
            }
        }
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
});