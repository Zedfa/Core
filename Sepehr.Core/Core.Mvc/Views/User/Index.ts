/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/kendo/kendo.all.d.ts" />
/// <reference path="../../scripts/tools/gridsearch.ts" />

interface GridUserModel extends kendo.data.Model {
    Active: boolean;
}

interface GridUserRoleModel extends kendo.data.Model {
    OldSelectedRoleId: any;
    UserId: any;
}

interface GridUserEditEvent extends kendo.ui.GridEditEvent {
    model?: GridUserModel;
    container: JQuery;
}

interface GridUserRoleEditEvent extends kendo.ui.GridEditEvent {
    model?: GridUserRoleModel;
    container: JQuery;
    sender: CustomGrid;
}

interface CustomObservable extends kendo.data.ObservableObject {
    RoleId: any;
}

interface CustomGrid extends kendo.ui.Grid {
    dataItem(row: any): CustomObservable;
}



var userName = '';
var selectedRowId: any = 0;
var colIndex;
var currentRowIndex;
var selectedUserId = -1;

function onEditUser(e: any): void {
    $("#CompanyOfHeadUser").removeAttr("data-val-required");
    $("#RoleChoosInHeadUser").removeAttr("data-val-required");
    $("#CompanyName").attr("data-val-required", "واحد سازمانی را انتخاب نمائید");

    var windowOptions: kendo.ui.WindowOptions = null;
    if (e.model.isNew()) {
        $('#Active').prop('checked', true);
        e.model.Active = true;
        e.container.kendoWindow("title", "تعریف کاربر");
    } else {

        e.container.kendoWindow("title", "ویرایش کاربر");
        $("#Password").attr("readonly", "true");
        $("#ConfirmPassword").attr("readonly", "true");
        $("#passwordPart").hide();

    }

    $('#UserName').keypress((e: JQueryEventObject) => {

        if (e.which == 64) { /// '@' keyKode is 64
            e.preventDefault();
        }
    });
}

function onEditGridUserRole(e: any): void {

    if (e.model.isNew()) {

        e.container.kendoWindow("title", "اختصاص نقش");
    } else {

        var griduserRole = e.sender;
        var currentSelectionURole = griduserRole.select();
        colIndex = currentSelectionURole.index();
        if (colIndex == -1)
            return;
        var currentUserRoleRow = currentSelectionURole.closest("tr");
        var dataItem = griduserRole.dataItem(currentUserRoleRow);
        e.model.OldSelectedRoleId = dataItem.RoleId;

    }
    var grid = $("#gridUser").data("kendoGrid");
    var currentSelection = grid.select();
    var currentRow = currentSelection.closest("tr");
    var selectedItem = grid.dataItem(currentRow);
    if (selectedItem) {
        e.model.UserId = selectedItem.Id;
        $("#name").html(userName);
    }
}
var userId = 0;

function onUserChange(e) {
    
    var grid = e.sender;
    var currentSelection = grid.select();
    colIndex = currentSelection.index();
    if (colIndex == -1)
        return;
    var currentRow = currentSelection.closest("tr");
    var dataItem = grid.dataItem(currentRow);
    userId = dataItem.id;
    selectedRowId = dataItem.uid;
    if (selectedUserId == dataItem.Id)
        return;
    currentRowIndex = currentSelection.closest("tr").index();
    selectedUserId = dataItem.Id;
    userName = dataItem.FName + ' ' + dataItem.LName;
    if (userName != '' && selectedUserId != -1) {

        var initFilterItem = { field: "UserId", operator: "eq", value: selectedUserId };
        $("#grdUserRole").data("kendoGrid").dataSource.filter({ field: "UserId", operator: "eq", value: dataItem.Id });
        ns_Search.setGridInitialFilterRule("#grdUserRole", initFilterItem);
        $("#grdUserRole").css("display", "block");
    }
}
