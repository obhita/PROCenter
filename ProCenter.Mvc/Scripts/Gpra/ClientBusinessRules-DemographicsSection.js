$(document).ready(function () {
    var myRules = {
        _0000067_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('#0000067_Value');
            }
            var val = $('#0000067_Value').val();
            if (val === undefined || val !== "A00000_3" ) {
                $('input[name="0000068_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000068_Value"]').closest('.question-root').show();
            }
        },
        _0000069_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000069_Value"]');
            }
            var val = $('input[name="0000069_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000070_Value"]').removeAttr('checked').val(null).closest('.question-root').hide();
                $('input[name="0000071_Value"]').removeAttr('checked').val(null).closest('.question-root').hide();
                $('input[name="0000072_Value"]').removeAttr('checked').val(null).closest('.question-root').hide();
                $('input[name="0000073_Value"]').removeAttr('checked').val(null).closest('.question-root').hide();
                $('input[name="0000074_Value"]').removeAttr('checked').val(null).closest('.question-root').hide();
                $('input[name="0000075_Value"]').removeAttr('checked').val(null).closest('.question-root').hide();
                $('input[name="0000076_Value"]').removeAttr('checked').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000070_Value"]').closest('.question-root').show();
                $('input[name="0000071_Value"]').closest('.question-root').show();
                $('input[name="0000072_Value"]').closest('.question-root').show();
                $('input[name="0000073_Value"]').closest('.question-root').show();
                $('input[name="0000074_Value"]').closest('.question-root').show();
                $('input[name="0000075_Value"]').closest('.question-root').show();
                $('input[name="0000076_Value"]').closest('.question-root').show();
            }
        },
        _0000076_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000076_Value"]');
            }
            var val = $('input[name="0000076_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000077_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000077_Value"]').closest('.question-root').show();
            }
        }
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
    
    $(function () {
        $('input[name="0000084_Value"]').datepicker({ minDate: '-0d' });
    });
});