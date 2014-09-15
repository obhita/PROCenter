(function ($) {
    var defaults = {
        type: "POST",
        url: '',
        traditional: true,
        defaultLocation: "/",
        selectedClass: "selected",
        template: {
            button: '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>',
            tab: '<li class="patient-tab" data-key="{0}"><a class="{3}" href="{1}" aria-label="{4}"><span class="fs1" aria-hidden="true" data-icon="&#xe075;">{2}</span></a></li>'
        }
    };

    var storage = sessionStorage;

    var methods = {
        init: function ($container, settings) {
            settings.$container = $container;

            var patients = undefined;
            if (storage.patients) {
                patients = $.parseJSON(storage.patients);
            }
            if (!patients) {
                patients = new Array();
                storage.patients = JSON.stringify(patients);
            }
            settings.patients = new Array();
            
            var tabs = $container.find("> li.patient-tab");
            var selectedKey = $container.find("> li.patient-tab a.selected").parent().data("key");
            var htmlTabs = "";
            for (var i = 0; i < patients.length; i++) {
                var patient = patients[i];
                methods.addTab(settings, patient, false);
                var tabClass = "";
                if (patient.key == selectedKey) {
                    tabClass = settings.selectedClass;
                }
                var flippedName = window.procenter.stringFlip(patient.name);
                htmlTabs += window.procenter.stringFormat(settings.template.tab, patient.key, settings.url + patient.key, patient.name, tabClass, flippedName + " tab");
            }
            tabs.remove();
            var nonPatientTabsHtml = "";
            $container.find("> li:not(.patient-tab)").each(function () { nonPatientTabsHtml += this.outerHTML; });
            $container.html(nonPatientTabsHtml + htmlTabs);
            
            $container.find("> li.patient-tab").find('a').append(settings.template.button);
            
            $container.on("click", ".close", function (evt) {
                evt.stopImmediatePropagation();
                evt.preventDefault();
                var self = $(this);
                var patientTab = self.closest('.patient-tab');
                var index = patientTab.index();
                methods.removeTab(settings, patientTab);
                if(patientTab.find('a').hasClass('selected')){
                    if (settings.patients.length == 0) {
                        window.location = settings.defaultLocation;
                    } else {
                        var href = settings.$container.find("li a")[index - 1].href;
                        window.location = href;
                    }
                }
            });
            $.data($container[0], "tabManagerSettings", settings);
        },
        addTab: function (settings, patient, addHtml) {
            for (var i = 0; i < settings.patients.length; i++) {
                if (settings.patients[i].key == patient.key) {
                    var obj = $.parseJSON(storage.patients);
                    $.each(obj, function (idx, pat) {
                        if (pat.key == patient.key) {
                            pat.name = patient.name;
                            storage.patients = JSON.stringify(obj);
                            settings.$container.find("li.patient-tab[data-key=" + patient.key + "]").children().children(".fs1").html(patient.name);
                            var flippedName = window.procenter.stringFlip(patient.name);
                            settings.$container.find("li.patient-tab[data-key=" + patient.key + "] a").attr("aria-label", flippedName + " tab");
                            return;
                        }
                    });
                    return;
                }
            }

            if (addHtml == undefined) {
                addHtml = true;
            }
            settings.patients.push(patient);
            storage.patients = JSON.stringify(settings.patients);
            if (addHtml) {
                var flippedName = window.procenter.stringFlip(patient.name);
                var tab = $(window.procenter.stringFormat(settings.template.tab, patient.key, settings.url + patient.key, patient.name, settings.selectedClass, flippedName + " tab"));
                tab.find('a').append(settings.template.button);
                settings.$container.append(tab);
            }
        },
        removeTab: function (settings, patientTab) {
            var patientKey = patientTab.data("key");
            patientTab.remove();
            for (var i = 0; i < settings.patients.length; i++) {
                var patient = settings.patients[i];
                if (patient.key == patientKey) {
                    settings.patients.splice(i, 1);
                    break;
                }
            }
            storage.patients = JSON.stringify(settings.patients);
        }
    };
    $.fn.tabManager = function (options) {
        if (!storage) {
            return null;
        }
        
        var defaultSettings = defaults;
        var method = undefined;
        if (typeof options === "object") {
            $.extend(defaultSettings, options);
        }
        else if (typeof options === "string") {
            method = options;
        }
        var args = Array.prototype.slice.call(arguments, 1);
        args.unshift
        return this.each(function () {
            var $self = $(this);
            var settings = $.data(this, "tabManagerSettings");
            if (!settings) {
                settings = jQuery.extend(true, {}, defaultSettings);
            } 
            if (method && methods[method]) {
                var methodArgs = args.slice(0);
                methodArgs.unshift(settings);
                return methods[method].apply($self, methodArgs);
            } else {
                return methods.init($self, settings);
            }
        });
    };
})(jQuery);