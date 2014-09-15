window.procenter.InitializeReminders = function (remindersBase, remindersApiBase, canEditReminders, patientKey, patientFullName, currentDate) {
    var search = undefined;
    var source = remindersApiBase + '/Get/';
    if (patientKey) {
        source = source + '?patientKey=' + patientKey;
    }

    $("#dashboard-filter").on("keyup search", function () {
        $('#calendar').fullCalendar('removeEventSource', source);
        search = this.value;
        source = remindersApiBase + '/Get/';
        if (search && search !== '') {
            source = source + '?sSearch=' + search;
        }
        if (patientKey) {
            source = source + '&patientKey=' + patientKey;
        }
        $('#calendar').fullCalendar('addEventSource', source);
    });

    $("#add-reminder").click(function() {
        initPatientDropdown("createEventModal");
        initFinderDropdown();
    });

    function initPatientDropdown(controlId) {
        if (patientKey) {
            $('#' + controlId + ' #reminder-patient-key [data-control=finder]').finder('select', { "Name": { "FullName": patientFullName }, "Key": patientKey });
            $('#' + controlId + ' #reminder-patient-key :input').attr("disabled", true);
            $('#' + controlId + ' #reminder-patient-key a').attr("disabled", true).off("click");
        }
    }

    function initFinderDropdown() {
        if (!canEditReminders) {
            $('.finder-all-button').attr("disabled", true).off("click");
        }
    }

    function initReminder(selector) {
        $(selector).find('[data-disabled=true] :input').attr('disabled', 'disabled');
        $(selector).find('.set-reminder-button').click(function () {
            $(this).hide();
            $(selector).find('.set-reminder-wrapper').removeClass("hidden").show();
        });

        initRecurrence();
        $.validator.addMethod("validEndDate", function (value) {
            return isValidDate(value);
        }, "The End Date field must be a valid date.");
        $.validator.addMethod("isNotBeforeAssessmentStartDate", function(value) {
            return isNotBeforeAssessmentStartDate(value);
        }, "The End Date field must be on or after Assessment Date.");
        $.validator.addMethod("validAssessmentDate", function (value) {
            return isValidDate(value);
        }, "The Assessment Date field must be a valid date.");
        $('#assessmentReminderDto_End').val('');

        $("a[href='#one-time-event']").on("click", function () {
            initRecurrence();
            $("#assessmentReminder_End").val('');
            $("input:radio[value=Daily],input:radio[value=Weekly],input:radio[value=Monthly]").removeAttr("checked");
        });
        $(".set-reminder-button").on("click", function() {
            $("#assessmentReminderDto_ReminderTime").addClass("input-validation-error");
            $("span[assessmentReminderDto_ReminderTime]").html("The Reminder field is required.");
        });

        $("#assessmentReminderDto_Start").rules('add', { validAssessmentDate: true });
        $('#assessmentReminderDto_End').rules('add', { isNotBeforeAssessmentStartDate: true });

        $("input:radio[name='assessmentReminderDto.ReminderRecurrence']").on("click", function () {
            $('#assessmentReminderDto_End').addClass("input-validation-error");
            $('#assessmentReminderDto_End').rules('add', { validEndDate: true });
            $("#assessmentReminderDto_ReminderTime").removeClass("input-validation-error");
            $("#assessmentReminderDto_ReminderTime").html("");
        });

        $("#assessmentReminder_Start, #assessmentReminderDto_Start").on("change", function () {
            disableReminderFields(isReminderSelectedDateToday());
        });
        disableReminderFields(isReminderSelectedDateToday());
    }

    function isReminderSelectedDateToday() {
        var today = $.datepicker.formatDate('mm/dd/yy', new Date());
        return Date.parse($("#assessmentReminder_Start").val()) == Date.parse(today) || Date.parse($("#assessmentReminderDto_Start").val()) == Date.parse(today);
    }

    function disableReminderFields(disable) {
        if (disable) {
            var disabled = 'disabled';
            $("#assessmentReminder_ReminderTime").attr('disabled', disabled);
            $("#assessmentReminder_ReminderTime").val(0);
            $("#assessmentReminder_ReminderUnit").attr('disabled', disabled);
            $("#assessmentReminder_SendToEmail").attr('disabled', disabled);
            $("#assessmentReminderDto_ReminderTime").attr('disabled', disabled);
            $("#assessmentReminderDto_ReminderTime").val(0);
            $("#assessmentReminderDto_ReminderUnit").attr('disabled', disabled);
            $("#assessmentReminderDto_SendToEmail").attr('disabled', disabled);
        } else {
            $("#assessmentReminder_ReminderTime").removeAttr('disabled');
            $("#assessmentReminder_ReminderUnit").removeAttr('disabled');
            $("#assessmentReminder_SendToEmail").removeAttr('disabled');
            $("#assessmentReminderDto_ReminderTime").removeAttr('disabled');
            $("#assessmentReminderDto_ReminderUnit").removeAttr('disabled');
            $("#assessmentReminderDto_SendToEmail").removeAttr('disabled');
        }
    }

    function isNotBeforeAssessmentStartDate(dtValue) {
        if (dtValue !== undefined) {
            var endDate = Date.parse(dtValue);
            if (endDate < Date.parse($("#assessmentReminderDto_Start").val())) {
                return false;
            }
            return true;
        }
        return true;
    }

    function isValidDate(dtValue) {
        var bits = dtValue.split('/');
        var d = new Date(bits[2], bits[0] - 1, bits[1]);
        var isValid = (d.getMonth()) == bits[0] - 1 && d.getDate() == Number(bits[1]);
        return isValid;
    }

    function initRecurrence() {
        $("#assessmentReminderDto_End").val('');
        $("input:radio[name='assessmentReminderDto.ReminderRecurrence']").attr("checked", false);
        $("#assessmentReminderDto_End").removeClass("input-validation-error");
        $("#assessmentReminderDto_End").rules('remove', 'validEndDate');
        $("#assessmentReminderDto_ReminderTime").removeClass("input-validation-error");
        $("#assessmentReminderDto_ReminderTime").html("");
        $("#assessmentReminderDto_End").html("");
        $("#ReminderRecurrence_Daily").val("Daily");
        $("#ReminderRecurrence_Weekly").val("Weekly");
        $("#ReminderRecurrence_Monthly").val("Monthly");
        $("#ReminderRecurrence_Daily").css("padding-right", "10px");
        $("#ReminderRecurrence_Weekly").css("padding-right", "10px");
        $("#ReminderRecurrence_Monthly").css("padding-right", "10px");
        $("#assessmentReminderDto_ReminderTime").val(0);
    }

    //calendar
    var calendar = $('#calendar').fullCalendar({
        theme: true,
        selectable: true,
        selectHelper: true,
        select: function (start) {
            if (canEditReminders) {
                var selectedDate = new Date(start);
                var todaysDate = new Date(currentDate);
                if (selectedDate < todaysDate) {
                    return;
                } else {
                    $('#assessmentReminderDto_Start').val($.datepicker.formatDate('mm/dd/yy', selectedDate));
                    $('#createEventModal').modal('show');
                }
            }
        },
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        editable: true,
        droppable: true,
        eventDrop: function (event, dayDelta) {
            $.ajax({ url: remindersBase + '/UpdateDate/' + event.id, data: { dayDelta: dayDelta }, type: "POST" })
                .done(function () {
                    calendar.fullCalendar('refetchEvents');
                });
        },
        events: source,
        eventAfterRender: function (event, element, view) {
            var description = "Cancel reminder for " + event.title + " on " + event.start;
            var layer = "<div id='events-layer' " +
                        "class='fc-transparent'>" +
                        "<a class='cancelEvent' aria-label='" + description + "' data-icon='&#xe0fd;' href='/AssessmentReminder/Cancel/?key=" + event.id + "&recurrenceKey=" + event.recurrenceKey + "'></a></div>";
            $(element).append(layer);
            $('.cancelEvent').ajaxLink(
            {
                success: function() {
                    calendar.fullCalendar('refetchEvents');
                },
            });
        },
        dayClick:
            function (date) {
                if (canEditReminders) {
                    var selectedDate = new Date(date);
                    var todaysDate = new Date(currentDate);
                    if (selectedDate < todaysDate) {
                        return;
                    } else {
                        $('#assessmentReminderDto_Start').val($.datepicker.formatDate('mm/dd/yy', date));
                        initPatientDropdown("createEventModal");
                        initFinderDropdown();
                        $('#createEventModal').modal('show');
                    }
                }
            },
        eventClick:
            function(calEvent) {
                $.ajax(remindersBase + '/Get/' + calEvent.recurrenceKey)
                    .done(function (data) {
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
                        if (patientKey) {
                            initPatientDropdown("editEventModal");
                            initFinderDropdown();
                        }
                        $('#assessmentReminder_Start').val($.datepicker.formatDate('mm/dd/yy', new Date($('#assessmentReminder_Start').val())));
                        initReminder('#editEventModal');
                        $('#editEventModal').modal('show');
                    })
                    .fail(function () {
                    });
            },
    });

    $('#calendar-widget').on('collapsed expanded', function () {
        $('#calendar').fullCalendar('render');
    });

    $('#assessmentReminderDto_Start').datepicker({ minDate: '-0d' });
    $('#assessmentReminderDto_End').datepicker({ minDate: '-0d' });

    $("#create-event-btn,#edit-event-btn").ajaxLink({
        getData: function (id, evt) {
            //The event is a one time event if the recurring event tab is hidden
            var isOneTimeEvent = $(this).parent().parent().find('#recurring-event').css('display') == 'none';
            var data = {};
            var isValid = true;
            var lastType = "";

            if ($(this).attr('id').indexOf('create') > -1) {
                $("#assessmentReminderDto_Key").remove();
            }

            $(this).closest('.modal-footer').prev().find(':input').each(function() {
                if ($(this).parent().hasClass("finder-wrapper")) {
                    var selectedData = $(this).finder().selectedData;
                    if (selectedData) {
                        data[this.name] = selectedData.Key;
                    }
                    isValid = isValid & $(this).valid();
                }
                else {
                    if (lastType != "checkbox" || this.type != "hidden" || !data.hasOwnProperty(this.name)) {
                        if (this.type != "radio" && this.type != "checkbox") {
                            data[this.name] = this.value;
                        }
                        if (this.type == "radio" && this.checked == true) {
                            data[this.name] = this.value;
                        }
                        if (this.type == "checkbox") {
                            data[this.name] = $(this).is(":checked");
                        }
                    }
                    if ($(this).prop('type') !== 'hidden', this.type != "radio") {
                        isValid = isValid & $(this).valid();
                    }
                    lastType = this.type;
                }
            });
            if (!isValid) {
                return false;
            }
            if (isOneTimeEvent) {
                var modelName = 'assessmentReminder';
                if ($(this).attr('id') == 'create-event-btn') {
                    modelName = 'assessmentReminderDto';
                }
                data[modelName + ".ReminderRecurrence"] = "OneTime";
                data[modelName + ".RecurrenceEndDate"] = data[modelName + ".Start"];
            }

            return data;
        },
        success: function () {
            $('#createEventModal').modal('hide');
            $('#editEventModal').modal('hide');
        }
    });

    $('#createEventModal, #editEventModal').on('hidden', function () {
        $('#createEventModal form :input').each(function () {
            $(this).val('');
        });
        calendar.fullCalendar.lazyFetching = false;
        calendar.fullCalendar('refetchEvents');
        calendar.fullCalendar('rerenderEvents');
    });

    $('#createEventModal').on("show", function () {
        initReminder();
    });

    initReminder("#createEventModal,#editEventModal");
}