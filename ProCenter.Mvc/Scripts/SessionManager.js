window.procenter.SessionManagerModule = function () {
    console.log("session manager module loaded...");
    var logoutHandler, sessionExpirationTimerHandler;
    var sessionTimeoutMessage;

    function initialize() {
        initializeSessionManagerModal();
    };

    function initializeSessionManagerModal() {
        $('#sessionManagerModal').modal({
            keyboard: false,
            backdrop: 'static',
            show: false
        });
        
        $('#sessionManagerModal').on('shown', function () {
            $('#sessionManagerModalStayConnected').focus();
        });

        $('#sessionManagerModalStayConnected').on('click', function () {
            $('#sessionManagerModal').modal('hide');
            resetSessionExpiration();
        });

        $('#sessionManagerModalLogout').on('click', function () {
            $('#sessionManagerModal').modal('hide');
            logout();
        });

        $.get('/Home/GetSessionManagerConstants')
           .done(function (results) {
               scheduleSessionExpirationPrompt(results);
               sessionTimeoutMessage = results.SessionTimeoutMessage;
           });

    }

    function showSessionExpirationWarning(results) {
        var logoutTimeInMilliSecs = results.SessionExpirationPromptTime * 60 * 1000;
        scheduleLogout(logoutTimeInMilliSecs, results.LogoutPageUrl);
        setSessionExpirationTimer((results.SessionTimeout - results.SessionExpirationPromptTime) * 60);
        $('#sessionManagerModal').modal('show');
        $('#sessionManagerModalOk').focus();
    }

    function setSessionExpirationTimer(sessionExpirationTime) {
        $('#sessionManagerModal .modal-body').text(sessionTimeoutMessage + sessionExpirationTime + " second(s).");
        sessionExpirationTimerHandler = setTimeout(function () { setSessionExpirationTimer(sessionExpirationTime - 1); }, 1000);
    }

    function resetSessionExpiration() {
        $.get('/Home/GetSessionManagerConstants')
           .done(function (results) {
               clearTimeout(sessionExpirationTimerHandler);
               scheduleSessionExpirationPrompt(results);
               clearTimeout(logoutHandler);
               sessionTimeoutMessage = results.SessionTimeoutMessage;
        });
    }

    function scheduleSessionExpirationPrompt(results) {
        setTimeout(function () { showSessionExpirationWarning(results); },
            (results.SessionExpirationPromptTime) * 60 * 1000);
    }

    function scheduleLogout(logoutTime, logoutPageUrl) {
        logoutHandler = setTimeout(function () { redirectToLogoutPage(logoutPageUrl); }, logoutTime);
    }

    function logout() {
        $.get('/Account/LogoutAndGetLogoutPageUrl')
               .done(function (results) {
                   redirectToLogoutPage(results.LogoutPageUrl);
               });
    }

    function redirectToLogoutPage(logoutPageUrl) {
        window.location = logoutPageUrl;
    }

    return {
        Initialize: initialize,
    };
}