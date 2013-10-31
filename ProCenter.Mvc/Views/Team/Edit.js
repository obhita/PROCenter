window.procenter.InitializeTeam = function(teamBaseUri, teamDataTableUri, staffDataTableUri, patientDataTableUri, updateTableSizes, checkDisabled, canAccessTeamEdit) {
    var $teamEditor = $('#team-editor');
    var initializeTeamEditor = function() {
        var $addStaffBtn = $teamEditor.find('.staff-list-editor a.add');
        var $removeStaffBtn = $teamEditor.find('.staff-list-editor a.remove');
        var $addPatientBtn = $teamEditor.find('.patient-list-editor a.add');
        var $removePatientBtn = $teamEditor.find('.patient-list-editor a.remove');
        var currentTeamKey = $teamEditor.find('#current-team-key').val();
                
        $teamEditor.ajaxForm({
            url: teamBaseUri + '/Edit/' + currentTeamKey,
            success: function () {
            }
        });

        $addStaffBtn.ajaxLink({
            url: teamBaseUri + '/AddStaff/' + currentTeamKey,
            getData: function() {
                var staffGuids = new Array();
                var selectedStaff = TableTools.fnGetInstance('staffAvailableDataTable').fnGetSelectedData();
                for (var index = 0; index < selectedStaff.length; index++) {
                    staffGuids.push(selectedStaff[index].Key);
                }
                return { staffKeysToAdd: staffGuids };
            },
            success: function(data) {
                var table = TableTools.fnGetInstance('staffAvailableDataTable');
                table.fnSelectNone();
                var tableData = staffAvailableTable.fnGetData();
                var currentDataKeys = staffCurrentTable.fnGetData().map(function(d) { return d.Key; });
                for (var index = 0; index < tableData.length; index++) {
                    if (data.indexOf(tableData[index].Key) !== -1 && currentDataKeys.indexOf(tableData[index].Key) === -1) {
                        staffCurrentTable.fnAddData(tableData[index]);
                    }
                }
                staffCurrentTable.fnDraw();
            }
        });

        $removeStaffBtn.ajaxLink({
            url: teamBaseUri + '/RemoveStaff/' + currentTeamKey,
            getData: function() {
                var staffGuids = new Array();
                var selectedStaff = TableTools.fnGetInstance('staffCurrentDataTable').fnGetSelected();
                for (var index = 0; index < selectedStaff.length; index++) {
                    staffGuids.push(selectedStaff[index].children[2].innerText);
                }
                return { staffKeysToRemove: staffGuids };
            },
            success: function(data) {
                var table = TableTools.fnGetInstance('staffCurrentDataTable');
                table.fnSelectNone();
                var tableData = staffCurrentTable.fnGetData();
                var indexCorrector = 0;
                for (var index = 0; index < tableData.length; index++) {
                    if (data.indexOf(tableData[index].Key) !== -1) {
                        staffCurrentTable.fnDeleteRow(index - indexCorrector);
                        indexCorrector++;
                    }
                }
            }
        });

        $addPatientBtn.ajaxLink({
            url: teamBaseUri + '/AddPatients/' + currentTeamKey,
            getData: function() {
                var keys = new Array();
                var selected = TableTools.fnGetInstance('patientAvailableDataTable').fnGetSelectedData();
                for (var index = 0; index < selected.length; index++) {
                    keys.push(selected[index].Key);
                }
                return { patientKeysToAdd: keys };
            },
            success: function(data) {
                var table = TableTools.fnGetInstance('patientAvailableDataTable');
                table.fnSelectNone();
                var tableData = patientAvailableTable.fnGetData();
                var currentDataKeys = patientCurrentTable.fnGetData().map(function(d) { return d.Key; });
                for (var index = 0; index < tableData.length; index++) {
                    if (data.indexOf(tableData[index].Key) !== -1 && currentDataKeys.indexOf(tableData[index].Key) === -1) {
                        patientCurrentTable.fnAddData(tableData[index]);
                    }
                }
                patientCurrentTable.fnDraw();
            }
        });

        $removePatientBtn.ajaxLink({
            url: teamBaseUri + '/RemovePatients/' + currentTeamKey,
            getData: function() {
                var keys = new Array();
                var selected = TableTools.fnGetInstance('patientCurrentDataTable').fnGetSelected();
                for (var index = 0; index < selected.length; index++) {
                    keys.push(selected[index].children[2].innerText);
                }
                return { patientKeysToRemove: keys };
            },
            success: function(data) {
                var table = TableTools.fnGetInstance('patientCurrentDataTable');
                table.fnSelectNone();
                var tableData = patientCurrentTable.fnGetData();
                var indexCorrector = 0;
                for (var index = 0; index < tableData.length; index++) {
                    if (data.indexOf(tableData[index].Key) !== -1) {
                        patientCurrentTable.fnDeleteRow(index - indexCorrector);
                        indexCorrector++;
                    }
                }
            }
        });

        var staffAvailableTable = $('#staffAvailableDataTable').dataTable({
            "sDom": "<'row-fluid'<'span6'T><'span6'f>r>t<'row-fluid'<'span5'i><'span7'p>>",
            "sPaginationType": "bootstrap",
            "sScrollY": "100%",
            "sScrollX": "100%",
            "sScrollXInner": "100%",
            "bScrollCollapse": true,
            "bAutoWidth": true,
            "bProcessing": true,
            "bServerSide": true,
            "bLengthChange": false,
            "bSort": false,
            "oLanguage": { "sSearch": "" },
            "oTableTools": {
                "sRowSelect": "multi",
                "aButtons": ["select_all", "select_none"],
                "fnRowSelected": function() {
                    $addStaffBtn.removeClass('disabled');
                },
                "fnRowDeselected": function() {
                    if (TableTools.fnGetInstance('staffAvailableDataTable').fnGetSelected().length == 0) {
                        $addStaffBtn.addClass('disabled');
                    }
                }
            },
            "sAjaxSource": staffDataTableUri,
            "aoColumns": [
                {
                    "mData": "Name.FirstName",
                    "sClass": "FirstColumn",
                },
                {
                    "mData": "Name.LastName",
                    "sClass": "Sue",
                },
                { "mData": "Key", "bSortable": false, "bSearchable": true, "sClass": "hidden" }
            ],
            "fnDrawCallback": function(oSettings) {
                $(oSettings.aanFeatures.f[0]).find('input').attr('placeholder', "Search");
                updateTableSizes();
            }
        });

        var staffCurrentTable = $('#staffCurrentDataTable').dataTable({
            "sDom": "<'row-fluid'<'span6'T><'span6'f>r>t<'row-fluid'<'span5'i><'span7'p>>",
            "sPaginationType": "bootstrap",
            "sScrollY": "100%",
            "sScrollX": "100%",
            "sScrollXInner": "100%",
            "bScrollCollapse": true,
            "bAutoWidth": true,
            "bSort": false,
            "bLengthChange": false,
            "oLanguage": { "sSearch": "" },
            "oTableTools": {
                "sRowSelect": "multi",
                "aButtons": ["select_all", "select_none"],
                "fnRowSelected": function() {
                    $removeStaffBtn.removeClass('disabled');
                },
                "fnRowDeselected": function() {
                    if (TableTools.fnGetInstance('staffAvailableDataTable').fnGetSelected().length == 0) {
                        $removeStaffBtn.addClass('disabled');
                    }
                }
            },
            "aoColumns": [
                {
                    "mData": "Name.FirstName",
                    "sClass": "FirstColumn",
                },
                {
                    "mData": "Name.LastName",
                    "sClass": "Sue",
                },
                { "mData": "Key", "bSortable": false, "bSearchable": true, "sClass": "hidden" }
            ],
            "fnDrawCallback": function(oSettings) {
                $(oSettings.aanFeatures.f[0]).find('input').attr('placeholder', "Search");
                updateTableSizes();
            }
        });

        var patientAvailableTable = $('#patientAvailableDataTable').dataTable({
            "sDom": "<'row-fluid'<'span6'T><'span6'f>r>t<'row-fluid'<'span5'i><'span7'p>>",
            "sPaginationType": "bootstrap",
            "sScrollY": "100%",
            "sScrollX": "100%",
            "sScrollXInner": "100%",
            "bScrollCollapse": true,
            "bAutoWidth": true,
            "bProcessing": true,
            "bServerSide": true,
            "bLengthChange": false,
            "bSort": false,
            "oLanguage": { "sSearch": "" },
            "oTableTools": {
                "sRowSelect": "multi",
                "aButtons": ["select_all", "select_none"],
                "fnRowSelected": function() {
                    $addPatientBtn.removeClass('disabled');
                },
                "fnRowDeselected": function() {
                    if (TableTools.fnGetInstance('patientAvailableDataTable').fnGetSelected().length == 0) {
                        $addPatientBtn.addClass('disabled');
                    }
                }
            },
            "sAjaxSource": patientDataTableUri,
            "aoColumns": [
                {
                    "mData": "Name.FirstName",
                    "sClass": "FirstColumn",
                },
                {
                    "mData": "Name.LastName",
                    "sClass": "Sue",
                },
                { "mData": "Key", "bSortable": false, "bSearchable": true, "sClass": "hidden" }
            ],
            "fnDrawCallback": function(oSettings) {
                $(oSettings.aanFeatures.f[0]).find('input').attr('placeholder', "Search");
                updateTableSizes();
            }
        });

        var patientCurrentTable = $('#patientCurrentDataTable').dataTable({
            "sDom": "<'row-fluid'<'span6'T><'span6'f>r>t<'row-fluid'<'span5'i><'span6'7>>",
            "sPaginationType": "bootstrap",
            "sScrollY": "100%",
            "sScrollX": "100%",
            "sScrollXInner": "100%",
            "bScrollCollapse": true,
            "bAutoWidth": true,
            "bSort": false,
            "bLengthChange": false,
            "oLanguage": { "sSearch": "" },
            "oTableTools": {
                "sRowSelect": "multi",
                "aButtons": ["select_all", "select_none"],
                "fnRowSelected": function() {
                    $removePatientBtn.removeClass('disabled');
                },
                "fnRowDeselected": function() {
                    if (TableTools.fnGetInstance('staffAvailableDataTable').fnGetSelected().length == 0) {
                        $removePatientBtn.addClass('disabled');
                    }
                }
            },
            "aoColumns": [
                {
                    "mData": "Name.FirstName",
                    "sClass": "FirstColumn",
                },
                {
                    "mData": "Name.LastName",
                    "sClass": "Sue",
                },
                { "mData": "Key", "bSortable": false, "bSearchable": true, "sClass": "hidden" }
            ],
            "fnDrawCallback": function(oSettings) {
                $(oSettings.aanFeatures.f[0]).find('input').attr('placeholder', "Search");
                updateTableSizes();
            }
        });
    };

    var showTeamGrid = function(widget) {
        $(widget).find('#team-editor').hide();
        $(widget).find('#team-editor').html('');
        $(widget).find('#team-table').show();
        teamDataTable.fnDraw();
    };

    var showTeamEditor = function(innerHtml) {
        var $widget = $('#team-widget');
        $widget.find('#team-editor').html(innerHtml);
        initializeTeamEditor();
        $('.dashboard-wrapper').dashboard('expand', $widget[0], showTeamGrid);
        $widget.find('#team-editor').show();
        $widget.find('.dataTable_wrapper').hide();
    };


    $("#create-team-btn").ajaxLink({
        getData: function() {
            var $name = $('#create-team-name');
            if (!$name.valid()) {
                return false;
            }
            return { name: $name.val() };
        },
        success: function(data) {
            showTeamEditor(data);
            $('#create-team-name').val('');
            $('#createTeamModal').modal("hide");
        }
    });

    var teamDataTable = $('#teamDataTable').dataTable({
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
        "sAjaxSource": teamDataTableUri,
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

    if (canAccessTeamEdit) {
        $("#teamDataTable tbody").click(function(event) {
            var key = teamDataTable.fnGetData(event.target.parentNode).Key;
            $.get(teamBaseUri + '/Edit/' + key, showTeamEditor);

        });
    }
}