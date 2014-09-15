window.procenter.InitializeReportBuilder = function (reportBase, systemAccountKey, portalPatientKey, noAssessmentWarning, heightLabels) {
    // patientKey will not be null only when the report builder is loaded from the patient tab
    var patientKey = $('.patient-tab > a.selected').parent().data('key');
    var patientTab = $('.patient-tab > a.selected').parent();
    var assessmentDefKey, assessmentCode, assessmentName, selectedReportName;
    var assessmentAnswersDictionary = {};
    var patientsWithSpecificResponseReport = "PatientsWithSpecificResponse";

    if (portalPatientKey != null) {
        patientKey = portalPatientKey;
    }

    function initReportTypeFinder() {
        if (patientTab.length > 0 || portalPatientKey != null) {
            $("#ReportType").attr("data-finder-url", "/api/Report/ReportTypeSearch?isPatientCentric=true");
        } else {
            $("#ReportType").attr("data-finder-url", "/api/Report/ReportTypeSearch?isPatientCentric=false");
        }
        $("#ReportType").attr("data-control", "finder");
        $("#ReportType").finder();
    }

    $('#ReportType').on('selectionChanged', function (event, selected) {
        selectedReportName = getReportName(selected);
        var data = {
            reportName: selectedReportName
        };
        if (patientTab.length > 0 || portalPatientKey != null) {
            data["patientKey"] = patientKey;
        }
        $.get(reportBase + '/Parameters', data, function (html) {
            // reset the dictionary and the selected assessment
            assessmentAnswersDictionary = {};
            assessmentDefKey = null;

            var $parameters = $('.report-parameters').html(html);
            $parameters.find('[data-control=simpleTabs]').simpleTabs();
            // if reportbuilder is loaded from the patient tab, then the patient dropdown can be hid
            if (patientKey) {
                var patientKeyContainer = $parameters.find('#patientkey-container').first();
                var patientKeyControl = patientKeyContainer.find('#patientkey-finder').first();
                patientKeyContainer.html('<input type="hidden" value="' + patientKey + '"' + ' name="' + patientKeyControl.attr("name") + '" />');
            }

            $parameters.find('[data-control=finder]').finder();
            $parameters.find('input[type=datetime]').datepicker({
                maxDate: '+0d',
                onSelect: function () {
                    $(this).trigger('blur');
                }
            });
            $('.question-lookup').hide();
            $('.question-lookup-no-results-found').hide();

            generateReportAjax();
            saveTemplateAjax();
            initAssessmentSelectionChange();
        });

    });

    getSavedTemplates('Template', 'template-list');
    getSavedTemplates('Saved', 'saved-list');
    initReportTypeFinder();

    function initAssessmentSelectionChange() {
        $('#assessmentdefinitioncode-finder').on('selectionChanged', function (event, selected) {
            if (selected) {
                var data = {
                    scoreType: selected.ScoreType
                };
                $.get(reportBase + '/GetReportParametersForAssessment', data, function (html) {
                    $('#scoretype-container').html(html);
                });
                assessmentDefKey = selected.Key;
                assessmentCode = selected.AssessmentCode;
                assessmentName = selected.AssessmentName;
            } else {
                $('#scoretype-container').html("");
                $("#Parameters_AssessmentName").val(null);
            }
        });
    }

    function getReportName(selected) {
        var reportName;
        if (selected) {
            return selected.ReportName;
        }
        if ($('#ReportType').data('finder').selectedData !== undefined && $('#ReportType').data('finder').selectedData != null) {
            reportName = $('#ReportType').data('finder').selectedData.ReportName;
        } else {
            // get from the hidden value
            reportName = $("#ReportName").val();
        }
        return reportName;
    }

    function getParameters() {
        var data = {};
        $('#generate-report').closest('.report-parameters').find(':input').each(function () {
            var $self = $(this);
            if ($self.data("control") == "finder") {
                var finder = $self.data('finder');
                if (this.id == "assessmentdefinitioncode-finder") {
                    if (finder.selectedData) {
                        data[this.name] = finder.selectedData.AssessmentCode;
                        $self.parent().next().val(finder.selectedData.AssessmentName);
                        assessmentDefKey = finder.selectedData.Key;
                    } else {
                        data[this.name] = null;
                    }
                }
                if (this.id == "patientkey-finder") {
                    if (finder.selectedData) {
                        data[this.name] = finder.selectedData.Key;
                        $self.parent().next().val(finder.selectedData.Name.FullName);
                    }
                    else {
                        data[this.name] = null;
                    }
                }
            } else if (!data.hasOwnProperty(this.name)) {
                if (this.type == "checkbox") {
                    data[this.name] = this.checked;
                } else {
                    data[this.name] = this.value;
                }
            }
        });

        if (Object.keys(assessmentAnswersDictionary).length > 0) {
            var answersArray = [];
            for (var assessmentCodeKey in assessmentAnswersDictionary) {
                for (var questionCode in assessmentAnswersDictionary[assessmentCodeKey].QuestionAnswers) {
                    if (assessmentAnswersDictionary[assessmentCodeKey].QuestionAnswers[questionCode].IsAnswered) {
                        answersArray.push(assessmentAnswersDictionary[assessmentCodeKey].QuestionAnswers[questionCode]);
                    }
                }
            }
            data["Parameters.JsonResponse"] = JSON.stringify(answersArray);
        }

        return data;
    }

    function generateReport() {
        var data = getParameters();
        var reportName = getReportName();
        var action = data["Parameters.ControllerAction"];
        var callbackUrl = '/Report/' + action + '?reportName=' + reportName + '&' + $.param(data);
        window.reportViewer1.callbackUrl = callbackUrl;
        data["reportName"] = reportName;
        window.reportViewer1.formHelper.customArgs = data;
        window.reportViewer1.EndCallback.ClearHandlers();
        showReportViewer(false);
        window.reportViewer1.EndCallback.AddHandler(function (s, e) {
            showReportViewer(window.reportViewer1.pageCount > 0);
            $("#content-templates").hide();
            $("#content-saved-reports").hide();
            $("#content-parameters").hide();
            $("#report-templates, #report-saved, #report-parameters").removeClass("disabled");
        });
        window.reportViewer1.Refresh();
    }

    function showReportViewer(show) {
        if (!show) {
            $("#save-report").hide();
            $("#ReportToolbar_Menu").hide();
            $(".report-content").hide();
            $("#reportViewer1_ContentFrame").hide();
            $("#no-report-data").show();
        } else {
            $("#save-report").show();
            $("#ReportToolbar_Menu").show();
            $(".report-content").show();
            $("#reportViewer1_ContentFrame").show();
            $("#no-report-data").hide();
        }
    }

    function generateReportAjax() {
        $('#generate-report').click(function (evt) {
            evt.stopImmediatePropagation();
            evt.preventDefault();
            $("#report-parameters, #report-templates, #report-saved").removeClass("disabled");
            generateReport();
        });
    }

    function saveTemplateAjax() {
        $('#save-template').ajaxLink({
            getData: function () {
                var data = getParameters();
                data["reportName"] = getReportName();
                return data;
            },
            success: function (data) {
                if (data != null && data.error != null) {
                    $('#messageModal .modal-body').text(data.error);
                    $("#messageModal").modal("show");
                } else {
                    getSavedTemplates('Template', 'template-list');
                }
            }
        });
    }

    window.procenter.reportToolbarItemClick = function (s, e) {
        if (e.item.name == "SaveReport") {
            var tempData = getParameters();
            tempData["reportName"] = getReportName();
            $.ajax({
                type: "POST",
                url: "/Report/SaveReportParameters" + getReportName(),
                data: tempData,
                traditional: true
            }).done(function (data) {
                if (data != null && data.error != null) {
                    $('#messageModal .modal-body').text(data.error);
                    $("#messageModal").modal("show");
                } else {
                    getSavedTemplates('Saved', 'saved-list');
                }
            }).fail(function (err) {
                $('#messageModal .modal-body').text(err);
                $("#messageModal").modal("show");
            });
        }
    }

    window.procenter.SetupReportBuilder = function () {
        generateReportAjax();
        saveTemplateAjax();
        generateReport();
    };

    function getSavedTemplates(reportType, selectorClassName) {
        $.get('/api/report/reportTemplateSearch/?page=0&pageSize=100&reportType=' + reportType + (systemAccountKey ? '&systemAccountKey=' + systemAccountKey : '')
            + (patientKey ? '&patientKey=' + patientKey : ''), null, function (jsonData) {
                var lis = '';
                for (var i = 0; i < jsonData.Data.length; i++) {
                    var dto = jsonData.Data[i];
                    lis += '<li><a href="' + reportBase + 'Parameters/' + dto.Key + '?reportName=' + dto.Name + (patientKey ? '&patientKey=' + patientKey : '') + '">' + dto.ReportDisplayName + ' ' + dto.Parameters + '</a></li>';
                }
                var $ul = $('.' + selectorClassName);
                $ul.html('<ul class="' + selectorClassName + '">' + lis + '</ul>');
                $ul.find('li>a').each(function () {
                    var $this = $(this);
                    $this.ajaxLink({
                        type: 'GET',
                        success: function (html) {
                            var $parameters = $('.report-parameters');
                            $parameters.html(html);
                            var reportName = $parameters.find('#ReportDisplayName').val();
                            $('#ReportType').val(reportName);
                            $parameters.find('[data-control=simpleTabs]').simpleTabs();
                            if (reportType == 'Template') {
                                $parameters.find('[data-control=simpleTabs]').simpleTabs('selectTab', $parameters.find("a[href='#time-period']")[0]);
                                $("#save-template").show();
                            }
                            $parameters.find('[data-control=finder]').finder();
                            $parameters.find('input[type=datetime]').datepicker({
                                maxDate: '+0d',
                                onSelect: function () {
                                    $(this).trigger('blur');
                                }
                            });
                            $parameters.closest('.parameters-expander').expander("collapse");
                            window.procenter.SetupReportBuilder();
                        },
                    });
                });
            });
    }

    window.procenter.AssessmentViewModel = function () {
        var self = this;
        self.Items = ko.observableArray([]);
        self.OnTextChange = function () {
            var searchValue = $('#question-lookup-text').val();
            if (searchValue.length > 2) {
                var parameters = {
                    page: 0,
                    pageSize: 20,
                    assessmentDefinitionKey: assessmentDefKey,
                    search: searchValue,
                };
                if (assessmentDefKey == undefined || assessmentDefKey == null) {
                    $("#messageModal").on("hidden", function () {
                        $("#assessmentdefinitioncode-finder").focus();
                        $("#assessmentdefinitioncode-finder").trigger("click");
                    });
                    $("#question-lookup-text").val("");
                    $('#messageModal .modal-body').text(noAssessmentWarning);
                    $("#messageModal").modal("show");
                    return;
                }
                $.get('/api/Assessment/GetLookupSearchActive', parameters)
                    .done(function (results) {
                        self.Items(getAssessmentItems(results.Data));
                        if (results.TotalCount == 0) {
                            $('.question-lookup').hide();
                            $('.question-lookup-no-results-found').show();
                        } else {

                            $('.question-lookup').show();
                            $('.question-lookup-no-results-found').hide();
                        }
                    });
            }
        };
    };

    window.procenter.InitQuestionsDictionary = function (questions, questionsHtmlDict) {
        assessmentAnswersDictionary = {};
        questions.forEach(function (question) {
            if (!assessmentAnswersDictionary[question.AssessmentCode]) {
                assessmentAnswersDictionary[question.AssessmentCode] = { QuestionAnswers: {} };
            }

            var newQuestion = {
                ItemDefinitionCode: question.ItemDefinitionCode,
                Responses: question.Responses,
                InputType: question.InputType,
                AssessmentCode: question.AssessmentCode,
                AssessmentDefinitionKey: question.AssessmentDefinitionKey,
                IsAnswered: true,
                ParentName: question.ParentName,
                IsLookup: question.InputType == "MultipleSelect",
                Code: question.ItemDefinitionCode,
                TemplateName: question.InputType,
                AssessmentName: question.AssessmentName
            };

            // initialize the question dictionary
            assessmentAnswersDictionary[question.AssessmentCode].QuestionAnswers[question.ItemDefinitionCode] = newQuestion;
            renderQuestion(questionsHtmlDict[question.ItemDefinitionCode], newQuestion);
        });

        initAssessmentSelectionChange();
    };

    function getAssessmentItems(assessmentItems) {
        var items = [];
        assessmentItems.forEach(function (item) {
            items.push(new assessmentItem(item));
        });

        return items;
    }

    function assessmentItem(item) {
        var self = this;
        self.Code = item.Code;
        self.Name = item.Name;
        self.Level = item.Level;
        self.ItemType = item.ItemType;
        self.TemplateName = item.TemplateName;
        self.AssessmentDefinitionKey = assessmentDefKey;
        self.AssessmentCode = item.AssessmentCode;
        self.AssessmentName = item.AssessmentName;
        self.Items = ko.observableArray([]);
        if (item.Items) {
            self.Items(getAssessmentItems(item.Items));
        }
        self.AddQuestion = function (question) {
            $('.question-lookup').hide();
            var parameters = {
                assessmentDefinitionKey: assessmentDefKey,
                itemDefinitionCode: question.Code,
                parentName: item.ParentName
            };
            $.get('/Assessment/GetQuestionDefinition', parameters)
                .done(function (results) {
                    if (!assessmentAnswersDictionary[question.AssessmentCode]) {
                        assessmentAnswersDictionary[question.AssessmentCode] = { QuestionAnswers: {} };
                    }

                    if (assessmentAnswersDictionary[question.AssessmentCode].QuestionAnswers[question.Code]) {
                        $('#messageModal .modal-body').text("You have already added this question");
                        $("#messageModal").modal("show");
                        return;
                    }
                    // initialize the question dictionary
                    assessmentAnswersDictionary[question.AssessmentCode].QuestionAnswers[question.Code] = {
                        ItemDefinitionCode: question.Code,
                        Responses: [],
                        InputType: question.TemplateName,
                        AssessmentCode: question.AssessmentCode,
                        AssessmentDefinitionKey: question.AssessmentDefinitionKey,
                        IsAnswered: false,
                        ParentName: item.ParentName,
                        IsLookup: question.TemplateName == "MultipleSelect",
                        AssessmentName: question.AssessmentName
                    };
                    
                    renderQuestion(results, question);
                    
                    $("#question-lookup-text").val("");
                    $("#question-lookup-text").focus();
                });
        };
    }
    
    function renderQuestion(questionHtmlString, question) {
        var questionContainerId = 'div_' + question.AssessmentCode;
        var assessmentQuestionHtml = $('#' + questionContainerId);
        // questionHtml declared to initialize events only to the newly added question
        var questionHtml;
        if (assessmentQuestionHtml.length == 0) {
            var assessmentInfo = "<div class='assessmentInfoContainer'><label>ASSESSMENT: </label> " + question.AssessmentName + "</div><div class='assessmentClose'>" +
                "<a href='#' class='btn-remove-assessment-questions-group' data-icon='&#xe0fa;' data-remove-assessment-questions='true' title='Remove Assessment Questions'></a>" +
                "</div><div style='clear: both;'></div>";
            var questionAssessmentInfo = $('<div/>').html(assessmentInfo);
            // initialize click event for the assessment close button
            initEvents(questionAssessmentInfo, question);
            assessmentQuestionHtml = $('<div id="' + questionContainerId + '" class="assessmentQuestionContainer" />').html(questionAssessmentInfo);
            questionHtml = $('<div/>').html(questionHtmlString);
            assessmentQuestionHtml.append(questionHtml);
            $('#questionsGrid').append(assessmentQuestionHtml);
        } else {
            questionHtml = $('<div/>').html(questionHtmlString);
            assessmentQuestionHtml.append(questionHtml);
        }
        // initialize the events like click, focusout etc that gets triggered when questions are answered
        initEvents(questionHtml, question);
        
        if (getReportName() == patientsWithSpecificResponseReport) {
            $('#assessmentdefinitioncode-finder').attr('disabled', 'disabled');
            $("#assessmentdefinitioncode-finder").next().attr('disabled', 'disabled');
        }
        
    }

    function initEvents(questionHtml, question) {
        questionHtml.find("select[data-role=multiselect]").multiselect();
        questionHtml.find(".heightTotalInches").renderHeight(heightLabels);
        questionHtml.on('change', 'select', function () {
            setValue(this, question);
        });
        questionHtml.on('click', ':radio,:checkbox', function () {
            setValue(this, question);
        });
        questionHtml.on('focusout', 'input:text', function () {
            setValue(this, question);
        });
        questionHtml.on('click', "a[data-remove-question]", function () {
            if (assessmentAnswersDictionary[question.AssessmentCode]) {
                delete assessmentAnswersDictionary[question.AssessmentCode].QuestionAnswers[question.Code];
            }
            $(this).closest(".question-root").remove();
            // if all the questions in the assessment group are removed, remove the group
            if (Object.keys(assessmentAnswersDictionary[question.AssessmentCode].QuestionAnswers).length == 0) {
                if (selectedReportName == patientsWithSpecificResponseReport) {
                    $('#assessmentdefinitioncode-finder').removeAttr('disabled');
                    $("#assessmentdefinitioncode-finder").next().removeAttr('disabled');
                }
                $("#div_" + question.AssessmentCode).remove();
            }
        });
        questionHtml.on('click', "a[data-remove-assessment-questions]", function () {
            // delete assessment group
            delete assessmentAnswersDictionary[question.AssessmentCode];
            if (selectedReportName == patientsWithSpecificResponseReport) {
                $('#assessmentdefinitioncode-finder').removeAttr('disabled');
                $("#assessmentdefinitioncode-finder").next().removeAttr('disabled');
            }
            $("#div_" + question.AssessmentCode).remove();
        });
    }

    function setValue(control, question) {
        var controlValues = [];
        var isValueChanged = false;
        switch (question.TemplateName) {
            case "MultipleSelect":
                controlValues = $(control).val();
                isValueChanged = true;
                break;
            case "IntRange":
                $(control.parentElement).find('input[data-name-intrange^="intRange"]').each(function () { controlValues.push(this.value); });
                isValueChanged = true;
                break;
            case "Height":
                // find controls with heightTotalInches class in the control's classes
                if ($.inArray("heightTotalInches", control.classList) >= 0) {
                    controlValues.push(control.value);
                    isValueChanged = true;
                }
                break;
            default:
                controlValues.push(control.value);
                isValueChanged = true;
                break;
        }

        if (isValueChanged) {
            assessmentAnswersDictionary[question.AssessmentCode].QuestionAnswers[question.Code].IsAnswered = true;
            assessmentAnswersDictionary[question.AssessmentCode].QuestionAnswers[question.Code].Responses = controlValues;
        }
    }
}