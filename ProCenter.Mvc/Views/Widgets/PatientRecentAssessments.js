window.procenter.InitializePatientRecentAssessments = function(patientAssessmentsApiBase, patientKey, editAssessmentAction, canEditAssessment) {
    $('#patientAssessmentsDataTable').dataTable({
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
        "sAjaxSource": patientAssessmentsApiBase + '/Get/' + patientKey,
        "aoColumns": [
            {
                "mData": "CreatedTimeString",
                "sClass": "FirstColumn",
            },
            {
                "mData": "AssessmentName",
            },
            {
                "mData": "PercentComplete",
                "fnRender": function(oObj) {
                    return (oObj.aData.PercentComplete * 100).toFixed(0) + " %";
                }
            },
            {
                "mData": "AssessmentInstanceKey",
                "sClass": "LastColumn",
                "bSortable": false,
                "bSearchable": false,
                "fnRender": function (oObj) {
                    var description;
                    var text;
                    var icon;
                    if (oObj.aData.IsSubmitted || !canEditAssessment) {
                        text = "View";
                        icon = "&#xe07f;";
                    } else {
                        text = "Edit";
                        icon = "&#xe005;";
                    }
                    description = text + " " + "assessment " + oObj.aData.AssessmentName;
                    return "<div><a class='btn btn-mini btn-info' data-icon='" + icon +
                        "' href=" + editAssessmentAction + '/' + oObj.aData.AssessmentInstanceKey + "?patientKey=" + oObj.aData.PatientKey +
                        " aria-label='" + description + "' >" + text + "</a></div>";
                }
            }
        ],
        "fnDrawCallback": function() {
            $('#patientAssessmentsDataTable_wrapper .dataTables_scroll,#patientAssessmentsDataTable_wrapper .dataTables_scroll .dataTables_scrollBody').attr("data-layout-height", "*");
            fixSizes(true);
        }
    });
}