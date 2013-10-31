window.procenter.InitializePatientReports = function ( assessmentBase, assessmentApiBase, patientKey, getReportAction, customizeReportAction, canCustomize) {
    $('#patientReportsDataTable').dataTable({
        "sDom": "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "sScrollY": "100%",
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bScrollCollapse": true,
        "bAutoWidth": true,
        "bSort": false,
        "bProcessing": true,
        "bServerSide": true,
        "bJQueryUI": true,
        "sAjaxSource": assessmentApiBase + '/GetReportDataTable?patientKey=' + patientKey,
        "aoColumns": [
            {
                "mData": "CreatedTimeString",
                "sClass": "FirstColumn",
            },
            {
                "mData": "DisplayName",
            },
            {
                "mData": "ReportStatus",
                "fnRender": function (oObj) {
                    var className = 'status_' + oObj.aData.ReportSeverity;
                    return "<div class='" + className + "'>" + oObj.aData.ReportStatus + "</div>";
                }
            },
            {
                "mData": "Key",
                "sClass": "LastColumn",
                "bSortable": false,
                "bSearchable": false,
                "fnRender": function (oObj) {
                    var tools = "<a class='btn btn-mini btn-info' data-icon='&#xe025;' target='_blank' href=" + getReportAction + '/' + oObj.aData.SourceKey + "?reportName=" + oObj.aData.Name + ">View</a>";
                    if (oObj.aData.CanCustomize && canCustomize) {
                        tools = "<a class='customize-btn btn btn-mini btn-info' data-loadelementid='customizeReportModal .modal-loading-indicator' data-icon='&#xe005;' href=" + customizeReportAction + '/' + oObj.aData.SourceKey + "?reportName=" + oObj.aData.Name + ">Personalize</a>" + tools;
                    }
                    return "<div class='tools'>" + tools + "</div>";
                }
            }
        ],
        "fnDrawCallback": function () {
            $('#patientReportsDataTable_wrapper .dataTables_scroll,#patientReportsDataTable_wrapper .dataTables_scroll .dataTables_scrollBody').attr("data-layout-height", "*");
            fixSizes(true);
            $('#patientReportsDataTable .customize-btn').ajaxLink({
                type: "GET",
                getData: function () {
                    $('#customizeReportModal').modal('show');
                    return {};
                },
                success: function (data) {
                    $('#customizeReportModal .modal-body').html(data);
                    $('#customizeReportModal .modal-body .report-customizer').ajaxForm({
                        forceValueName: false,
                        success: function (data) {
                        }
                    });
                }
            });
        }
    });
    $('#customizeReportModal').on('hidden', function () {
        $(this).find('.modal-body').children().remove();
    });
}