$(document).ready(function () {
    var myRules = {
        _0000202_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000202_Value"]');
            }
            var val = $('input[name="0000202_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000203_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000203_Value"]').closest('.question-root').show();
            }
        },
        _0000203_ValueRules: function() {
             if (!$.rules.init) {
                 $.rules.Utilities.AddSelector('input[name="0000203_Value"]');
             }
             var val = $('input[name="0000203_Value"]').val();
             if (val === undefined || val === "" || val < 0) {
                  $('input[name="0000203_Value"]').val(null);
             }
        },
        _0000204_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000204_Value"]');
            }
            var val = $('input[name="0000204_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000205_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000205_Value"]').closest('.question-root').show();
            }
        },
        _0000205_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000205_Value"]');
            } var val = $('input[name="0000205_Value"]').val();
            if (val === undefined || val === "" || val < 0) {
                $('input[name="0000205_Value"]').val(null);
            }
        },
        _0000206_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000206_Value"]');
            }
            var val = $('input[name="0000206_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000207_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000207_Value"]').closest('.question-root').show();
            }
        },
        _0000207_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000207_Value"]');
            }
            var val = $('input[name="0000207_Value"]').val();
            if (val === undefined || val === "" || val < 0) {
                $('input[name="0000207_Value"]').val(null);
            }
        },
        _0000209_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('#0000209_Value');
            }
            var val = $('#0000209_Value').val();
            if (val === undefined || val !== "Q00000_4") {
                $('input[name="0000210_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000210_Value"]').closest('.question-root').show();
            }
        }
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
    

});