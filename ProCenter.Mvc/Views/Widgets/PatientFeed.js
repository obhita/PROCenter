window.procenter.InitializePatientFeed = function(patientBase, patientKey, alertMessageFormat) {
    //ajax call for patient feed
    $.ajax({
        url: patientBase + '/PatientFeed/' + patientKey,
        type: "POST",
        success: function(data) {
            $('#patientFeed').append(data);
            $('#patientFeed').find(".pie-chart").easyPieChart({
                barColor: '#00aeef',
                trackColor: '#00aeef',
                scaleColor: false,
                lineWidth: 8,
                trackLineWidth: 2,
                lineCap: 'square'
            });

            $('.reminder-message .btn-acknowledge').ajaxLink({
                success: function(data) {
                    $('#patientFeed').find('.' + data.key).closest('.patient-feed-content').remove();
                }
            });
            
            $('.assessment-summary .btn-sendemail').ajaxLink({
                success: function (data) {
                    var emailSentDate = $('#emailSentDate_' + data.Key);
                    var emailSentDateContainer = $('#emailSentDateContainer_' + data.Key);
                    var emailFailedDateContainer = $('#emailFailedDateContainer_' + data.Key);
                    var emailFailedDate = $('#emailFailedDate_' + data.Key);
                    if (data.IsSuccessful) {
                        emailSentDate.html(data.EmailSentDate);
                        emailFailedDateContainer.hide();
                        emailSentDateContainer.show();
                    } else {
                        emailFailedDate.html(data.EmailFailedDate);
                        emailFailedDateContainer.show();
                    }
                }
            });

            if (window.procenter.checkAlerts) {
                window.procenter.checkAlerts(alertMessageFormat);
            }
        }
    });
}