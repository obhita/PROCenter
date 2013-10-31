window.procenter.InitializePatientFeed = function(patientBase, patientKey) {
    //ajax call for patient feed
    $.ajax({
        url: patientBase + '/PatientFeed/' + patientKey,
        type: "POST",
        success: function(data) {
            $('#patientFeed').html(data);
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
        }
    });
}