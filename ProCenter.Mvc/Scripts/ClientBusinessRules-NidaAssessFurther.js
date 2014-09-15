$(document).ready(function() {
    var myRules = {
        _3269985_ValueRules: function() {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="3269985_Value"]');
            }
            var val = $('input[name="3269985_Value"]').val().trim();
            if (val === undefined || val === "") {
                $("#3269984_Value").val("").closest('.question-root').hide();
            } else {
                $("#3269984_Value").closest('.question-root').show();
            }
        },
        _3269978_ValueRules: function() {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="3269978_Value"]');
            }
            var val = $('input[name="3269978_Value"]').prop('checked');
            if (val === undefined || !val) {
                $("#3269986_Value").val("").closest('.question-root').hide();
            } else {
                $("#3269986_Value").closest('.question-root').show();
            }
        }
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
});