$(document).ready(function () {
    var myRules = {
        _0000168_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000168_Value"]');
            }
            var val = $('input[name="0000168_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000169_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000169_Value"]').closest('.question-root').show();
            }
        },
        _0000170_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000170_Value"]');
            }
            var val = $('input[name="0000170_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000171_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000171_Value"]').closest('.question-root').show();
            }
        },
        _0000172_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000172_Value"]');
            }
            var val = $('input[name="0000172_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000173_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000173_Value"]').closest('.question-root').show();
            }
        },
        _0000174_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000174_Value"]');
            }
            var val = $('input[name="0000174_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000175_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000175_Value"]').closest('.question-root').show();
            }
        },
        _0000176_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000176_Value"]');
            }
            var val = $('input[name="0000176_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000177_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000177_Value"]').closest('.question-root').show();
            }
        },
        _0000178_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000178_Value"]');
            }
            var val = $('input[name="0000178_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000179_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000179_Value"]').closest('.question-root').show();
            }
        },
        _0000180_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000180_Value"]');
            }
            var val = $('input[name="0000180_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000181_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000181_Value"]').closest('.question-root').show();
            }
        },
        _0000182_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000182_Value"]');
            }
            var val = $('input[name="0000182_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000183_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000183_Value"]').closest('.question-root').show();
            }
        },
        _0000184_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000184_Value"]');
            }
            var val = $('input[name="0000184_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000185_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000185_Value"]').closest('.question-root').show();
            }
        },
        _0000186_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000186_Value"]');
            }
            var val = $('input[name="0000186_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000187_Value"]').val(null).closest('.question-root').hide();
                $('input[name="0000188_Value"]').val(null).closest('.question-root').hide();
                $('input[name="0000189_Value"]').val(null).closest('.question-root').hide();
                $('input[name="0000190_Value"]').val(null).closest('.question-root').hide();
                $('input[name="0000191_Value"]').val(null).closest('.question-root').hide();
            } else {
                $('input[name="0000187_Value"]').closest('.question-root').show();
                $('input[name="0000188_Value"]').closest('.question-root').show();
                $('input[name="0000189_Value"]').closest('.question-root').show();
                $('input[name="0000190_Value"]').closest('.question-root').show();
                $('input[name="0000191_Value"]').closest('.question-root').show();
            }
        },
        _0000192_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="0000192_Value"]');
            }
            var val = $('input[name="0000192_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('input[name="0000193_Value"]').removeAttr('checked').closest('.question-root').hide();
            } else {
                $('input[name="0000193_Value"]').closest('.question-root').show();
            }
        },
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
});