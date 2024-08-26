$(function() {

    $.sessionTimeout({
        ignoreUserActivity: true,

        keepAliveUrl: 'http://localhost:46333/Login?handler=ExtendSession',
        ajaxType: 'GET',

        logoutUrl: 'http://localhost:46333/Logout',
        redirUrl: 'http://localhost:46333/Logout',

        warnAfter: 180000,
        redirAfter: 300000, //it's always larger than warnAfter time.
        countdownMessage: 'Redirecting in {timer} seconds.'
    });
});