window.procenter.InitializePatient = function (patientBaseUri, teamBaseUri, teamApiBaseUri) {
    var patientKey = $('#Key').val();
    var checkDisabled = function () {
        $('[data-disabled="true"]').find(":input").prop('disabled', true);
        $('[data-disabled="false"]').find(":input").prop('disabled', false);
    };

    checkDisabled();

    function addPatientModalValidators() {
        $('#DateOfBirth').datepicker({
            maxDate: '+0d',
            onSelect: function () {
                $(this).trigger('blur');
            }
        });
        $.validator.addMethod("noFutureDate", function (value) {
            if (value !== undefined) {
                var dob = Date.parse(value);
                var today = new Date();
                if (dob > today) {
                    return false;
                }
                return true;
            }
            return true;
        }, "Birth Date cannot be a future date."); //todo: validation message localization

        $('#DateOfBirth').rules('add', { noFutureDate: true });
        $.validator.addMethod("validDate", function (value) {
            var bits = value.split('/');
            var d = new Date(bits[2], bits[0] - 1, bits[1]);
            var isValid = (d.getMonth()) == bits[0] - 1 && d.getDate() == Number(bits[1]);
            return isValid;
        }, "The field Birth Date must be a date in format mm/dd/yyyy."); //todo: validation message localization
        $('#DateOfBirth').rules('add', { validDate: true });
    }

    $('.patient-editor .patient-information').ajaxForm({
        url: patientBaseUri + '/Edit/' + patientKey,
        validate: true,
        success: function() {
            // ensures that the tab for this patient is at the top and selected
            window.procenter.patientTabManager = $('.top-nav ul').tabManager({ url: '/Patient/Index/' });
            window.procenter.patientTabManager.tabManager("addTab", { key: patientKey, name: $("#Name_LastName").val() + ', ' + $("#Name_FirstName").val() });
        }
    });

    addPatientModalValidators();

    $("#change-password-btn").ajaxLink({
        contentType: "application/json",
        getData: function () {
            var data = {};
            var isValid = true;
            $('#changePasswordModal').find('.modal-body :input').each(function () {
                data[this.name] = this.value;
                isValid = isValid & $(this).valid();
            });
            if (!isValid) {
                return false;
            }
            return JSON.stringify(data);
        },
        success: function (data) {
            var resultCode = data[0];
            var $changePasswordMessage = $('#change-password-message');
            $changePasswordMessage.html(data[1]);
            $changePasswordMessage.removeClass('hidden');
            if (resultCode === "Succeed") {
                $('#changePasswordModal .modal-body').hide();
                $('#changePasswordModal .modal-footer').hide();
                $changePasswordMessage.css('color', 'green');
            }
        }
    });

    var addTeams = function (teams) {
        var $list = $('.patient-editor .current-teams');
        var listItems = '';
        for (var i = 0; i < teams.length; i++) {
            var team = teams[i];
            if ($list.html().indexOf(team.Key) == -1) {
                listItems += '<li id=' + team.Key + '><span>' + team.Name + '</span><a data-icon="&#xe0a7;" class="remove-btn"></a><div class="modal-loading-indicator hidden"></div></li>';
            }
        }
        if (listItems.length > 0) {
            $list.html($list.html() + listItems);
        }
    };

    $.getJSON(teamApiBaseUri + '/GetByPatientKey?patientKey=' + patientKey, function (results) {
        addTeams(results); 
    });
    
    $('.patient-editor .current-teams').on('click', '.remove-btn', function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        var listItem = $(this).parent();
        var id = listItem[0].id;
        var $loader = listItem.find('.modal-loading-indicator').show();
        var keysArray = new Array();
        keysArray.push(patientKey);
        $.ajax({
            type: "POST",
            url: teamBaseUri + '/RemovePatients/' + id,
            data: { patientKeysToRemove: keysArray },
            traditional: true
        }).done(function () {
            listItem.remove();
        }).fail(function () {
            alert('Server error please retry.');
        })
          .always(function () { $loader.hide(); });
    });

    $('.patient-editor .teams').on('selectionChanged', function (evt, data) {
        if (data && $('#' + data.Key).length === 0) {
            $('.patient-editor .teams .add-btn').removeAttr("disabled").attr("href", teamBaseUri + '/AddPatients/' + data.Key);
        } else {
            $('.patient-editor .teams .add-btn').attr("disabled", "disabled");
        }
    });

    $('.patient-editor .teams .add-btn').ajaxLink({
        getData: function () {
            var patientGuids = new Array();
            patientGuids.push(patientKey);

            return { patientKeysToAdd: patientGuids };
        },
        success: function () {
            var finder = $('.patient-editor .teams [data-control="finder"]').finder();
            var team = finder.selectedData;
            if (team) {
                addTeams([team]);
            }
            finder.clearSelected();
        }
    });
    
    function setSystemAjaxLinks() {
        $(".system-account-lock").ajaxLink({
            success: function (data, $link) {
                $link.text(data.text);
                $link.attr('href', data.location);
            }
        });

        $(".system-account-reset-password").ajaxLink({
            type: 'POST',
            success: function (data) {
                if (data) {
                    if (data.Data.text != "" || data.Data.error != "") {
                        var msg = data.Data.text + data.Data.error;
                        $('#messageModal .modal-body').text(msg);
                        $("#messageModal").modal("show");
                    }
                }
            },
            error: function () {
            }
        });
    }

    //create account
    $("#create-account-btn").ajaxLink({
        url: patientBaseUri + '/CreateAccount/' + patientKey,
        success: function (data) {
            if (data.error) {
                var $error = $('#system-account-content div.field-validation-error');
                $error.removeClass('hidden');
                var message = "";
                var br = "";
                for (var i = 0; i < data.errors.length; i++) {
                    message += br + data.errors[i].Message;
                    br = "</br>";
                }
                $error.html(message);
            } else {
                var $account = $('#system-account-content');
                $('#createAccountModal').modal("hide");
                $account.html(data);
                var closeButton = $account.find("button.close[data-dismiss='modal']").first();
                closeButton.on("click", function () {
                    $("#messageModal").modal("hide");
                });
                setSystemAjaxLinks();
                $('#system-account-content .system-account').find(":input").prop('disabled', true);
            }
        },
        error: function (jqXhr) {
            var $error = $('#system-account-content div.field-validation-error');
            $error.removeClass('hidden');
            $error.html(jqXhr.statusText);
        },
        getData: function () {
            var data = {};
            var valid = true;
            $('#createAccountModal').find('.modal-body :input').each(function () {
                data[this.name] = this.value;
                valid = valid & $(this).valid();
            });

            if (!valid) {
                return false;
            }
            return data;
        }
    });
    
    $('#createAccountModal').on("hidden", function () {
        $('#system-account-content div.field-validation-error').addClass('hidden');
    });

    $('#createAccountModal').on('show', function () {
        $('#createAccountModal #systemAccount_Email').val($('#Email').val());
    });

    setSystemAjaxLinks();
    // ensures that the tab for this patient is at the top and selected
    window.procenter.patientTabManager = $('.top-nav ul').tabManager({ url: '/Patient/Index/' });
    window.procenter.patientTabManager.tabManager("addTab", { key: patientKey, name: $("#Name_LastName").val() + ', ' + $("#Name_FirstName").val() });
    $(".editor-content").find(":input:not(:button):not(:hidden):enabled[data-val=true]:first").focus();
}