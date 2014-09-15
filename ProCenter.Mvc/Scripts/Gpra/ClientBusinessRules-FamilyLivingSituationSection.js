$(document).ready(function () {
    var myRules = {
        _0000136_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('#0000136_Value');
            }
            var val = $('#0000136_Value').val();
            if (val === undefined || val !== "F00000_3") {
                $("#0000137_Value").val(null).closest('.question-root').hide();
            } else {
                $("#0000137_Value").closest('.question-root').show();
            }
        },
        _0000137_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('#0000137_Value');
            }
            var val = $('#0000137_Value').val();
            if (val === undefined || val !== "G00000_5") {
                $('input[name="0000138_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000138_Value"]').closest('.question-root').show();
            }
        },
        _0000144_ValueRules: function () {             if (!$.rules.init) {                 $.rules.Utilities.AddSelector('input[name="0000144_Value"]');            }             var val = $ ('input[name="0000144_Value"]').val();             if (val === undefined || val === "" || val < 0) {                $('input[name="0000144_Value"]').val(null);
            } 
        },
    _0000146_ValueRules: function () {         if (!$.rules.init) {             $.rules.Utilities.AddSelector('input[name="0000146_Value"]');        } var val = $('input[name="0000146_Value"]').val();         if (val === undefined || val === "" || val < 0) {             $ ('input[name="0000146_Value"]').val(null);
        } 
    },
    _0000147_ValueRules: function () {         if (!$.rules.init) {             $.rules.Utilities.AddSelector('input[name="0000147_Value"]');        }         var val = $('input[name="0000147_Value"]').val();         if (val === undefined || val === "" || val < 0) {             $('input[name="0000147_Value"]').val(null);
        }
    },
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
});