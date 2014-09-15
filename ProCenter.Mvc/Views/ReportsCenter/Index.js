window.procenter.reportsCenter = {
    initializeReportCenter: function (reportBaseUrl, reportTemplateUrl, recentReportsUrl, missedRemindersUrl, reminderBaseUrl, systemAccountKey, organizationKey, noAssessmentWarning, heightLabels) {
        $('#back-to-reportscenter').hide();
        $('#report-builder').hide();
        $("#btn-group-savedreportstemplates").hide();

        $("#dashboard-filter").on("keyup search", function () {
            refreshData(this.value);
        });

        var getShowAll = function() {
            return ($("#reports-center-report-filter").val() == "AllReports");
        };

        var openReportBuilder = function() {
                $('.reports-center-dashboard').hide();
                $('#report-builder').show();
                $('#report-builder-link').hide();
                $("#dashboard-filter").hide();
                $('#back-to-reportscenter').show();

                window.procenter.InitializeReportBuilder(reportBaseUrl, systemAccountKey, null, noAssessmentWarning, heightLabels);

                $("#save-report").hide();
                $("#ReportToolbar_Menu").hide();
                $("#reportViewer1_ContentFrame").hide();
                $("#btn-group-savedreportstemplates").hide();
                $("#report-parameters").addClass("disabled");
                $("#report-saved, #report-templates").removeClass("disabled");
            

        };

        var addReportParameters = function(html, urlReportName, reportType) {
            openReportBuilder();

            var $parameters = $('.report-parameters');
            $parameters.html(html);
            $parameters.find('[data-control=simpleTabs]').simpleTabs();
            $parameters.find('[data-control=finder]').finder();
            $parameters.find('input[type=datetime]').datepicker({
                maxDate: '+0d',
                onSelect: function() {
                    $(this).trigger('blur');
                }
            });
            var reportName = $parameters.find('#ReportDisplayName').val();
            $('#ReportType').val(reportName);
            window.procenter.SetupReportBuilder();
            if (reportType == 'Template' || $("#TimePeriod").val() != null && $("#TimePeriod").val() != "") {
                $parameters.find('[data-control=simpleTabs]').simpleTabs('selectTab', $parameters.find("a[href='#time-period']")[0]);
                $("#save-template").show();
            }
        };

        var updateTableSizes = function () {
            $('.dataTable_wrapper .dataTables_scroll, .dataTable_wrapper .dataTables_scroll .dataTables_scrollBody').attr("data-layout-height", "*");
            fixSizes(true);
        };

        function refreshData(value) {
            var tables = $.fn.dataTable.fnTables(true);
            for (var i = 0; i < tables.length; i++) {
                $(tables[i]).dataTable().fnFilter(value);
            }
        }

        $('#dashboard-filter').clearable();

        $("#reports-center-report-filter").on('change', function() {
            refreshData('');
        });
        


        $('#report-builder-link').on('click', function () {
            openReportBuilder();
            $("#btn-group-savedreportstemplates").show();
            $("#reports-center-report-filter").hide();
            if (!$("#parametersExpander").find(".expand-content").is(".expanded")) {
                $("#parametersExpander").expander("toggleExpand");
            }
            if ($("#templateExpander").find(".expand-content").is(".expanded")) {
                $("#templateExpander").expander("toggleExpand");
            }
        });

        $('#back-to-reportscenter').on('click', function () {
            $('.reports-center-dashboard').show();
            $('#report-builder-link').show();
            $("#dashboard-filter").show();
            $('#report-builder').hide();
            $('#back-to-reportscenter').hide();
            $("#btn-group-savedreportstemplates").hide();
            $("#reports-center-report-filter").show();
        });

        $('#missedAppointmentDataTable').dataTable({
            "sDom": "<'row-fluid'<'span6'l>r>t<'row-fluid'<'span6'i><'span6'p>>",
            "sPaginationType": "bootstrap",
            "sScrollY": "1",
            "sScrollX": "100%",
            "sScrollXInner": "100%",
            "bScrollCollapse": true,
            "bAutoWidth": true,
            "bProcessing": true,
            "bServerSide": true,
            "bSort": false,
            "sAjaxSource": missedRemindersUrl,
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "systemAccountKey", "value": systemAccountKey });
                aoData.push({ "name": "organizationKey", "value": organizationKey });
                aoData.push({ "name": "showAll", "value": getShowAll() });
            },
            "aoColumns": [
                {
                    "mData": "PatientName",
                    "sClass": "PatientNameColumn",
                },
                {
                    "mData": "AssessmentName",
                    "sClass": "AssessmentNameColumn",
                },
                {
                    "mData": "Description",
                    "sClass": "DescriptionColumn",
                },
                {
                    "mData": "ReminderStartDateString",
                    "sClass": "ReminderStartDateStringColumn",
                },
                {
                    "mData": "Key",
                    "sClass": "RecurrenceKey",
                    "bSortable": false,
                    "bSearchable": false,
                    "fnRender": function (oObj) {
                        var icon = '&#xe07f;';
                        var text = "View";
                        var description = text + " missed reminder for " + oObj.aData.PatientName;
                        return "<div><a class='btn btn-mini btn-info view-reminder' aria-label='" + description + "' data-id='" + oObj.aData.RecurrenceKey + "' data-icon='" + icon +
                            "' href=" + reminderBaseUrl + '/Get/' + oObj.aData.RecurrenceKey + ">view</a></div>";
                    }
                }
            ],
            "fnDrawCallback": function () {
                updateTableSizes();
                $(".reports-center-dashboard").find('.view-reminder').ajaxLink({
                    type: 'GET',
                    success: function (data) {
                        if (data == null) return;
                        $('#editEventModal .modal-body').html('<form>' + data + '</form>');
                        $.validator.unobtrusive.parse('#editEventModal'); // Note: only work with new added forms
                        $('#assessmentReminder_Start').datepicker({ minDate: '-0d' });
                        $('#assessmentReminder_End').datepicker({ minDate: '-0d' });
                        $('#editEventModal [data-control=finder]').finder();
                        $('#editEventModal [data-control=simpleTabs]').simpleTabs();
                        var isRecurring = $('#editEventModal .recurring-reminder-wrapper').hasClass('is-recurring');
                        if (isRecurring) {
                            $('#editEventModal [data-control=simpleTabs]').simpleTabs('selectTab', $("a[href='#recurring-event']")[1]);
                        }
                        //if (patientKey) {
                        //    window.procenter.InitializeReminders.initPatientDropdown("editEventModal");
                        //    window.procenter.InitializeReminders.initFinderDropdown();
                        //}
                        //window.procenter.InitializeReminders.initReminder('#editEventModal');
                        $('#editEventModal').modal('show');
                    }
                });
            }
        });

        $('#reportTemplatesDataTable').dataTable({
            "sDom": "<'row-fluid'<'span6'l>r>t<'row-fluid'<'span6'i><'span6'p>>",
            "sPaginationType": "bootstrap",
            "sScrollY": "1",
            "sScrollX": "100%",
            "sScrollXInner": "100%",
            "bScrollCollapse": true,
            "bAutoWidth": true,
            "bProcessing": true,
            "bServerSide": true,
            "bSort": false,
            "sAjaxSource": reportTemplateUrl,
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "systemAccountKey", "value": systemAccountKey });
                aoData.push({ "name": "reportType", "value": "Template" });
                aoData.push({ "name": "organizationKey", "value": organizationKey });
                aoData.push({ "name": "showAll", "value": getShowAll() });
            },
            "aoColumns": [
                {
                    "mData": "NameParameters",
                    "sClass": "ReportNameColumn",
                },
                {
                    "mData": "Key",
                    "sClass": "ReportKeyColumn",
                    "bSortable": false,
                    "bSearchable": false,
                    "fnRender": function (oObj) {
                        var icon = '&#xe07f;';
                        var text = "View";
                        var description = text + " report for template " + oObj.aData.NameParameters;
                        return "<div><a class='btn btn-mini btn-info view-template-report' aria-label='" + description + "' data-id='" + oObj.aData.Key + "' data-icon='" + icon +
                            "' href=" + "/Report/Parameters/" + oObj.aData.Key + "?reportName=" + oObj.aData.ReportTypeName + ">view</a></div>";
                    }
                }
            ],
            "fnDrawCallback": function () {
                updateTableSizes();
                $(".reports-center-dashboard").find('.view-template-report').ajaxLink({
                    type: 'GET',
                    success: function (html) {
                        addReportParameters(html, this.url, "Template");
                    }
                });
            }
        });

        $('#savedReportsDataTable').dataTable({
            "sDom": "<'row-fluid'<'span6'l>r>t<'row-fluid'<'span6'i><'span6'p>>",
            "sPaginationType": "bootstrap",
            "sScrollY": "1",
            "sScrollX": "100%",
            "sScrollXInner": "100%",
            "bScrollCollapse": true,
            "bAutoWidth": true,
            "bProcessing": true,
            "bServerSide": true,
            "bSort": false,
            "sAjaxSource": reportTemplateUrl,
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "systemAccountKey", "value": systemAccountKey });
                aoData.push({ "name": "reportType", "value": "Saved" });
                aoData.push({ "name": "organizationKey", "value": organizationKey });
                aoData.push({ "name": "showAll", "value": getShowAll() });
            },
            "aoColumns": [
                {
                    "mData": "NameParameters",
                    "sClass": "ReportNameColumn",
                },
                {
                    "mData": "Key",
                    "sClass": "ReportKeyColumn",
                    "bSortable": false,
                    "bSearchable": false,
                    "fnRender": function (oObj) {
                        var icon = '&#xe07f;';
                        var text = "View";
                        var description = text + " saved report " + oObj.aData.NameParameters;
                        return "<div><a class='btn btn-mini btn-info view-saved-report' aria-label='" + description + "' data-id='" + oObj.aData.Key + "' data-icon='" + icon +
                            "' href=" + "/Report/Parameters/" + oObj.aData.Key + "?reportName=" + oObj.aData.ReportTypeName + ">view</a></div>";
                    }
                }
            ],
            "fnDrawCallback": function () {
                updateTableSizes();
                $(".reports-center-dashboard").find('.view-saved-report').ajaxLink({
                    type: 'GET',
                    success: function (html) {
                        addReportParameters(html, this.url, "Saved");
                    }
                });
            }
        });

        $('#recentReportsDataTable').dataTable({
            "sDom": "<'row-fluid'r>t<'row-fluid'<'span6'>>",
            "sPaginationType": "bootstrap",
            "sScrollY": "1",
            "sScrollX": "100%",
            "sScrollXInner": "100%",
            "bScrollCollapse": true,
            "bAutoWidth": true,
            "bProcessing": true,
            "bServerSide": true,
            "bSort": false,
            "sAjaxSource": recentReportsUrl + "?systemAccountKey=" + systemAccountKey,
            "aoColumns": [
                {
                    "mData": "Name",
                    "sClass": "ReportNameColumn",
                },
                {
                    "mData": "Assessment",
                    "sClass": "AssessmentNameColumn",
                },
                {
                    "mData": "RunDateString",
                    "sClass": "ReportRunDateColumn"
                },
                {
                    "mData": "ReportKey",
                    "sClass": "ReportKeyColumn",
                    "bSortable": false,
                    "bSearchable": false,
                    "fnRender": function (oObj) {
                        var icon = '&#xe07f;';
                        var text = "View";
                        var description = text + " recent report " + oObj.aData.Name + " for assessment " + oObj.aData.Assessment;
                        return "<div><a id='button-recent-reports' class='btn btn-mini btn-info view-report' aria-label='" + description + "' data-id='" + oObj.aData.ReportKey + "' data-icon='" + icon +
                            "' href=" + "/Report/Parameters/" + oObj.aData.ReportKey + "?reportName=" + oObj.aData.ReportTypeName + ">view</a></div>";
                    }
                }
            ],
            "fnDrawCallback": function () {
                updateTableSizes();
                $(".reports-center-dashboard").find('.view-report').ajaxLink({
                    type: 'GET',
                    success: function (html) {
                        addReportParameters(html, this.url);
                    }
                });
            }
        });
    }
}


