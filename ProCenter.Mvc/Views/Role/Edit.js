window.procenter.InitializeRole = function(roleBaseUri, roleDataTableUri, updateTableSizes, checkDisabled, canAccessRoleEdit) {
    $("#create-role-btn").ajaxLink({
        getData: function() {
            var data = {};
            var isValid = true;
            $('#createRoleModal').find('.modal-body :input').each(function() {
                data[this.name] = this.value;
                isValid = isValid & $(this).valid();
            });
            if (!isValid) {
                return false;
            }
            return data;
        },
        success: function(data) {
            showRoleEditor(data);
            $('#createRoleModal').modal("hide");
        }
    });

    var roleSearchTable = $('#roleSearchDataTable').dataTable({
        "sDom": "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "sScrollY": "100%",
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bScrollCollapse": true,
        "bAutoWidth": true,
        "bProcessing": true,
        "bServerSide": true,
        "bSort": false,
        "sAjaxSource": roleDataTableUri,
        "aoColumns": [
            {
                "mData": "Name",
                "sClass": "FirstColumn",
            },
            { "mData": "Key", "bSortable": false, "bSearchable": true, "sClass": "hidden" }
        ],
        "fnDrawCallback": function() {
            updateTableSizes();
        }
    });

    var initializeRoleEditor = function() {
        var $roleEditor = $('.role-editor');
        var roleKey = $('#current-role-key').val();

        $roleEditor.ajaxForm({
            url: roleBaseUri + '/Edit/' + roleKey,
            success: function() {
            }
        });

        $('#available-permissions').change(function() {
            if ($("#available-permissions :selected").length > 0) {
                $("#add-permissions-btn").removeClass("disabled");
            } else {
                $("#add-permissions-btn").addClass("disabled");
            }
        });

        $('#current-permissions').change(function() {
            if ($("#current-permissions :selected").length > 0) {
                $("#remove-permissions-btn").removeClass("disabled");
            } else {
                $("#remove-permissions-btn").addClass("disabled");
            }
        });

        var $container = $('#permissions-container');
        $("#add-permissions-btn").ajaxLink({
            url: roleBaseUri + '/AddPermissions/',
            success: function() {
                $container.attr("data-ajax-status", "success");
                $("#available-permissions > option:selected").each(function() {
                    $(this).remove().appendTo("#current-permissions");
                });
                $("#available-permissions").change();
                $("#current-permissions").change();
            },
            getData: function() {
                $container.attr("data-ajax-status", "loading");
                var data = {};
                data["permissions"] = new Array();
                $('#available-permissions option:selected').each(function() {
                    data["permissions"].push(this.value);
                });
                return data;
            },
            error: function() {
                $container.attr("data-ajax-status", "fail");
            },
            complete: function() {
                setTimeout(function() { $container.removeAttr("data-ajax-status"); }, 8000);
            }
        });

        $("#remove-permissions-btn").ajaxLink({
            url: roleBaseUri + '/RemovePermissions/',
            success: function() {
                $container.attr("data-ajax-status", "success");
                $("#current-permissions > option:selected").each(function() {
                    $(this).remove().appendTo("#available-permissions");
                });
                $("#available-permissions").change();
                $("#current-permissions").change();
            },
            getData: function() {
                $container.attr("data-ajax-status", "loading");
                var data = {};
                data["permissions"] = new Array();
                $('#current-permissions option:selected').each(function() {
                    data["permissions"].push(this.value);
                });
                return data;
            },
            error: function() {
                $container.attr("data-ajax-status", "fail");
            },
            complete: function() {
                setTimeout(function() { $container.removeAttr("data-ajax-status"); }, 8000);
            }
        });
    };

    var showRoleGrid = function(widget) {
        $(widget).find('#role-editor').hide();
        $(widget).find('#role-editor').html('');
        $(widget).find('#role-table').show();
        roleSearchTable.fnDraw();
    };

    var showRoleEditor = function(innerHtml) {
        var $widget = $('#role-widget');
        $widget.find('#role-editor').html(innerHtml);
        initializeRoleEditor();
        $('.dashboard-wrapper').dashboard('expand', $widget[0], showRoleGrid);
        $widget.find('#role-editor').show();
        $widget.find('.dataTable_wrapper').hide();
    };

    if (canAccessRoleEdit) {
        $("#roleSearchDataTable tbody").click(function(event) {
            var key = roleSearchTable.fnGetData(event.target.parentNode).Key;
            $.get(roleBaseUri + '/Edit/' + key, showRoleEditor);
        });
    }
}