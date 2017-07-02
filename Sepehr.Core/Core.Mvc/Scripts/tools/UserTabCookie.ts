

module ns_Cookie {
    export function clearCookie(cookieUserName) {
        var cval = document.cookie;
    }

    export function setTabsInLocalStorage(tabItems) {

        if (typeof tabItems == 'object') {
            var copyOfInsertedTabs = clone(tabItems);
            $.each(copyOfInsertedTabs, function (key, val) {
               
                val.Url = encodeURI(val.Url);
            });
            localStorage.setItem("tabItems", JSON.stringify(copyOfInsertedTabs));
        }
     
    }


    var replaceUserCookie = function (startEnd, userSpecCookie) {
        var cookVal = document.cookie;
        cookVal.replace(cookVal.substring(startEnd.sIx, startEnd.eIx), userSpecCookie);
    }

    var getUserCookieStartEndIndex = function (cookieUserName) {
        var cookieValue = document.cookie;
        var startEnd = { sIx: 0, eIx: 0 };
        var cookieStart = cookieValue.indexOf(" un=" + cookieUserName);
        if (cookieStart == -1) {
            cookieStart = cookieValue.indexOf("un=" + cookieUserName);
        }
        if (cookieStart == -1) {
            cookieValue = null;
        }
        else {
            var cookieEnd = cookieValue.indexOf("un=", cookieStart) - 1;
            if (cookieEnd == -1) {
                cookieEnd = cookieValue.length;
            }
        }
        startEnd.sIx = cookieStart;
        startEnd.eIx = cookieEnd;
        return startEnd;
    }

    var cookieAlreadyExists = function (cookieUserName, cv) {
        var alreadyExists = false;
        if (cv[cookieUserName])
            return true;
        return false;
    }

}


