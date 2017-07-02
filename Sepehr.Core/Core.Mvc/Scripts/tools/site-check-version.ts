/// <reference path="../../scripts/typings/signalr/signalr.d.ts" />
/// <reference path="../../scripts/customtypescriptextensions/hubextention.main.d.ts" />
/// <reference path="../customtypescriptextensions/windowextension.d.ts" />

function updateSite() {
    var siteVersionHub = $.connection.siteVersionHub,
        intervalList = [];


    siteVersionHub.client.reset = function () {
        checkVersion(siteVersionHub);

    };

    $.connection.hub.start(function () {
        checkVersion(siteVersionHub);

        if (intervalList.length > 0) {

            $.each(intervalList, (i, interval) => {
                clearInterval(interval);
                intervalList.splice(i, 1);
            });
        }
    });
    $.connection.hub.reconnected(function () {
        checkVersion(siteVersionHub);
    });

    $.connection.hub.disconnected(function () {
        manageLostConnection();

    });
    //api/SiteVersionApi/get

    //$.connection.hub.stateChanged(function (change) {

    //    if (change.newState === $.connection.connectionState.reconnecting) {
    //        $.connection.hub.stop();//raise disconnected event
    //    }
    //});

    $(window).focus((e) => {
        window.active = true;
        // 0:'connecting', 1: 'connected', 2: 'reconnecting', 3: 'disconnected'
        if ($.connection.siteVersionHub.connection.state == $.connection.connectionState.disconnected) {
            $.connection.hub.start();
        }

    });
    $(window).blur((e) => {
        //alert("blur");
        window.active = false;
        manageLostConnection();

    });

    function manageLostConnection() {

        $.connection.hub.stop();
        intervalList.push(
            setInterval(function () {
                if (window.active && $.connection.siteVersionHub.connection.state == $.connection.connectionState.disconnected) {
                    $.connection.hub.start();
                }

            }, 300)); // Restart connection after 5 min.

    }
}

function checkVersion(hub) {

    var clientVer = Number(getCookieByKey("ClientVersion")),
        winVer = Number(sessionStorage.getItem("winVer"));
    //if (winVer == 0 || (winVer < clientVer-3)) {
    //    sessionStorage.setItem("winVer", clientVer.toString());
    //}
    //if (winVer < clientVer && winVer >= clientVer - 3)
    //{
    //    winVer += 1;

    //}
    //else {
    //    winVer = clientVer;

    //}
    if (winVer == 0) {
        winVer = clientVer;
    }
    else if ((winVer < clientVer - 3) || (winVer > clientVer)) {
        winVer = clientVer - 3;
    }

    //var clientVer = Number(sessionStorage.getItem("clientVer")),
    var defferd = hub.server.check(winVer);

    defferd.then((versions) => {
        if (versions == null) {
            console.log("context is null");
        }
        else {
            sessionStorage.setItem("winVer", versions.Client);

            if (versions.Server != versions.Client) {

                try {
                    applicationCache.update()
                }
                catch (err) {

                }
                finally {
                    // window.location.reload(true);
                    var cookies = document.cookie.split(";");

                    for (var i = 0; i < cookies.length; i++) {
                        var cookie = cookies[i];
                        var eqPos = cookie.indexOf("=");
                        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
                        document.cookie = name + "=;expires=1970-01-10T08:38:40.048Z; path=/;";
                    }
                    localStorage.clear(),
                        sessionStorage.clear();
                    window.location.assign((<any>window.location).origin);

                }
            }
        }
    });
}

function updateSiteCache(event) {

    window.applicationCache.swapCache();

}
function checkingSiteCache(event) {

    //window.applicationCache.swapCache();

}
function progressSiteCache(event) {
    if (event.loaded == 0) {

        applicationCache.abort();
    }
    //if (event.path.length > 0)
    //{
    //   

    //}
    // window.applicationCache.swapCache();

}
function obsoleteSiteCache(event) {

    //window.applicationCache.swapCache();

}

function downloadingSiteCache(event) {

    //window.applicationCache.swapCache();

};
//window.applicationCache.addEventListener('updateready',updateSiteCache, false);

//window.applicationCache.addEventListener('checking', checkingSiteCache, false);
//window.applicationCache.addEventListener('progress', progressSiteCache, false);

//window.applicationCache.addEventListener('obsolete', obsoleteSiteCache, false);
//window.applicationCache.addEventListener('downloading', downloadingSiteCache, false);

