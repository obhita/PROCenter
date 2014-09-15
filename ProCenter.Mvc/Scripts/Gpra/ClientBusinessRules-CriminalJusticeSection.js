$(document).ready(function () {
    var myRules = {
        _0000161_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000161_Value"]');
            }
            var val = $('input[name="0000161_Value"]').val();
            if (val === undefined || val === "" || val < 1) {
                if (val < 0) {
                    $('input[name="0000161_Value"]').val(null);
                }
                $('input[name="0000162_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000162_Value"]').closest('.question-root').show();
            }
        },
        _0000162_ValueRules: function () { 
            if (!$.rules.init) { 
                $.rules.Utilities.AddSelector('input[name="0000162_Value"]');
            } 
            var val = $('input[name="0000162_Value"]').val(); 
            if (val === undefined || val === "" || val < 0) { 
                $('input[name="0000162_Value"]').val(null);
            } 
        },
        _0000163_ValueRules: function () { 
            if (!$.rules.init) { 
                $.rules.Utilities.AddSelector('input[name="0000163_Value"]');
            } 
            var val = $('input[name="0000163_Value"]').val(); 
            if (val === undefined || val === "" || val < 1) { 
                $('input[name="0000163_Value"]').val(null);
            } 
        },
        _0000164_ValueRules: function () { if (!$.rules.init) {             $.rules.Utilities.AddSelector('input[name="0000164_Value"]');            }             var val = $('input[name="0000164_Value"]').val();             if (val === undefined || val === "" || val < 1) {
                $('input[name="0000164_Value"]').val(null);
                
            }
        },
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));


});