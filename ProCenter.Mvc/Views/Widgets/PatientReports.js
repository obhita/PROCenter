window.procenter.InitializePatientReports = function (assessmentBase, assessmentApiBase, patientKey, getReportAction, customizeReportAction, canCustomize) {

    var rowFormat = "<div class=\"row-fluid {0}\"><span class=\"span2 fs1\" data-icon=\"{1}\"></span><span class=\"span4\">{2}</span><span class=\"span2 {3}\">{4}</span><span class=\"span2\">{5}</span><span class=\"span3 last-column\">{6}</span><div class=\"clear\"></div></div>";

    var getToolsHtml = function (rowCanCustomize, key, name, type) {
        var tools = "<a class='btn btn-mini btn-info view-report' data-icon='&#xe025;' target='_blank' href=" + getReportAction + '/' + key + "?reportName=" + name + ">View</a>";
        if (type == '1') {
            tools = "<a class='btn btn-mini btn-info view-report-builder' data-icon='&#xe025;' href='/Report/Parameters/" + key + "?patientKey=" + patientKey + "&reportName=" + name + "'>View</a>";
        }
        if (rowCanCustomize && canCustomize) {
            tools = "<a class='customize-btn btn btn-mini btn-info' data-loadelementid='customizeReportModal .modal-loading-indicator' data-icon='&#xe005;' href=" + customizeReportAction + '/' + key + "?reportName=" + name + ">Personalize</a>" + tools;
        }
        return "<div class='tools'>" + tools + "</div>";
    };

    var $patientReportsWidget = $('.patient-reports');

    var setupReportPersonalization = function () {
        $('.assessment-reports .customize-btn').ajaxLink({
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
    };

    var patientReportsDataTable = $('#patientReportsDataTable').dataTable({
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
                "mData": "ReportType",
                "sClass": "FirstColumn",
                "fnRender": function (oObj) {
                    var icon = "&#xe1b7;";
                    if (oObj.aData.ReportType == 1) {
                        icon = "&#xe097;";
                    }
                    return "<span class='fs1' data-icon='" + icon + "'></span>";
                }
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
                "mData": "CreatedTimeString"
            },
            {
                "mData": "Key",
                "sClass": "LastColumn",
                "bSortable": false,
                "bSearchable": false,
                "fnRender": function (oObj) {
                    return getToolsHtml(oObj.aData.CanCustomize, oObj.aData.SourceKey, oObj.aData.Name);
                }
            }
        ],
        "fnDrawCallback": function () {
            $('#patientReportsDataTable_wrapper .dataTables_scroll,#patientReportsDataTable_wrapper .dataTables_scroll .dataTables_scrollBody').attr("data-layout-height", "*");
            fixSizes(true);
            setupReportPersonalization();
        }
    });
    
    $('#customizeReportModal').on('hidden', function () {
        $(this).find('.modal-body').children().remove();
    });

    var addRows = function($container, data) {
        var html = "";
        var rowClass = "odd";
        if (!data || !data.length || data.length === 0) {
            html = '<span class="message">No data...</span>';
        } else {
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var icon = "&#xe1b7;";
                if (item.ReportType == 1) {
                    icon = "&#xe097;";
                }
                var severityClassName = 'status_' + item.ReportSeverity;
                html += window.procenter.stringFormat(rowFormat, rowClass, icon, item.DisplayName, severityClassName, item.ReportStatus, item.CreatedTimeString, getToolsHtml(item.CanCustomize, item.SourceKey, item.Name, item.ReportType));
                
                if (rowClass == "odd") {
                    rowClass = "even";
                } else {
                    rowClass = "odd";
                }
            }
        }
        $container.html(html);
    };

    var getAssessmentReports = function() {
        var container = $patientReportsWidget.find('.assessment-reports');
        container.html('<span class="message">Loading...</span>');
        $.getJSON(assessmentApiBase + '/GetReportsByType?patientKey=' + patientKey,
            {
                type: 0,
                rowCount: 3
            }, function(data) {
                addRows(container, data);
                setupReportPersonalization();
            })
            .error(function () {
                container.html('<span class="message">Error...</span>');
            });
    };

    var getSavedReports = function () {
        var container = $patientReportsWidget.find('.saved-reports');
        container.html('<span class="message">Loading...</span>');
        $.getJSON(assessmentApiBase + '/GetReportsByType?patientKey=' + patientKey,
            {
                type: 1,
                rowCount: 3
            }, function (data) {
                addRows(container, data);

                container.find('.view-report-builder').ajaxLink({
                    type: 'GET',
                    success: function (html) {
                        $patientReportsWidget.closest('.dashboard').dashboard("expand", $patientReportsWidget);
                        loadExpanded();
                        $patientReportsWidget.find('.simpleTabs-container').simpleTabs('selectTab', $patientReportsWidget.find(".simpleTabs-container a[href='#report-builder']")[0]);
                        $patientReportsWidget.find('.report-builder .parameters-expander').expander("collapse");

                        //todo: better way to set reportName, or maybe re-design the model to include report name
                        var url = this.url;
                        var placeHolder = "reportName";
                        var reportName = url.substr(url.indexOf(placeHolder) + placeHolder.length + 1);
                        $('#ReportType').val(reportName);
                        var $parameters = $('.report-parameters');
                        $parameters.html(html);
                        $parameters.find('[data-control=simpleTabs]').simpleTabs();
                        $parameters.find('[data-control=finder]').finder();
                        $parameters.find('input[type=datetime]').datepicker({
                            maxDate: '+0d',
                            onSelect: function () {
                                $(this).trigger('blur');
                            }
                        });
                        window.procenter.SetupReportBuilder();
                    },
                });
            })
            .error(function () {
                container.html('<span class="message">Error...</span>');
            });
    };

    var loadCollapsed = function () {
        $patientReportsWidget.find('.expanded-content').hide();
        $patientReportsWidget.find('.collapsed-content').fadeIn();
        getAssessmentReports();
        getSavedReports();
    };

    var loadExpanded = function () {
        $patientReportsWidget.find('.collapsed-content').hide();
        $patientReportsWidget.find('.expanded-content').fadeIn();
        patientReportsDataTable.fnDraw();
    };

    function showReportArea(button, area) {
        $("#content-parameters").hide();
        $("#report-parameters-div").hide();
        $("#content-templates").hide();
        $("#content-saved-reports").hide();
        if (button.hasClass("disabled")) {
            $(button).removeClass("disabled");
        } else {
            $("#report-parameters, #report-saved, #report-templates").removeClass("disabled");
            area.show();
            $(button).addClass("disabled");
        }
    }

    $('.btn').button();
    $("#content-templates").hide();
    $("#content-saved-reports").hide();
    $("#content-parameters").show();
    $("#report-parameters").addClass("disabled");

    $("#report-parameters").on("click", function () {
        showReportArea($(this), $("#content-parameters"));
    });
    $("#report-saved").on("click", function () {
        showReportArea($(this), $("#content-saved-reports"));
    });
    $("#report-templates").on("click", function () {
        showReportArea($(this), $("#content-templates"));
    });

    loadCollapsed();

    $patientReportsWidget.on('expanded', loadExpanded);
    $patientReportsWidget.on('collapsed', loadCollapsed);

    $patientReportsWidget.find('.expanded-content').on('tabChanged', window.fixSizes);
}