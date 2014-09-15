$(document).ready(function () {
    var myRules = {
        _0000215_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('#0000215_Value');
            }
            var val = $('#0000215_Value').val();
            if (val === undefined || val !== "T00000_1") {
                $('#0000216_Value').val(null).closest('.question-root').hide();
            } else {
                $('#0000216_Value').closest('.question-root').show();
            }
        },
        _0000216_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('#0000216_Value');
            }
            var val = $('#0000216_Value').val();
            if (val === undefined || val !== "U00000_12") {
                $('input[name="0000217_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000217_Value"]').closest('.question-root').show();
            }
        }
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
    
    $(function () {
        $('input[name="0000214_Value"]').datepicker({ minDate: '-0d' });
    });
});