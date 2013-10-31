window.procenter.InitializeOrganization = function (organizationBaseUri, organizationKey, addressModalId, addressesPropertyName, phoneModalId, phonesPropertyName) {
    var checkDisabled = function () {
        $('[data-disabled="true"]').find(":input").prop('disabled', true);
        $('[data-disabled="false"]').find(":input").prop('disabled', false);
    };

    checkDisabled();

    var $organizationEditor = $('.organization-editor');
    $organizationEditor.ajaxForm({
        url: organizationBaseUri + '/Edit/' + organizationKey,
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
            var $addressesList = $('.' + addressesPropertyName + ' ul');
            var $li = $("<li class='question-root'><div><div class='clear'></div></div></li>");
            $li.find(":first-child").html(data);
            var primaryInput = $addressesList.find('#OrganizationAddressDto_IsPrimary:checked');
            if (primaryInput.length > 0 && $li.find('#IsPrimary:checked').length > 0) {
                primaryInput.prop("checked", false);
            }
            $addressesList.append($li);
            $("#" + addressModalId).modal("hide");
            $("#" + addressModalId).find('.modal-body :input').val('');
        },
        getData: function () {
            var data = {};
            $("#" + addressModalId).find('.modal-body :input').each(function () {
                if (!data.hasOwnProperty(this.name)) {
                    data[this.name] = this.value;
                }
            });
            return data;
        }
    });
    $("#" + phoneModalId + "-add-btn").ajaxLink({
        url: organizationBaseUri + '/AddPhone/' + organizationKey,
        success: function (data) {
            var $addressesList = $('.' + phonesPropertyName + ' ul');
            var $li = $("<li class='question-root'><div><div class='clear'></div></div></li>");
            $li.find(":first-child").html(data);
            var primaryInput = $addressesList.find('#OrganizationPhoneDto_IsPrimary:checked');
            if (primaryInput.length > 0 && $li.find('#IsPrimary:checked').length > 0) {
                primaryInput.prop("checked", false);
            }
            $addressesList.append($li);
            $("#" + phoneModalId).modal("hide");
            $("#" + phoneModalId).find('.modal-body :input').val('');
        },
        getData: function () {
            var data = {};
            $("#" + phoneModalId).find('.modal-body :input').each(function () {
                if (!data.hasOwnProperty(this.name)) {
                    data[this.name] = this.value;
                }
            });
            return data;
        }
    });
}