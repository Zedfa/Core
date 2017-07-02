/// <reference path="../../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../../scripts/typings/kendo/kendo.all.d.ts" />

interface CustomGrdCompanyModel extends kendo.data.Model {
    CompanyActive: any;
    CompanyIsValidNationalCode: any;
}

interface CustomGrdCompanySaveEvent extends kendo.ui.GridSaveEvent {
    model: CustomGrdCompanyModel
}

var validationMessageTmpl = kendo.template($("#message").html());

$("#CompanyNationalCode").blur((e: JQueryEventObject) => {

    //var validateCompanyNationalCode = checkCompanyNationalCode();


    //if (!validateCompanyNationalCode) {

    //    showMessage($("#divCompanyNationalCode"), "CompanyNationalCode", "کد ملی معتبر را وارد نمائید");
    //    $("#CompanyIsValidNationalCode").val("false");

    //}
    //else {
    $("#CompanyIsValidNationalCode").val("true");
    var validatorNationalCode = $("#divCompanyNationalCode").kendoValidator().data("kendoValidator");
    validatorNationalCode.hideMessages();


    //}
});

function checkCompanyNationalCode(): boolean {
    var mc;
    mc = $("#CompanyNationalCode").val();
    if (mc == "")
        return true;
    if (mc.length == 10) {
        if (mc == '1111111111' || mc == '0000000000' || mc == '2222222222' || mc == '3333333333' || mc == '4444444444' || mc == '5555555555' || mc == '6666666666' || mc == '7777777777' || mc == '8888888888' || mc == '9999999999') {
            return false;
        }
        var c = parseInt(mc.charAt(9));
        var n = parseInt(mc.charAt(0)) * 10 + parseInt(mc.charAt(1)) * 9 + parseInt(mc.charAt(2)) * 8 + parseInt(mc.charAt(3)) * 7 + parseInt(mc.charAt(4)) * 6 + parseInt(mc.charAt(5)) * 5 + parseInt(mc.charAt(6)) * 4 + parseInt(mc.charAt(7)) * 3 + parseInt(mc.charAt(8)) * 2;
        var r = (n - n / 11) * 11;
        if ((r == 0 && r == c) || (r == 1 && c == 1) || (r > 1 && c == 11 - r)) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
}

function showMessage(container: JQuery, name: string, errors: string): void {
    container.find("[data-valmsg-for=" + name + "],[data-val-msg-for=" + name + "]")
        .replaceWith(validationMessageTmpl({ field: name, message: errors }));
}

function onSaveGrdCompany(e: CustomGrdCompanySaveEvent): void {
    e.model.CompanyActive = $("#CompanyActive").prop('checked');
    var val = $("#CompanyIsValidNationalCode").val();
    e.model.CompanyIsValidNationalCode = val;
}

