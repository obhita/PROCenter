$(document).ready(function () {
    var myRules = {
        _0000019_ValueRules: function() {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000019_Value"]');
            }
            var val = $('input[name="0000019_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000020_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000020_Value"]').closest('.question-root').show();
            }
        },
        _0000033_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000033_Value"]');
            }
            var val = $('input[name="0000033_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000034_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000034_Value"]').closest('.question-root').show();
            }
        },
        _0000043_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000043_Value"]');
            }
            var val = $('input[name="0000043_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000044_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000044_Value"]').closest('.question-root').show();
            }
        },
        _0000048_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000048_Value"]');
            }
            var val = $('input[name="0000048_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000049_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000049_Value"]').closest('.question-root').show();
            }
        },
        _0000055_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000055_Value"]');
            }
            var val = $('input[name="0000055_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000056_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000056_Value"]').closest('.question-root').show();
            }
        },
        _0000059_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000059_Value"]');
            }
            var val = $('input[name="0000059_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000060_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000060_Value"]').closest('.question-root').show();
            }
        },
        _0000065_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000065_Value"]');
            }
            var val = $('input[name="0000065_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000066_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000066_Value"]').closest('.question-root').show();
            }
        }
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
});