


function OnEditCompanyRole(e:kendo.ui.GridEditEvent):void {
    if (!(e.model.isNew())) {
        $("#CompanyInRole").attr("disabled", "disabled");
        $("#btnRP_CompanyInRole").attr("disabled", "disabled");
    }
}