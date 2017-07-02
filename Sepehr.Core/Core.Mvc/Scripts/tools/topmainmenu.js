var ns_MainMenu;
(function (ns_MainMenu) {
    ns_MainMenu.tabStrip = null;
    ns_MainMenu.insertedTabs = [];
    ns_MainMenu.tbConfig = null;
    ns_MainMenu.isComeFromRemoveOp = false;
    ns_MainMenu.isComeFromRfresh = false;
    ns_MainMenu.reportUrlParams = '';
    ns_MainMenu.reportName = '';
    ns_MainMenu.itemText = '';
    ns_MainMenu.reportContentCount = 0;
    ns_MainMenu.wndmodalAboutUs = null;
    ns_MainMenu.tabAddedForReport = false;
    ns_MainMenu.hasFirstItemSelected = false;
    ns_MainMenu.comeFromTabClick = true;
    ns_MainMenu.isComFromMenuItemClick = false;
    ns_MainMenu.isComeFromAddressBar = false;
    ns_MainMenu.menuItems = [];
    function menuItemSelected(e, url, text, queryString) {
        ns_MainMenu.isComFromMenuItemClick = true;
        var selectedItemUrl = e && !url ? $(e.item.firstChild).find("span").attr("id") : url;
        var tabCaption = e && !text ? $(e.item.firstChild).find("span").text() : text;
        ns_MainMenu.comeFromTabClick = true;
        if (selectedItemUrl) {
            var uniqueName = selectedItemUrl.split('#')[0];
            selectedItemUrl = selectedItemUrl.split('#')[1];
            if (selectedItemUrl == 'CoreAboutUs') {
                ns_MainMenu.wndmodalAboutUs.center();
                ns_MainMenu.wndmodalAboutUs.open();
                return;
            }
            if (selectedItemUrl) {
                ns_MainMenu.itemText = e && !text ? $(e.item.firstChild).find("span").text() : text;
                if (selectedItemUrl.indexOf('Report_') == 0) {
                    ns_MainMenu.reportName = ns_MainMenu.itemText;
                    ns_MainMenu.reportId = selectedItemUrl;
                    var reportTemplate;
                    if (!reportTemplate) {
                        $.get("Report/Index", { tempView: selectedItemUrl }).done(function (templ) {
                            var currentTemplate = templ.toString().replace(/#/gm, '\\#').replace(/<\/script>/gm, '<\\/script>');
                            showReportParameterDialog(currentTemplate, ns_MainMenu.reportName, ns_MainMenu.reportId);
                        }).fail(function (err) { });
                    }
                    else {
                        showReportParameterDialog(reportTemplate, ns_MainMenu.reportName, ns_MainMenu.reportId);
                    }
                }
                else {
                    makeProperTabDynamic(ns_MainMenu.itemText, null, null, null, uniqueName, queryString);
                }
            }
        }
    }
    ns_MainMenu.menuItemSelected = menuItemSelected;
    function onError(e) {
    }
    ns_MainMenu.onError = onError;
    function onTabItemSelected(e, location, uniqueName) {
        var itemUrl = $(e.item).find("span.k-i-close").attr("id");
        if (itemUrl) {
            if (itemUrl.indexOf("Report") == -1 && !ns_MainMenu.isComeFromRemoveOp) {
                if (!ns_MainMenu.isComeFromRfresh) {
                    reArrangeInsertedItems(location, uniqueName);
                    putTabInfoIntoLocalStorage(ns_MainMenu.insertedTabs);
                }
                else if (ns_MainMenu.isComeFromRfresh) {
                    if (ns_MainMenu.insertedTabs.length == 1 && !ns_MainMenu.hasFirstItemSelected) {
                    }
                    ns_MainMenu.isComeFromRfresh = false;
                }
                ns_MainMenu.hasFirstItemSelected = false;
            }
        }
    }
    ns_MainMenu.onTabItemSelected = onTabItemSelected;
    function putTabInfoIntoLocalStorage(storingTabs) {
        var noneReportTabs = $.grep(storingTabs, function (tc) { return !tc.IsReport; });
        var noneLastSelectedFound = true;
        for (var i = 0; i < noneReportTabs.length; i++) {
            if (noneReportTabs[i].IsLastSelected) {
                noneLastSelectedFound = false;
                break;
            }
        }
        if (noneLastSelectedFound && noneReportTabs.length) {
            noneReportTabs[0].IsLastSelected = true;
        }
        ns_Cookie.setTabsInLocalStorage(noneReportTabs);
    }
    ns_MainMenu.putTabInfoIntoLocalStorage = putTabInfoIntoLocalStorage;
    function reArrangeInsertedItems(location, uniqueName) {
        var itemFoundIx = -1;
        for (var ix = 0; ix < ns_MainMenu.insertedTabs.length; ix++) {
            if (ns_MainMenu.insertedTabs[ix].Url == uniqueName) {
                itemFoundIx = ix;
                ns_MainMenu.insertedTabs[itemFoundIx].IsLastSelected = true;
                if (!ns_MainMenu.comeFromTabClick) {
                    location.path("/menu/" + uniqueName);
                    window.location.href = location.$$absUrl;
                }
                break;
            }
        }
        if (itemFoundIx != -1) {
            makeNodesNonLastSelected(itemFoundIx);
        }
    }
    ns_MainMenu.reArrangeInsertedItems = reArrangeInsertedItems;
    function makeNodesNonLastSelected(itemFoundIx) {
        for (var ix = 0; ix < ns_MainMenu.insertedTabs.length; ix++) {
            if (ns_MainMenu.insertedTabs[ix].Url != ns_MainMenu.insertedTabs[itemFoundIx].Url) {
                ns_MainMenu.insertedTabs[ix].IsLastSelected = false;
            }
        }
    }
    ns_MainMenu.makeNodesNonLastSelected = makeNodesNonLastSelected;
    function onTabItemActivated(e) {
    }
    ns_MainMenu.onTabItemActivated = onTabItemActivated;
    function onTabItemContentLoad(e) {
    }
    ns_MainMenu.onTabItemContentLoad = onTabItemContentLoad;
    function onTabItemError(e) { }
    ns_MainMenu.onTabItemError = onTabItemError;
    function showReportParameterDialog(repParameterUrl, repName, repID, width, height) {
        var parentTag = $("#topMenu");
        var container = $("<div>");
        parentTag.append(container);
        var repParamDialogTemplate = { url: repParameterUrl, data: null };
        var w = (width == undefined || width == null) ? 750 : width;
        var h = (height == undefined || height == null) ? 300 : height;
        var win = container.kendoWindow({
            width: w,
            height: h,
            modal: true,
            actions: ["Refresh", "Maximize", "Minimize", "Close"],
            title: "تعیین پارامترهای " + " " + repName,
            visible: false,
            resizable: false,
            scrollable: false,
            animation: { "open": { "effects": "fadeIn" }, "destroy": { "effects": "fadeOut", "reverse": true } },
            refresh: function (e) {
                var inputVals;
                if (inputVals) {
                    for (var item in inputVals) {
                        if (inputVals[item].dropdownlist) {
                            $('#' + item).data('kendoDropDownList').select(parseInt(inputVals[item].dropdownlist));
                            $('#' + item).attr('value', inputVals[item].dropdownlist);
                        }
                        else if (inputVals[item].multiselect) {
                            if ($('#' + item).data('kendoMultiSelect').dataSource.data().length != 0)
                                $('#' + item).data('kendoMultiSelect').value(inputVals[item].multiselect);
                        }
                        else if (inputVals[item].numerictextbox || inputVals[item].numerictextbox === '') {
                            $('#' + item).data('kendoNumericTextBox').value(inputVals[item].numerictextbox);
                        }
                        else {
                            $('#' + item).val(inputVals[item]);
                        }
                    }
                }
            },
            content: repParamDialogTemplate,
            close: function () {
                this.destroy();
            }
        }).data("kendoWindow");
        win.center().open();
    }
    ns_MainMenu.showReportParameterDialog = showReportParameterDialog;
    function makeProperTabDynamic(pageName, url, isLastSelected, index, menuUrl, queryString) {
        var alreadyInserted = false;
        var cUrl = null;
        if (!url) {
            cUrl = menuUrl;
            if (cUrl) {
                ns_MainMenu.insertedTabs.forEach(function (tabItem) {
                    if (tabItem.Url == menuUrl + queryString) {
                        alreadyInserted = true;
                    }
                    return;
                });
            }
            else
                return;
            if (!alreadyInserted) {
                appendTabToTabStrip(pageName, cUrl, queryString);
                if (!checkIfItemAlreadyExists(menuUrl + queryString)) {
                    var tbConfig = { TabName: pageName, Url: cUrl + queryString, IsLastSelected: true, wasTabsContentLoaded: true };
                    ns_MainMenu.insertedTabs.push(tbConfig);
                    $.each(ns_MainMenu.insertedTabs, function (key, val) {
                        if (key == ns_MainMenu.insertedTabs.length - 1) {
                            return;
                        }
                        else {
                            val.IsLastSelected = false;
                        }
                    });
                    ns_MainMenu.tabStrip.select((ns_MainMenu.tabStrip.tabGroup.children("li").length - 1));
                }
            }
            else {
                ns_MainMenu.tabStrip.select(getIndexOfAlreadyInsertedItem(menuUrl + queryString));
            }
        }
        else {
            appendTabToTabStrip(pageName, url, queryString);
            if (!checkIfItemAlreadyExists(url)) {
                var tbConfig = { TabName: pageName, Url: url + queryString, IsLastSelected: isLastSelected, wasTabsContentLoaded: true };
                ns_MainMenu.insertedTabs.push(tbConfig);
                if (isLastSelected == true) {
                    ns_MainMenu.tabStrip.select(index);
                }
            }
        }
    }
    ns_MainMenu.makeProperTabDynamic = makeProperTabDynamic;
    function appendTabToTabStrip(pageName, url, queryString) {
        var uniqueName = url + queryString;
        var tabTitle = pageName + "<span class='k-icon k-i-close' id='" + uniqueName + "' onclick=' ns_MainMenu.tabStrip.remove($(this).closest(\"li\")); ns_MainMenu.removeTabItemFromSession($(this).attr(\"id\")); ' /> ";
        ns_MainMenu.tabStrip.append({
            text: tabTitle,
            encoded: false
        });
    }
    ns_MainMenu.appendTabToTabStrip = appendTabToTabStrip;
    function defineNextSelectedAfterRemoval(foundIndex) {
        if (foundIndex != -1) {
            var listLength = ns_MainMenu.insertedTabs.length;
            if (ns_MainMenu.insertedTabs[foundIndex].IsLastSelected) {
                if (listLength > 1) {
                    if (foundIndex == listLength - 1) {
                        ns_MainMenu.insertedTabs[foundIndex - 1].IsLastSelected = true;
                    }
                    else {
                        ns_MainMenu.insertedTabs[foundIndex + 1].IsLastSelected = true;
                    }
                }
            }
        }
    }
    ns_MainMenu.defineNextSelectedAfterRemoval = defineNextSelectedAfterRemoval;
    function getUrlFromAccessibleViewElementsByUniqueName(uniqueName) {
        var queryString = "";
        var elements = ns_MainMenu.$initialScope.AccessibleViewElements;
        for (var i = 0; i < elements.length; i++) {
            if (elements[i].UniqueName.split('#')[0] == uniqueName) {
                if (elements[i].LastEnteredURL != undefined) {
                    queryString = elements[i].LastEnteredURL;
                }
                break;
            }
        }
        return "/menu/" + uniqueName + queryString;
    }
    ns_MainMenu.getUrlFromAccessibleViewElementsByUniqueName = getUrlFromAccessibleViewElementsByUniqueName;
    function removeAllTabs() {
        $.each(ns_MainMenu.insertedTabs, function (key, val) {
            var element = document.getElementById(val.Url.hashCode());
            $(element).detach(),
                $(element).remove();
        });
        localStorage.removeItem("tabItems");
    }
    ns_MainMenu.removeAllTabs = removeAllTabs;
    function removeTabItemFromSession(pageUrl) {
        var alreadyInsertedIndex = getIndexOfAlreadyInsertedItem(pageUrl);
        if (alreadyInsertedIndex != -1 && ns_MainMenu.insertedTabs[alreadyInsertedIndex].IsReport) {
            ns_MainMenu.insertedTabs = $.grep(ns_MainMenu.insertedTabs, function (tc) { return tc.Url != pageUrl; });
            var lastInsertedIndex = getLastInsertedItem();
            var itemUrl = ns_MainMenu.insertedTabs[lastInsertedIndex].Url;
            ns_MainMenu.tabStrip.select(lastInsertedIndex);
            var url = itemUrl;
            ns_MainMenu.$initialScope.location.path(url);
            if (!ns_MainMenu.$initialScope.$$phase)
                ns_MainMenu.$initialScope.$apply();
        }
        else {
            ns_MainMenu.defineNextSelectedAfterRemoval(alreadyInsertedIndex);
            ns_MainMenu.insertedTabs = $.grep(ns_MainMenu.insertedTabs, function (tc) { return tc.Url != pageUrl; });
            if (ns_MainMenu.insertedTabs.length == 0) {
                ns_MainMenu.$initialScope.location.path("/");
                if (!ns_MainMenu.$initialScope.$$phase)
                    ns_MainMenu.$initialScope.$apply();
            }
            else if (ns_MainMenu.insertedTabs.length > 1) {
                $.each(ns_MainMenu.insertedTabs, function (key, val) {
                    if (val.IsLastSelected) {
                        var urlByUniqueName = getUrlFromAccessibleViewElementsByUniqueName(val.Url);
                        var url = urlByUniqueName;
                        ns_MainMenu.$initialScope.location.path(url);
                        if (!ns_MainMenu.$initialScope.$$phase)
                            ns_MainMenu.$initialScope.$apply();
                        SelectProperTab(key);
                        return;
                    }
                });
            }
            else {
                var uniqueName = ns_MainMenu.tabStrip.tabGroup.children("li").find("span.k-link > span").attr('id');
                var urlByUniqueName = getUrlFromAccessibleViewElementsByUniqueName(uniqueName);
                ns_MainMenu.$initialScope.location.path(urlByUniqueName);
                if (!ns_MainMenu.$initialScope.$$phase)
                    ns_MainMenu.$initialScope.$apply();
                SelectProperTab(0);
            }
            putTabInfoIntoLocalStorage(ns_MainMenu.insertedTabs);
        }
        var element = document.getElementById(pageUrl.hashCode());
        $(element).detach(),
            $(element).remove();
    }
    ns_MainMenu.removeTabItemFromSession = removeTabItemFromSession;
    function SelectProperTab(key) {
        ns_MainMenu.isComeFromRemoveOp = true;
        ns_MainMenu.tabStrip.select(key);
        ns_MainMenu.isComeFromRemoveOp = false;
    }
    ns_MainMenu.SelectProperTab = SelectProperTab;
    function getLastInsertedItem() {
        var foundIndex = -1;
        for (var i = 0; i < ns_MainMenu.insertedTabs.length; i++) {
            if (ns_MainMenu.insertedTabs[i].IsLastSelected) {
                foundIndex = i;
                break;
            }
        }
        return foundIndex;
    }
    ns_MainMenu.getLastInsertedItem = getLastInsertedItem;
    function checkIfItemAlreadyExists(url) {
        var ifExists = false;
        for (var i = 0; i < ns_MainMenu.insertedTabs.length; i++) {
            if (ns_MainMenu.insertedTabs[i].Url == url) {
                ifExists = true;
                break;
            }
        }
        return ifExists;
    }
    ns_MainMenu.checkIfItemAlreadyExists = checkIfItemAlreadyExists;
    function getIndexOfAlreadyInsertedItem(url) {
        var foundIndex = -1;
        for (var i = 0; i < ns_MainMenu.insertedTabs.length; i++) {
            if (ns_MainMenu.insertedTabs[i].Url == url) {
                foundIndex = i;
                break;
            }
        }
        return foundIndex;
    }
    ns_MainMenu.getIndexOfAlreadyInsertedItem = getIndexOfAlreadyInsertedItem;
    function appendTabForReport(url, reportName, repUrlParams, illustrationType, dontShowRepParamWindow) {
        var alreadyInserted = false;
        var addr = 'Report/ApplyParamsLoadCorrespondentReport';
        ns_MainMenu.reportUrlParams = repUrlParams;
        var cont = '';
        if (!alreadyInserted) {
            var reportPage = '';
            var reportID = url.indexOf('cshtml') != -1 ? url : (url + ".cshtml");
            $.post(addr, repUrlParams).done(function (x) {
                ++ns_MainMenu.reportContentCount;
                reportPage = x;
                if (dontShowRepParamWindow == undefined || dontShowRepParamWindow == null || dontShowRepParamWindow == false) {
                    cont = "<div><a class='k-button k-button-icontext' style='top:40px; margin-top:-3px; position:relative;float:right;right:70px;' onclick=\"ns_MainMenu.mapLocalToGlobal('" + reportID + "','" + reportName + "');" + "ns_MainMenu.showReportParameterDialog(ns_Report.getReportPopupTemplate('" + reportID + "')" + ",'"
                        + reportName + "','"
                        + reportID + "')\"><span  class='k-icon k-i-funnel'  ></span>بارگذاری صفحه پارامتر ها</a>"
                        + reportPage + "</div>";
                }
                else {
                    cont = "<div>" + reportPage + "</div>";
                }
                var empty = "menu/empty";
                ns_MainMenu.$initialScope.location.path(url);
                if (!ns_MainMenu.$initialScope.$$phase)
                    ns_MainMenu.$initialScope.$apply();
                var tabTitle = reportName + "<span class='k-icon k-i-close' id='" + url + "' onclick='ns_MainMenu.tabStrip.remove($(this).closest(\"li\")); ns_MainMenu.removeTabItemFromSession($(this).attr(\"id\")); ' /> ";
                ns_MainMenu.tabStrip.append({
                    text: tabTitle,
                    encoded: false,
                    content: cont
                });
                var tbConfig = { TabName: tabTitle, Url: url, IsReport: true };
                ns_MainMenu.insertedTabs.push(tbConfig);
                ns_MainMenu.isComeFromRemoveOp = true;
                ns_MainMenu.tabStrip.select((ns_MainMenu.tabStrip.tabGroup.children("li").length - 1));
                ns_MainMenu.isComeFromRemoveOp = false;
            })
                .fail(function (e) {
                alert(e);
            });
        }
    }
    ns_MainMenu.appendTabForReport = appendTabForReport;
    function appendTabForReportAsGrid(url, reportName) {
        var alreadyInserted = false;
        checkIfCurrentDetailReportAlreadyExistClosePrevious(url);
        var tbConfig = { TabName: reportName, Url: url, IsReport: true };
        ns_MainMenu.insertedTabs.push(tbConfig);
        ns_MainMenu.isComeFromRemoveOp = true;
        ns_MainMenu.tabStrip.select((ns_MainMenu.tabStrip.tabGroup.children("li").length - 1));
        ns_MainMenu.isComeFromRemoveOp = false;
    }
    ns_MainMenu.appendTabForReportAsGrid = appendTabForReportAsGrid;
    function checkIfCurrentDetailReportAlreadyExistClosePrevious(url) {
        ns_MainMenu.tabStrip.tabGroup.children("li").each(function (e) {
            if ($($(this)[0].innerHTML).attr('data-content-url').split('?')[0].indexOf(url.split('?')[0]) != -1) {
                ns_MainMenu.tabStrip.remove($(this));
            }
        });
    }
    ns_MainMenu.checkIfCurrentDetailReportAlreadyExistClosePrevious = checkIfCurrentDetailReportAlreadyExistClosePrevious;
    function mapLocalToGlobal(repId, repName) {
        ns_MainMenu.reportId = repId;
        ns_MainMenu.reportName = repName;
    }
    ns_MainMenu.mapLocalToGlobal = mapLocalToGlobal;
    function closeAllOpenTabStrips() {
        var tbStrp = $('#tabStripNavigator').data('kendoTabStrip');
        if (tbStrp.tabGroup.find('li').length != 0) {
            ns_MainMenu.insertedTabs.forEach(function (tabItem) {
                var element = document.getElementById(tabItem.Url.hashCode());
                $(element).detach(),
                    $(element).remove();
            });
            ns_MainMenu.tabStrip.remove(tbStrp.tabGroup.find('li'));
            ns_MainMenu.insertedTabs = [];
        }
    }
    ns_MainMenu.closeAllOpenTabStrips = closeAllOpenTabStrips;
})(ns_MainMenu || (ns_MainMenu = {}));
function signOut() {
    $.ajax({
        url: getAreaApiUrl("AccountApi"),
        cache: false,
        type: 'PUT',
        statusCode: { 201: function () { window.status = "ok"; } },
        success: function (data) {
            if (data) {
                showLoginWindow();
            }
            else {
            }
        }
    }).fail(function (xhr, textStatus, err) { alert(err); });
}
