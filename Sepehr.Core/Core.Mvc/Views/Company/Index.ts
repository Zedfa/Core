///// Top Scripts
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/kendo/kendo.all.d.ts" />

interface GridCompanyModel extends kendo.data.Model {
    CompanyIsValidNationalCode: any;
   
}

interface GridCompanyEditEvent extends kendo.ui.GridEditEvent {
    model?: GridCompanyModel;
    container: JQuery;
}
var Company = {};
var wndCompanyHelp = "";

function onEditGrdCompany(e:GridCompanyEditEvent) {
    e.model.CompanyIsValidNationalCode = true;
    $("#CompanyIsValidNationalCode").val("true");
}


