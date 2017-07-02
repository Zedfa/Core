function updateSite() {
    var siteVersionHub = $.connection.siteVersionHub, intervalList = [];
    siteVersionHub.client.reset = function () {
        checkVersion(siteVersionHub);
    };
    $.connection.hub.start(function () {
        checkVersion(siteVersionHub);
        if (intervalList.length > 0) {
            $.each(intervalList, function (i, interval) {
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
    $(window).focus(function (e) {
        window.active = true;
        if ($.connection.siteVersionHub.connection.state == $.connection.connectionState.disconnected) {
            $.connection.hub.start();
        }
    });
    $(window).blur(function (e) {
        window.active = false;
        manageLostConnection();
    });
    function manageLostConnection() {
        $.connection.hub.stop();
        intervalList.push(setInterval(function () {
            if (window.active && $.connection.siteVersionHub.connection.state == $.connection.connectionState.disconnected) {
                $.connection.hub.start();
            }
        }, 300));
    }
}
function checkVersion(hub) {
    var clientVer = Number(getCookieByKey("ClientVersion")), winVer = Number(sessionStorage.getItem("winVer"));
    if (winVer == 0) {
        winVer = clientVer;
    }
    else if ((winVer < clientVer - 3) || (winVer > clientVer)) {
        winVer = clientVer - 3;
    }
    var defferd = hub.server.check(winVer);
    defferd.then(function (versions) {
        if (versions == null) {
            console.log("context is null");
        }
        else {
            sessionStorage.setItem("winVer", versions.Client);
            if (versions.Server != versions.Client) {
                try {
                    applicationCache.update();
                }
                catch (err) {
                }
                finally {
                    var cookies = document.cookie.split(";");
                    for (var i = 0; i < cookies.length; i++) {
                        var cookie = cookies[i];
                        var eqPos = cookie.indexOf("=");
                        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
                        document.cookie = name + "=;expires=1970-01-10T08:38:40.048Z; path=/;";
                    }
                    window.location.assign(window.location.origin);
                }
            }
        }
    });
}
function updateSiteCache(event) {
    window.applicationCache.swapCache();
}
function checkingSiteCache(event) {
}
function progressSiteCache(event) {
    if (event.loaded == 0) {
        applicationCache.abort();
    }
}
function obsoleteSiteCache(event) {
}
function downloadingSiteCache(event) {
}
;
