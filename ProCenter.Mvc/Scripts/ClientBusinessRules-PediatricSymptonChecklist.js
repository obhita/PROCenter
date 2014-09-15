$(document).ready(function() {
    var myRules = {
        _71250039_ValueRules: function () {
            if (!$.rules.init) {
                $.rules.Utilities.AddSelector('input[name="71250039_Value"]');
            }
            var val = $('input[name="71250039_Value"]').prop('checked');
            if (val === undefined || !val) {
                $('[name="71250040_Value"]').val("").closest('.question-root').hide();
            } else {
                $('[name="71250040_Value"]').closest('.question-root').show();
            }
        },
    };

    $.extend($.rules.CompletenessRules, myRules);
    $.rules.Initialize($('#contentDiv'));
});