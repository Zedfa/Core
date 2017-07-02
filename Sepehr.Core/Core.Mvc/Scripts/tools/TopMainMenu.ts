/// <reference path="usertabcookie.ts" />
/// <reference path="generaltools.ts" />
/// <reference path="../../views/shared/_layout.ts" />

module ns_MainMenu {
    export var tabStrip = null;
    export var insertedTabs = [];
    export var tbConfig = null;
    export var isComeFromRemoveOp = false;
    export var isComeFromRfresh = false;
    export var reportUrlParams = '';
    export var reportName = '';
    export var itemText = '';
    export var reportContentCount = 0;//"url" : "Role"
    export var reportId;
    export var wndmodalAboutUs: kendo.ui.Window = null;
    export var tabAddedForReport = false;
    export var userName;
    export var hasFirstItemSelected = false;
    export var locationService;
    export var $scope;
    export var $rootScope;
    export var $initialScope;
    export var comeFromTabClick = true;
    export var isComFromMenuItemClick = false;
    export var isComeFromAddressBar = false;
    export var menuItems = [];



    export function menuItemSelected(e, url, text, queryString) {

        isComFromMenuItemClick = true;
        var selectedItemUrl = e && !url ? $(e.item.firstChild).find("span").attr("id") : url;//$($(e.item.innerHTML)[0].innerHTML).attr('id');

        var tabCaption = e && !text ? $(e.item.firstChild).find("span").text() : text;
        ns_MainMenu.comeFromTabClick = true;

        if (selectedItemUrl) {

            var uniqueName = selectedItemUrl.split('#')[0];
            selectedItemUrl = selectedItemUrl.split('#')[1];


            if (selectedItemUrl == 'CoreAboutUs') {
                wndmodalAboutUs.center();
                wndmodalAboutUs.open();
                return;
            }

            if (selectedItemUrl) {

                itemText = e && !text ? $(e.item.firstChild).find("span").text() : text;//$($(e.item.innerHTML)[0].innerHTML).text();
                if (selectedItemUrl.indexOf('Report_') == 0) {
                    reportName = itemText;
                    reportId = selectedItemUrl;
                    var reportTemplate;//= ns_Report.getReportPopupTemplate(reportId)
                    if (!reportTemplate) {
                        $.get("Report/Index", { tempView: selectedItemUrl }).done(function (templ) {
                            var currentTemplate = templ.toString().replace(/#/gm, '\\#').replace(/<\/script>/gm, '<\\/script>');
                            // ns_Report.setReportPopupTemplate(reportId, currentTemplate);
                            showReportParameterDialog(currentTemplate, reportName, reportId);

                        }).fail(function (err) { });
                    }
                    else {
                        showReportParameterDialog(reportTemplate, reportName, reportId);
                    }
                }
                else {

                    makeProperTabDynamic(itemText, null, null, null, uniqueName, queryString);
                }
            }
        }
    }


    export function onError(e) {
    }


    export function onTabItemSelected(e, location, uniqueName) {

        var itemUrl = $(e.item).find("span.k-i-close").attr("id");//$(e.item.innerHTML).attr("data-content-url");
        if (itemUrl) {
            if (itemUrl.indexOf("Report") == -1 && !isComeFromRemoveOp) {
                if (!isComeFromRfresh) {
                    //Only when a remove operation is not done(equivalent to just a tab item selection occures)
                    reArrangeInsertedItems(location, uniqueName);
                    putTabInfoIntoLocalStorage(insertedTabs)
                }
                //At the end of any refreshment and selection.
                else if (isComeFromRfresh) {
                    if (insertedTabs.length == 1 && !hasFirstItemSelected) {
                        //ns_Cookie.setCookie(userName, insertedTabs, 1);
                    }
                    isComeFromRfresh = false;
                }
                hasFirstItemSelected = false;
            }
        }
    }
    //When inserted Array is included with both ordinary tabs and also report tabs,
    //and we dont want to store report Url address into the user cookie.(after insert and also after item removal)
    export function putTabInfoIntoLocalStorage(storingTabs) {
        var noneReportTabs = $.grep(storingTabs, function (tc: any) { return !tc.IsReport; });
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
    //This method is for rearranging the inserted tab items based on 
    //the field IsLastSelected.(just after the newly tab inserted)
    export function reArrangeInsertedItems(location, uniqueName) {

        var itemFoundIx = -1;
        for (var ix = 0; ix < insertedTabs.length; ix++) {
            if (insertedTabs[ix].Url == uniqueName) {
                itemFoundIx = ix;
                insertedTabs[itemFoundIx].IsLastSelected = true;
                if (!comeFromTabClick) {
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


    export function makeNodesNonLastSelected(itemFoundIx) {

        for (var ix = 0; ix < insertedTabs.length; ix++) {
            if (insertedTabs[ix].Url != insertedTabs[itemFoundIx].Url) {
                insertedTabs[ix].IsLastSelected = false;
            }
        }
    }
    export function onTabItemActivated(e) {
    }
    export function onTabItemContentLoad(e) {
    }
    export function onTabItemError(e) { }
    export function showReportParameterDialog(repParameterUrl: string, repName: string, repID: string, width?: number, height?: number) {
        var parentTag = $("#topMenu");
        var container = $("<div>");
        parentTag.append(container);
        var repParamDialogTemplate = <kendo.ui.WindowContent>{ url: repParameterUrl, data: null };
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
            refresh: (e) => {
                var inputVals;// = ns_Report.getReportValues(repID);
                if (inputVals) {
                    for (var item in inputVals) {

                        if (inputVals[item].dropdownlist) {
                            //It is worthy to remind that the kendo dropdown works
                            //just with selectedIndex property , in order to handle 
                            //the selection of the desired values. 
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

    export function makeProperTabDynamic(pageName: string, url: string, isLastSelected: boolean, index: number, menuUrl: string, queryString) {

        var alreadyInserted = false;
        var cUrl: string = null;

        if (!url) {
            cUrl = menuUrl;
            if (cUrl) {
                insertedTabs.forEach((tabItem) => {

                    if (tabItem.Url == menuUrl + queryString) {
                        alreadyInserted = true;

                    }
                    return;
                });
            }
            else return;
            if (!alreadyInserted) {
                appendTabToTabStrip(pageName, cUrl, queryString);
                if (!checkIfItemAlreadyExists(menuUrl + queryString)) {

                    var tbConfig = { TabName: pageName, Url: cUrl + queryString, IsLastSelected: true, wasTabsContentLoaded: true };
                    insertedTabs.push(tbConfig);
                    //In new Mode 
                    $.each(insertedTabs, function (key, val) {
                        if (key == insertedTabs.length - 1) {
                            return;
                        }
                        else {
                            val.IsLastSelected = false;
                        }
                    });
                    tabStrip.select((tabStrip.tabGroup.children("li").length - 1));
                }
            }
            else {
                tabStrip.select(getIndexOfAlreadyInsertedItem(menuUrl + queryString));
            }
        }
        else {

            appendTabToTabStrip(pageName, url, queryString);
            if (!checkIfItemAlreadyExists(url)) {
                var tbConfig = { TabName: pageName, Url: url + queryString, IsLastSelected: isLastSelected, wasTabsContentLoaded: true };
                insertedTabs.push(tbConfig);
                if (isLastSelected == true) {
                    tabStrip.select(index);
                }
            }
        }
    }
    export function appendTabToTabStrip(pageName, url, queryString) {

        var uniqueName = url + queryString;

        var tabTitle = pageName + "<span class='k-icon k-i-close' id='" + uniqueName + "' onclick=' ns_MainMenu.tabStrip.remove($(this).closest(\"li\")); ns_MainMenu.removeTabItemFromSession($(this).attr(\"id\")); ' /> "
        tabStrip.append({
            text: tabTitle,
            encoded: false
        });
    }
    //The array is being rearranged based on the Last Selected
    export function defineNextSelectedAfterRemoval(foundIndex) {

        if (foundIndex != -1) {
            var listLength = insertedTabs.length;
            if (insertedTabs[foundIndex].IsLastSelected) {
                if (listLength > 1) {
                    if (foundIndex == listLength - 1) {
                        insertedTabs[foundIndex - 1].IsLastSelected = true;
                    }
                    else {
                        insertedTabs[foundIndex + 1].IsLastSelected = true;
                    }

                }
            }
        }
    }
    export function getUrlFromAccessibleViewElementsByUniqueName(uniqueName) {
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
    export function removeAllTabs() {
       
        $.each(insertedTabs, function (key, val) {
            var element = document.getElementById(val.Url.hashCode());
            $(element).detach(),
                $(element).remove();
        });
        localStorage.removeItem("tabItems");
    }
    export function removeTabItemFromSession(pageUrl) {

        var alreadyInsertedIndex = getIndexOfAlreadyInsertedItem(pageUrl);
        if (alreadyInsertedIndex != -1 && insertedTabs[alreadyInsertedIndex].IsReport) {
            insertedTabs = $.grep(insertedTabs, function (tc) { return tc.Url != pageUrl; });
            var lastInsertedIndex = getLastInsertedItem();
            var itemUrl = insertedTabs[lastInsertedIndex].Url;
            tabStrip.select(lastInsertedIndex);
            var url = itemUrl
            ns_MainMenu.$initialScope.location.path(url);
            if (!ns_MainMenu.$initialScope.$$phase) ns_MainMenu.$initialScope.$apply();
        }
        else {
            ns_MainMenu.defineNextSelectedAfterRemoval(alreadyInsertedIndex);
            //The specified Url is going to be deleted from ns_MainMenu.insertedTabs.
            insertedTabs = $.grep(insertedTabs, function (tc) { return tc.Url != pageUrl; });
            if (insertedTabs.length == 0) {
                ns_MainMenu.$initialScope.location.path("/");
                if (!ns_MainMenu.$initialScope.$$phase) ns_MainMenu.$initialScope.$apply();

            }
            else if (insertedTabs.length > 1) {
                $.each(insertedTabs, function (key, val) {
                    if (val.IsLastSelected) {

                        var urlByUniqueName = getUrlFromAccessibleViewElementsByUniqueName(val.Url);
                        var url: any = urlByUniqueName;
                        ns_MainMenu.$initialScope.location.path(url);
                        if (!ns_MainMenu.$initialScope.$$phase) ns_MainMenu.$initialScope.$apply();

                        SelectProperTab(key);
                        return;
                    }
                });

            }
            else {

                var uniqueName: any = tabStrip.tabGroup.children("li").find("span.k-link > span").attr('id');
                var urlByUniqueName = getUrlFromAccessibleViewElementsByUniqueName(uniqueName);
                ns_MainMenu.$initialScope.location.path(urlByUniqueName);
                if (!ns_MainMenu.$initialScope.$$phase) ns_MainMenu.$initialScope.$apply();
                //just one or zero item exists in the array
                SelectProperTab(0);
                //ns_Cookie.setCookie(ns_MainMenu.userName, ns_MainMenu.insertedTabs, 1);
            }
            //May be there is at least one Report tab item exists in the array.
            putTabInfoIntoLocalStorage(insertedTabs);
        }

        //kendo.destroy($("#" + pageUrl + "Page"))

        var element = document.getElementById(pageUrl.hashCode());
        $(element).detach(),
            $(element).remove();
    }
    export function SelectProperTab(key) {
        isComeFromRemoveOp = true;
        tabStrip.select(key);
        isComeFromRemoveOp = false;
    }
    export function getLastInsertedItem() {

        var foundIndex = -1;
        for (var i = 0; i < insertedTabs.length; i++) {
            if (insertedTabs[i].IsLastSelected) {
                foundIndex = i;
                break;
            }
        }
        return foundIndex;
    }
    export function checkIfItemAlreadyExists(url) {

        var ifExists = false;
        for (var i = 0; i < insertedTabs.length; i++) {
            if (insertedTabs[i].Url == url) {
                ifExists = true;
                break;
            }
        }
        return ifExists;
    }
    export function getIndexOfAlreadyInsertedItem(url) {

        var foundIndex = -1;
        for (var i = 0; i < insertedTabs.length; i++) {
            if (insertedTabs[i].Url == url) {
                foundIndex = i;
                break;
            }
        }
        return foundIndex;
    }
    export function appendTabForReport(url, reportName, repUrlParams, illustrationType, dontShowRepParamWindow) {

        var alreadyInserted = false;
        var addr = 'Report/ApplyParamsLoadCorrespondentReport';
        reportUrlParams = repUrlParams;
        var cont = '';

        if (!alreadyInserted) {
            var reportPage = '';
            var reportID = url.indexOf('cshtml') != -1 ? url : (url + ".cshtml");
            $.post(addr, repUrlParams).done(function (x) {
                ++reportContentCount;
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
                if (!ns_MainMenu.$initialScope.$$phase) ns_MainMenu.$initialScope.$apply();
                var tabTitle = reportName + "<span class='k-icon k-i-close' id='" + url + "' onclick='ns_MainMenu.tabStrip.remove($(this).closest(\"li\")); ns_MainMenu.removeTabItemFromSession($(this).attr(\"id\")); ' /> ";
                tabStrip.append({
                    text: tabTitle,
                    encoded: false,
                    content: cont
                });

                var tbConfig = { TabName: tabTitle, Url: url, IsReport: true };
                insertedTabs.push(tbConfig);
                isComeFromRemoveOp = true;
                tabStrip.select((tabStrip.tabGroup.children("li").length - 1));
                isComeFromRemoveOp = false;
            })
                .fail(function (e) {
                    alert(e);
                });
        }
    }
    export function appendTabForReportAsGrid(url, reportName) {

        var alreadyInserted = false;
        checkIfCurrentDetailReportAlreadyExistClosePrevious(url);
        //appendTabToTabStrip(reportName, url);
        var tbConfig = { TabName: reportName, Url: url, IsReport: true };
        insertedTabs.push(tbConfig);
        isComeFromRemoveOp = true;
        tabStrip.select((tabStrip.tabGroup.children("li").length - 1));
        isComeFromRemoveOp = false;

    }
    export function checkIfCurrentDetailReportAlreadyExistClosePrevious(url) {
        tabStrip.tabGroup.children("li").each(function (e) {
            if ($($(this)[0].innerHTML).attr('data-content-url').split('?')[0].indexOf(url.split('?')[0]) != -1) {
                tabStrip.remove($(this));
            }
        });
    }
    export function mapLocalToGlobal(repId, repName) {
        reportId = repId;
        reportName = repName;
    }
    export function closeAllOpenTabStrips() {

        var tbStrp = $('#tabStripNavigator').data('kendoTabStrip');

        if (tbStrp.tabGroup.find('li').length != 0) {
            insertedTabs.forEach((tabItem) => {

                var element = document.getElementById(tabItem.Url.hashCode());
                $(element).detach(),
                    $(element).remove();

            });
            tabStrip.remove(tbStrp.tabGroup.find('li'));
            insertedTabs = [];
        }
    }
}

function signOut() {
    $.ajax({
        url: getAreaApiUrl("AccountApi"),
        cache: false,
        type: 'PUT',
        statusCode: { 201: function () { window.status = "ok"; } },
        success: (data) => {
            if (data) {
                showLoginWindow();
            }
            else {
            }
        }
    }).fail(function (xhr, textStatus, err) { alert(err); });
}


