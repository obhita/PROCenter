window.procenter.InitializeAssessment = function (organizationBaseUri, organizationDataTableUri, updateTableSizes, checkDisabled, canAccessOrganizationEdit) {
    var assessmentSearchDataTable = $('#assessmentSearchDataTable').dataTable({
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
        "sAjaxSource": organizationDataTableUri,
        "aoColumns": [
            {
                "mData": "AssessmentName",
                "sClass": "FirstColumn",
            },
            {
                "mData": "Key",
                "bSearchable": false,
                "bSortable": false,
                "fnRender": function(oObj) {
                    if (canAccessOrganizationEdit) {
                        var description = "Remove assessment " + oObj.aData.AssessmentName + " from active assessments";
                        return '<a href="' + organizationBaseUri + '/DeactivateAssessment/' + oObj.aData.Key + '" aria-label="' + description + '" class="btn btn-mini btn-info" data-icon="&#xe0fa;">Remove</a>';
                    } else {
                        return '';
                    }
                }
            }
        ],
        "fnDrawCallback": function() {
            $('#assessmentSearchDataTable td a.btn.btn-mini').each(function() {
                $(this).ajaxLink({
                    getData: function() {
                        var iRow = assessmentSearchDataTable.fnGetPosition($(this).closest('tr')[0]);
                        return { iRow: iRow };
                    },
                    success: function(data) {
                        assessmentSearchDataTable.fnDeleteRow(data.iRow, null, true);
                    }
                });
            });
            updateTableSizes();
        }
    });

    $('#assessment-widget .assessment-definition').on('selectionChanged', function(evt, data) {
        if (data && $('#' + data.Key).length === 0) {
            $('#assessment-widget .assessment-definition a.add-btn').removeAttr("disabled").attr("href", organizationBaseUri + '/ActivateAssessment/' + data.Key);
        } else {
            $('#assessment-widget .assessment-definition a.add-btn').attr("disabled", "disabled");
        }
    });

    $('#assessment-widget .assessment-definition a.add-btn').ajaxLink({
        getData: function () {
            return true;
        },
        success: function() {
            var finder = $('#assessment-widget .assessment-definition [data-control="finder"]').finder();
            var assessment = finder.selectedData;
            assessmentSearchDataTable.fnAddData(assessment, true);
            finder.clearSelected();
        }
    });
}