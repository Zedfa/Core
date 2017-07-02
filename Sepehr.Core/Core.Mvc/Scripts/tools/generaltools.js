function setMultiselect(bindingName, displayName, valueName, displayObject, valueObject) {
    var multiSelect = $("#" + bindingName).data("kendoMultiSelect");
    var selectedData = [];
    var selectedValues = [];
    for (var i = 0; i < valueObject.length; i++) {
        var data = JSON.parse('{"' + displayName + '" :"' + displayObject[i] + '", "' + valueName + '":' + valueObject[i] + '}');
        selectedData.push(data);
        selectedValues.push(selectedData[i][valueName]);
    }
    multiSelect.dataSource.data(selectedData);
    multiSelect.value(selectedValues);
    multiSelect.trigger("change");
}
function clearMultiSelect(bindingName) {
    $("#" + bindingName).data("kendoMultiSelect").value([]);
}
function loadTreeView(id, dataTextField, url, funcName, dataKeyName, filterObject) {
    dataKeyName = (dataKeyName == "" || dataKeyName === undefined) ? "Id" : dataKeyName;
    var tempJQ = $("#" + id).data("kendoTreeView");
    var tempDS = new kendo.data.HierarchicalDataSource({
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
function SetDropDown(bindingName, displayName, valueName, displayObject, valueObject) {
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
function ClearDropDown(bindingName) {
    $("#" + bindingName).data("kendoDropDownList").value([]);
}
function DisableElement(elementName) {
    $("#" + elementName).attr("Style", "opacity:0.4");
    $("#" + elementName).attr("disabled", "disabled");
}
function enableElement(elementName) {
    $("#" + elementName).attr("Style", "opacity:1");
    $("#" + elementName).removeAttr("disabled");
}
function strenghtChecker(element) {
    var value = $(element).val();
    var pattern = /[a-zA-Z]+[0-9]|[0-9]+[a-zA-Z]/;
    var stateElement = $(element).next("span");
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
function getInternetExplorerVersion() {
    var rv = -1;
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}
function defaultButtonCall(target, isInTab) {
    var button;
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
function hideChecker(element) {
    element.hide();
}
function RemoveGridFilter(gridName) {
    var grdrfilter = $("#" + gridName).data("kendoGrid");
    var $filter = new Array();
    grdrfilter.dataSource.filter($filter);
}
function addFiterToGrid(filter, container, operator) {
    var op = (typeof operator == "string" ? operator : "eq");
    var filters = [];
    for (var i = 0; i < filter.length; i++) {
        var val = $("#" + container + " input[id=" + filter[i] + "]").val();
        filters.push({ field: filter[i], operator: op, value: val });
    }
    return filters;
}
function getAreaApiUrl(controller, area) {
    if (area === void 0) { area = 'core'; }
    return "api" + "/" + area + "/" + controller;
}
function getAreaUrl(controller, action, area) {
    if (area === void 0) { area = '/core'; }
    return area + "/" + controller + "/" + action;
}
function detectBrowser() {
    var userAgent = window.navigator.userAgent;
    var browsers = new BrowserTypes();
    for (var key in browsers) {
        if (browsers[key].test(userAgent)) {
            return key;
        }
    }
    ;
    return undefined;
}
function hideWaitingSign(selector, selectorForContent) {
    if ($(selector).length > 0) {
        $(selectorForContent).hide();
        kendo.ui.progress($(selector), false);
        if ($(".k-loading-mask").length == 0) {
            $('html').css("overflow", "initial");
            $('html').css("margin-right", "0");
        }
    }
}
function showWaitingSign(selector, selectorForContent) {
    if ($(selector).length > 0) {
        $(selectorForContent).show();
        kendo.ui.progress($(selector), true);
        $('html').css("overflow", "hidden");
        $('html').css("margin-right", "17px");
    }
}
function CallServerMethodSync(url, data, verb, isJsonRequest) {
    if (verb === void 0) { verb = "POST"; }
    if (isJsonRequest === void 0) { isJsonRequest = true; }
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
function isEqual(object1, object2) {
    return JSON.stringify(object1) == JSON.stringify(object2);
}
function clone(object) {
    return JSON.parse(JSON.stringify(object));
}
function getCookieByKey(key) {
    var cookiesArr = document.cookie.split(";"), result = "";
    $.each(cookiesArr, function (index, cookie) {
        var keyValue = $.trim(cookie).split("="), currentKey = keyValue[0], currentValue = keyValue[1];
        if (currentKey == key) {
            result = currentValue;
            return;
        }
    });
    return result;
}
