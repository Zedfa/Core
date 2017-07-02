/// <reference path="../custom-types/browsers.ts" />
/// <reference path="../customtypescriptextensions/windowextension.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/kendo/kendo.all.d.ts" />

function setMultiselect(bindingName: string, displayName: string, valueName: string, displayObject: any[], valueObject: any[]): void {
    var multiSelect: any = $("#" + bindingName).data("kendoMultiSelect");

    var selectedData = [];
    var selectedValues = [];

    for (var i = 0; i < valueObject.length; i++) {

        var data: JSON = JSON.parse('{"' + displayName + '" :"' + displayObject[i] + '", "' + valueName + '":' + valueObject[i] + '}');

        selectedData.push(data);
        selectedValues.push(selectedData[i][valueName]);
    }
    multiSelect.dataSource.data(selectedData);
    multiSelect.value(selectedValues);

    multiSelect.trigger("change");
}

function clearMultiSelect(bindingName: string): void {
    $("#" + bindingName).data("kendoMultiSelect").value([]);
}

function loadTreeView(id: string, dataTextField: string, url: string, funcName: string, dataKeyName: string, filterObject: kendo.data.DataSourceFilter): void {
    dataKeyName = (dataKeyName == "" || dataKeyName === undefined) ? "Id" : dataKeyName;

    var tempJQ = $("#" + id).data("kendoTreeView");


    var tempDS: kendo.data.HierarchicalDataSource = new kendo.data.HierarchicalDataSource({
        transport: {
            read: {
                url: url,
                data: eval(funcName),
                dataType: "json"
            }
        },

        schema: {
            model: { id: dataKeyName, hasChildren: "hasChildren", Title: "Title" }
        },

        serverFiltering: true
    });

    if (tempJQ === undefined || tempJQ === null) {

        tempJQ = $("#" + id).kendoTreeView({

            dataTextField: dataTextField,

            dataSource: tempDS

        }).data("kendoTreeView");
    }

    else {
        tempJQ.dataSource.filter({});
    }

}

function SetDropDown(bindingName: string, displayName: string, valueName: string, displayObject: string, valueObject: any[]): void {
    var drpSelect = $("#" + bindingName).data("kendoDropDownList");
    var selectedData = [];
    var selectedValues = [];

    for (var i = 0; i < valueObject.length; i++) {

        var data = JSON.parse('{"' + displayName + '" :"' + displayObject[i] + '", "' + valueName + '":"' + valueObject[i] + '"}');

        selectedData.push(data);
        selectedValues.push(selectedData[i][valueName]);
    }
    drpSelect.dataSource.data(selectedData);

    drpSelect.trigger("change");

}

function ClearDropDown(bindingName: string): void {
    $("#" + bindingName).data("kendoDropDownList").value([]);
}


function DisableElement(elementName: string): void {
    $("#" + elementName).attr("Style", "opacity:0.4");
    $("#" + elementName).attr("disabled", "disabled");
}

function enableElement(elementName: string): void {
    $("#" + elementName).attr("Style", "opacity:1");
    $("#" + elementName).removeAttr("disabled");
}

function strenghtChecker(element: HTMLElement): void {
    var value: string = $(element).val();

    var pattern = /[a-zA-Z]+[0-9]|[0-9]+[a-zA-Z]/;

    var stateElement: JQuery = $(element).next("span");

    stateElement.removeClass();

    stateElement.addClass("k-tooltip validation-checker");

    if (value.length > 5 && pattern.test(value)) {

        stateElement.addClass("input-validation-confirm field-validation-confirm").text("خوب");
    }
    else {

        stateElement.addClass("input-validation-error field-validation-error").text("ضعیف");
    }

    stateElement.show();
}

function getInternetExplorerVersion(): number {
    var rv: number = -1;
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua: string = navigator.userAgent;
        var re: RegExp = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}

function defaultButtonCall(target: HTMLElement, isInTab: boolean): void {
    var button: JQuery;
    if (isInTab) {
        button = $("body").find(".k-content.k-state-active[aria-expanded='true'] .k-button[default]:first").length > 0 ?
            $("body").find(".k-content.k-state-active[aria-expanded='true'] .k-button[default]:first") :
            $("body").find(".k-content.k-state-active[aria-expanded='true'] input[type=button][default]:first");
    }
    else {
        button = $(".k-button[default]:last");
    }
    button.click();
}


function hideChecker(element: JQuery): void {
    element.hide();
}


function RemoveGridFilter(gridName: string): void {
    var grdrfilter = $("#" + gridName).data("kendoGrid");
    var $filter = new Array();
    grdrfilter.dataSource.filter($filter);
}


function addFiterToGrid(filter: string, container: string, operator: string): any[] {
    var op = (typeof operator == "string" ? operator : "eq");
    var filters = [];
    for (var i = 0; i < filter.length; i++) {
        var val = $("#" + container + " input[id=" + filter[i] + "]").val();
        filters.push({ field: filter[i], operator: op, value: val });
    }
    return filters;
}

function getAreaApiUrl(controller: string, area: string = 'core'): string {
    return "api" + "/" + area + "/" + controller;
}

function getAreaUrl(controller: string, action: string, area: string = '/core') {

    return area + "/" + controller + "/" + action;
}


function detectBrowser(): AgentInfo {

    var userAgent = window.navigator.userAgent;

    var browsers = new BrowserTypes();
    var devices = new DeviceTypes();
    var agentInfo = new AgentInfo();


    for (var key in browsers) {
        if (browsers[key].test(userAgent)) {
            agentInfo.browser = key;
            break;
        }
    };

    for (var key in devices) {
        if (devices[key].test(userAgent)) {
            agentInfo.device = key;
            break;
        }
    };

    return agentInfo;
}
function hideWaitingSign(selector, selectorForContent): void {

    if ($(selector).length > 0) {
        $(selectorForContent).hide();
        kendo.ui.progress($(selector), false);
        if ($(".k-loading-mask").length == 0) {
            $('html').css("overflow", "initial");
            $('html').css("margin-right", "0");
            //$('html,body').animate({ scrollTop: 0 }, 'slow');
        }
    }
}
function showWaitingSign(selector, selectorForContent): void {

    if ($(selector).length > 0) {
        $(selectorForContent).show();
        kendo.ui.progress($(selector), true);
        $('html').css("overflow", "hidden");
        $('html').css("margin-right", "17px");
    }

}

function CallServerMethodSync(url: string, data: Object, verb: string = "POST", isJsonRequest: boolean = true): XMLHttpRequest {
    var xhr = new XMLHttpRequest();
    xhr.open(verb, url, false);
    if (isJsonRequest) {
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    }
    if (data) {
        xhr.send(JSON.stringify(data));
    }
    else {
        xhr.send();
    }
    return xhr;
}
function isEqual(object1: any, object2: any): boolean {

    return JSON.stringify(object1) == JSON.stringify(object2);
}
function clone(object: JSON): JSON {

    return JSON.parse(JSON.stringify(object));
}
function getCookieByKey(key: string): string {
    var cookiesArr = document.cookie.split(";"),
        result = "";
    $.each(cookiesArr, (index, cookie) => {

        var keyValue = $.trim(cookie).split("="),
            currentKey = keyValue[0],
            currentValue = keyValue[1];

        if (currentKey == key) {
            result = currentValue;
            return;
        }

    });
    return result;
}
