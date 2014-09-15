window.procenter.checkAlerts = function (messageFormat) {
    var alertTempate = '<div class="alert notification"><button type="button" class="close pull-right" data-dismiss="alert">x</button></div>';
    var $container = $('#mainContent').prepend('<div class="portal-alert-container"></div>').find('.portal-alert-container');
    $('#patientFeed .assessment-summary[data-assessment-name]').each(function () {
        var $self = $(this);
        var assessmentTitle = $self.attr('data-assessment-name');
        var $alert = $(alertTempate);
        var $link = $self.find('.btn-view-assessment');
        $alert.prepend($link.clone().text('Click here to start'));
        $alert.prepend(procenter.stringFormat(messageFormat, assessmentTitle));
        $container.append($alert);
        $alert.alert();
    });
};