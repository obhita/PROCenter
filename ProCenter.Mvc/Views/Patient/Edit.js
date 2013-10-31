window.procenter.InitializePatient = function (patientBaseUri, teamBaseUri, teamApiBaseUri) {
    var patientKey = $('#Key').val();
    var checkDisabled = function () {
        $('[data-disabled="true"]').find(":input").prop('disabled', true);
        $('[data-disabled="false"]').find(":input").prop('disabled', false);
    };

    checkDisabled();

    function isValidDate(dtValue) {
        var timestamp = Date.parse(dtValue);
        return !isNaN(timestamp);
    }

    $('#DateOfBirth').datepicker({
        maxDate: '+0d',
        onSelect:function() {
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
        return isValidDate(value);
    }, "The field Birth Date must be a date in format mm/dd/yyyy.");
    $('#DateOfBirth').rules('add', { validDate: true });

    $('.patient-editor .patient-information').ajaxForm({
        url: patientBaseUri + '/Edit/' + patientKey,
        validate: true,
        success: function() {
            
        }
    });
    
    var addTeams = function (teams) {
        var $list = $('.patient-editor .current-teams');
        var listItems = '';
        for (var i = 0; i < teams.length; i++) {
            var team = teams[i];
            listItems += '<li id=' + team.Key + '><span>' + team.Name + '</span><a data-icon="&#xe0a7;" class="remove-btn"></a><div class="modal-loading-indicator hidden"></div></li>';

        }
        $list.html($list.html() + listItems);
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
            addTeams([team]);
            finder.clearSelected();
        }
    });
    
    //create account
    $("#create-account-btn").ajaxLink({
        url: patientBaseUri + '/CreateAccount/' + patientKey,
        success: function (data) {
            var $account = $('#system-account-content');
            $('#createAccountModal').modal("hide");
            $account.html(data);
            $('#system-account-content .system-account').find(":input").prop('disabled', true);
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
        //$('#createAccountModal #systemAccount_Email').val($('#Email').val());
        function isValidUsername(value) {
            var dtRegex = new RegExp(/^[a-zA-Z0-9._@]+$/);
            return dtRegex.test(value);
        }
        $.validator.addMethod("validUsername", function (value) {
            return isValidUsername(value);
        }, "The username can only contain letters, numbers, dot(.), at sign(@) and underscore(_).");
        $('#systemAccount_Username').rules('add', { validUsername: true });
    });
}