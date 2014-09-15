window.procenter.InitializeOrganization = function (organizationBaseUri, organizationKey, addressModalId, addressesPropertyName, phoneModalId, phonesPropertyName) {
    var checkDisabled = function () {
        $('[data-disabled="true"]').find(":input").prop('disabled', true);
        $('[data-disabled="false"]').find(":input").prop('disabled', false);
    };

    checkDisabled();

    var removeAddressHandler = function () {
        $(".question-answer-wrapper #removeOrganizationAddress").each(function (idx, obj) {
            $(this).ajaxLink({
                url: organizationBaseUri + '/RemoveAddress/?addressHash=' + $(this).attr("data-val"),
                success: function (data) {
                    if (data.success) {
                        $(obj).closest("li").remove();
                    }
                },
                getData: function () {
                    if ($(this).attr("data-val") == "") {
                        return false;
                    }
                    if (!confirm($(this).attr("title"))) {
                        return false;
                    }
                    var data = {};
                    return data;
                },
                fail: function (data) {
                }
            });
        });
    }

    var removePhoneHandler = function () {
        $(".question-answer-wrapper #removeOrganizationPhone").each(function (idx, obj) {
            $(this).ajaxLink({
                url: organizationBaseUri + '/RemovePhone/?phoneHash=' + $(this).attr("data-val"),
                success: function(data) {
                    if (data.success) {
                        $(obj).closest("li").remove();
                    }
                },
                getData: function () {
                    if ($(this).attr("data-val") == "") {
                        return false;
                    }
                    if (!confirm($(this).attr("title"))) {
                        return false;
                    }
                    var data = {};
                    return data;
                },
                fail: function(data) {
                }
            });
        });
    }

    removeAddressHandler();
    removePhoneHandler();

    var $organizationEditor = $('.organization-editor');
    $organizationEditor.ajaxForm({
        url: organizationBaseUri + '/Edit/' + organizationKey,
        validate: true,
        success: function (data) {
            if (data.originalHash) {
                var hashInput = $organizationEditor.find("input[value='" + data.originalHash + "']");
                hashInput.val(data.newHash);
                if (data.newIsPrimary) {
                    var $list = hashInput.closest('ul');
                    var primary = $list.find("[id$='IsPrimary']:checked");
                    var newPrimary = hashInput.parent().find("[id$='IsPrimary']")[0];
                    if (primary.length > 1) {
                        primary.each(function () {
                            if (this != newPrimary) {
                                this.checked = false;
                            }
                        });
                    }
                }
            }
        }
    });

    $("#" + addressModalId + "-add-btn").ajaxLink({
        url: organizationBaseUri + '/AddAddress/' + organizationKey,
        success: function (data) {
            if (data == null) return;
            if (data.errors != null) {
                var br = "";
                var msg = "";
                for (var err=0; err < data.errors.length; err++) {
                    msg = br + data.errors[err].Message;
                    br = "</br>";
                }
                $("#form_errors").text(msg);
                return;
            }

            var $addressesList = $('.' + addressesPropertyName + ' ul');
            var $li = $("<li class='question-root' data-ajax-container='true' data-ajax-container-status='true'><form><div class='question-answer-wrapper'><div class='clear'></div></div></form></li>");

            $li.find("div:first-child").html(data);
            $li.find("div.question-answer-wrapper").prepend("<a id='removeOrganizationAddress' data-val='' role='button' data-icon='&#xe0a7;' class='remove-btn pull-right' title='Are you sure you want to delete this address?'></a>");
            $li.find("#removeOrganizationAddress").attr("data-val", $li.find("input:hidden#OriginalHash").attr("value"));
            var primaryInput = $addressesList.find('#OrganizationAddressDto_IsPrimary:checked');
            if (primaryInput.length > 0 && $li.find('#IsPrimary:checked').length > 0) {
                primaryInput.prop("checked", false);
            }
            $addressesList.append($li);
            removeAddressHandler();
            $.validator.unobtrusive.parse('.OrganizationAddresses'); // to re-initiate unobtrusive validation
            $("#" + addressModalId).modal("hide");
            $("#" + addressModalId).find('.modal-body :input').val('');
        },
        getData: function () {
            var data = {};
            if ($("#" + addressModalId).find('.modal-body form').valid()) {
                $("#" + addressModalId).find('.modal-body :input').each(function() {
                    if (!data.hasOwnProperty(this.name)) {
                        data[this.name] = this.value;
                    }
                });
                return data;
            }
            return false;
        }
    });

    $("#" + phoneModalId + "-add-btn").ajaxLink({
        url: organizationBaseUri + '/AddPhone/' + organizationKey,
        success: function(data) {
            var $addressesList = $('.' + phonesPropertyName + ' ul');
            var $li = $("<li class='question-root' data-ajax-container='true' data-ajax-container-status='true'><form><div class='question-answer-wrapper'><div class='clear'></div></div></form></li>");
            $li.find("div:first-child").html(data);
            $li.find("div.question-answer-wrapper").prepend("<a id='removeOrganizationPhone' data-val='' role='button' data-icon='&#xe0a7;' class='remove-btn pull-right' title='Are you sure you want to delete this phone number?'></a>");
            $li.find("#removeOrganizationPhone").attr("data-val", $li.find("input:hidden#OriginalHash").attr("value"));
            var primaryInput = $addressesList.find('#OrganizationPhoneDto_IsPrimary:checked');
            if (primaryInput.length > 0 && $li.find('#IsPrimary:checked').length > 0) {
                primaryInput.prop("checked", false);
            }
            $addressesList.append($li);
            removePhoneHandler();
            $.validator.unobtrusive.parse('.OrganizationPhones'); // to re-initiate unobtrusive validation
            $("#" + phoneModalId).modal("hide");
            $("#" + phoneModalId).find('.modal-body :input').val('');
        },
        getData: function() {
            var data = {};
            if ($("#" + phoneModalId).find('.modal-body form').valid()) {
                $("#" + phoneModalId).find('.modal-body :input').each(function() {
                    if (!data.hasOwnProperty(this.name)) {
                        data[this.name] = this.value;
                    }
                });
                return data;
            }
            return false;
        }
    });

    $('#add-OrganizationAddresses-modal').ajaxLink({
        type: 'GET',
        getData: function () {
            $('#add-OrganizationAddresses-loading-indicator').show();
            $('#add-OrganizationAddresses-Modal').modal('show');
            return {};
        },
        success: function (data) {
            $('#add-OrganizationAddresses-Modal .save-btn').show();
            $('#add-OrganizationAddresses-Modal .modal-error').text('');
            $('#add-OrganizationAddresses-Modal .modal-body').html(data);
        },
        error: function (jqXhr, status, error) {
            $('#add-OrganizationAddresses-Modal .modal-error').text(error);
            $('#add-OrganizationAddresses-Modal .save-btn').hide();
        },
        complete: function () {
            $('#patient-create-loading-indicator').hide();
        }
    });

    $("#org-admin-btn").ajaxLink({
        url: organizationBaseUri + '/CreateOrganizationAdmin/' + organizationKey,
        success: function () {
            $('#createOrganizationAdminModal').modal("hide");
        },
        error: function (jqXhr) {
            var $error = $('#createOrganizationAdminModal div.field-validation-error');
            $error.removeClass('hidden');
            $error.html(jqXhr.statusText);
        },
        getData: function () {
            var data = {};
            var valid = true;
            $('#createOrganizationAdminModal').find('.modal-body :input').each(function () {
                data[this.name] = this.value;
                valid = valid & $(this).valid();
            });

            if (!valid) {
                return false;
            }
            return data;
        }
    });
}