window.procenter.InitializeReminders = function (remindersBase, remindersApiBase, canEditReminders, patientKey) {
    var search = undefined;
    var source = remindersApiBase + '/Get/';
    if (patientKey) {
        source = source + '?patientKey=' + patientKey;
    }

    $("#dashboard-filter").on("keyup search", function () {
        var tables = $.fn.dataTable.fnTables(true);
        for (var i = 0; i < tables.length; i++) {
            $(tables[i]).dataTable().fnFilter(this.value);
        }
        search = this.value;
        if (search && search !== '') {
            $('#calendar').fullCalendar('removeEventSource', source);
            source = source + '?sSearch=' + search;
            if (patientKey) {
                source = source + '&patientKey=' + patientKey;
            }
            $('#calendar').fullCalendar('addEventSource', source);
        }
    });

    function initReminder(selector) {
        $(selector).find('[data-disabled=true] :input').attr('disabled','disabled');
        $(selector).find('.set-reminder-button').click(function () {
            $(this).hide();
            $(selector).find('.set-reminder-wrapper').removeClass("hidden").show();
        });
    }

    //calendar
    var calendar = $('#calendar').fullCalendar({
        theme: true,
        selectable: true,
        selectHelper: true,
        select: function (start) {
            $('#assessmentReminderDto_Start').val(start.toLocaleDateString());
            $('#createEventModal').modal('show');
        },
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        editable: true,
        droppable: true,
        eventDrop: function (event, dayDelta) {
            $.ajax({ url: remindersApiBase + '/UpdateDate/")' + event.id, data: { dayDelta: dayDelta }, type: "POST" })
                .done(function () {
                    calendar.fullCalendar('refetchEvents');
                });
        },
        events: source,
        dayClick:
            function (date) {
                if (canEditReminders) {
                    $('#assessmentReminderDto_Start').val(date.toLocaleDateString());
                    $('#createEventModal').modal('show');
                }
            },
        eventClick:
            function(calEvent) {
                $.ajax(remindersBase + '/Get/' + calEvent.id)
                    .done(function(data) {
                        $('#editEventModal .modal-body').html('<form>' + data + '</form>');
                        $.validator.unobtrusive.parse('#editEventModal'); // Note: only work with new added forms
                        $('#assessmentReminder_Start').datepicker({ minDate: '-0d' });
                        $('#editEventModal [data-control=finder]').finder();
                        initReminder('#editEventModal');
                        $('#editEventModal').modal('show');
                    })
                    .fail(function() {
                    });
            },

        eventMouseover: function (calEvent) {
            if (canEditReminders) {
                var layer = "<div id='events-layer' class='fc-transparent'><a id='cancelEvent' data-icon='&#xe0fd;' href='/AssessmentReminder/Cancel/" + calEvent.id + "'></a></div>";
                $(this).append(layer);
                $('#cancelEvent').ajaxLink(
                    {
                        success: function() {
                            calendar.fullCalendar('refetchEvents');
                        },
                    });
            }
        },
        eventMouseout: function () {
            $("#events-layer").remove();
        },
    });

    $('#calendar-widget').on('collapsed expanded', function () {
        $('#calendar').fullCalendar('render');
    });

    $('#assessmentReminderDto_Start').datepicker({ minDate: '-0d' });

    $("#create-event-btn,#edit-event-btn").ajaxLink({
        getData: function () {
            var data = {};
            var isValid = true;
            var lastType = "";
            $(this).closest('.modal-footer').prev().find(':input').each(function() {
                if ($(this).parent().hasClass("finder-wrapper")) {
                    var selectedData = $(this).finder().selectedData;
                    if (selectedData) {
                        data[this.name] = selectedData.Key;
                    }
                    isValid = isValid & $(this).valid();
                } else {
                    if (lastType != "checkbox" || this.type != "hidden" || !data.hasOwnProperty(this.name)) {
                        data[this.name] = this.value;
                    }
                    if ($(this).prop('type') !== 'hidden') {
                        isValid = isValid & $(this).valid();
                    }
                    lastType = this.type;
                }
            });
            if (!isValid) {
                return false;
            }
            return data;
        },
        success: function () {
            calendar.fullCalendar('refetchEvents');
            $('#createEventModal').modal('hide');
            $('#editEventModal').modal('hide');
        }
    });

    $('#createEventModal').on('hidden', function () {
        $('#createEventModal').find('.set-reminder-button').show();
        $('#createEventModal').find('.set-reminder-wrapper').hide();
        $('#createEventModal form :input').each(function () {
            $(this).val('');
        });
    });

    initReminder("#createEventModal,#editEventModal");
}