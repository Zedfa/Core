function OnEditCompanyRole(e) {
    if (!(e.model.isNew())) {
        $("#CompanyInRole").attr("disabled", "disabled");
        $("#btnRP_CompanyInRole").attr("disabled", "disabled");
    }
}
