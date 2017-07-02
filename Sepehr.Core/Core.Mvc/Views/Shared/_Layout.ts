/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/kendo/kendo.all.d.ts" />
/// <reference path="../../scripts/tools/generaltools.ts" />
/// <reference path="../../scripts/tools/dialogbox.ts" />


$(function () {

    kendo.culture("fa-IR");

    var hdn = $("#hdnError");
    if (hdn.val()) {
        DialogBox.ShowError(hdn.val(), false , null , null);
        hdn.val("");
    }

    var locationSpinner;
    var jqXhr:JQuery = $(this).ajaxStart(() => {

        if (///(args.currentTarget.activeElement != "undefined") &&
            $("#dw_loginDiv").length > 0   )
        {
            locationSpinner = "dw_loginDiv";
        }

        else {
            locationSpinner = "spinnerPart";
        }
        kendo.ui.progress($("#" + locationSpinner), true);
      
    });
    $(this).ajaxStop(() => {
        kendo.ui.progress($("#" + locationSpinner), false);
        return null;
    });

    $(this).ajaxError( (event, request, settings, exception) => {
        if (request.status == 440 || request.status == 403) {
            showLoginWindow();
            DialogBox.ShowError("حساب شما منقضی شده است. لطفا دوباره وارد شوید", 200, 100, true);

        }
        else if (request.status == 401) {
            DialogBox.ShowError("شما مجوز دسترسی انجام این عملیات را ندارید", 200, 100, true);
        }
        else if (request.responseText) {

            try {
                var errorBody = $.parseJSON(request.responseText);
                if (typeof (errorBody.ModelState) != "undefined" && typeof (errorBody.ModelState.isHandled) == "undefined")
                    DialogBox.ShowError(request.responseText, 250, 100, true);
                else if (typeof (errorBody.ModelState) != "undefined") {
                    if (errorBody.ModelState.isHandled.length > 0) {
                        for (var i = 0; i < errorBody.ModelState.isHandled.length; i++) {
                            if (errorBody.ModelState.isHandled[i] != "true") {
                                DialogBox.ShowError(request.responseText, null, null, false);
                            }
                        }
                    }
                    else if (errorBody.ModelState.isHandled != "true") {
                        DialogBox.ShowError(request.responseText, null, null, false);

                    }
                }
                else if (typeof (errorBody.ExceptionMessage) != "undefined")
                    DialogBox.ShowError(errorBody.ExceptionMessage, 300, 200, true);
                else if (typeof (errorBody.ModelState) == "undefined")
                    DialogBox.ShowError(request.responseText, null, null, errorBody.IsRTL);
            } catch (e) {
                DialogBox.ShowError(request.responseText, null, null, true);
            }

        }
    });

    $("body").keypress( (e:any) => {
        if (e.keyCode === 13) {
           
            if (e.target.type != "button" && e.target.type != "submit") {
                var isInTab = $(e.target).data("kendoTabStrip") != undefined || $(e.target).closest(".k-tabstrip").length > 0 ? true : false;
                defaultButtonCall(e.target, isInTab);
            }
        }
    });
   
    var tooltipAnimation: kendo.ui.TooltipAnimation = { open: { duration: 5000 } };
    var tooltipOpt: kendo.ui.TooltipOptions = { duration: 5000, position: "top", width: 100, height: 40, content: $("#toolTipContainer").html()}
    $("#memberInfo").kendoTooltip(tooltipOpt);

});


function showChangePassWindow() {
    var parenTag = $("#main-content").attr("ng-controller", "coreLoginController");
    var container = $("<div id='dw_ChangePassDiv'>");
    parenTag.append(container);
    container.addClass("k-block k-info-colored");
    var win = container.kendoWindow({
        actions: ["Close"],
        width: 300,
        height: 250,
        modal: true,
        title: "تغییر کلمه عبور",
        visible: false,
        resizable: true,
        scrollable: false,
        content: getAreaUrl("Account", "ChangePassword")
    }).data("kendoWindow");
    win.center().open();
}

function showLoginWindow():void {
    var parenTag = $("#main-content");
    var container = $("<div id='dw_loginDiv'>");
    parenTag.append(container);
    container.addClass("k-block k-info-colored");
    var win = container.kendoWindow({
        actions: [],
        width: 385,
        height: 318,
        modal: true,
        title: "ورود به سیستم",
        visible: false,
        resizable: true,
        scrollable: false,
        content: getAreaUrl("Account", "LogOn"),
        close: (e) => {
            e.preventDefault();
        }
    }).data("kendoWindow");
    win.center().open();

}

//function RenderMainMenu() {
//    var menuUrl = getAreaUrl("Home", "MainMenu");
//    $("#topMainMenu").load(menuUrl,  (e:JQueryEventObject) => {
//        if (status == "error") {
//            var msg = "Sorry but there was an error: ";
//            $("#error").html(msg);// + xhr.status + " " + xhr.statusText);
//        }
//    });

//}
