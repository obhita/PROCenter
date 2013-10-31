$(document).ready(function() {
    $.rules = {
        init: false,
        changeSelector: "",
        Initialize: function(form) {
            $.rules.init = false;
            $.rules.changeSelector = "";
            var rulesContainer = $.rules.CompletenessRules;
            if (rulesContainer != undefined) {
                for (var ruleName in rulesContainer) {
                    var r = rulesContainer[ruleName];
                    if (typeof r === 'function') {
                        r();
                    }
                }
            }

            form.on('change', $.rules.changeSelector, function(e) {
                var idRule = "_" + this.id + "Rules",
                    nameRule = "_" + this.name + "Rules";
                var rule = rulesContainer[idRule];
                if (typeof rule === 'function') {
                    rule();
                    return;
                }
                rule = rulesContainer[nameRule];
                if (typeof rule === 'function') {
                    rule();
                }
            });

            $.rules.init = true;
            $.rules.changeSelector = "";
        },
        Utilities: {
            AddSelector: function(selector) {
                if ($.rules.changeSelector && $.rules.changeSelector.length > 0) {
                    $.rules.changeSelector = $.rules.changeSelector + "," + selector;
                } else {
                    $.rules.changeSelector = selector;
                }
            }
        },
        CompletenessRules: {},
    };
});
