/// <reference path="gridsearch.ts" />

//var ns_Grid = ns_Grid || {};
//ns_Grid.GridOperations = ns_Grid.GridOperations || {};

module ns_Grid {
    export module GridOperations {

        export function onEditWindowOpen(e: kendo.ui.WindowEvent): void {
             //e.sender.element.find('k-edit-form-container').append('<div style="display:block;bottom:0;"> action section</div>')
             //e.sender.element.find('.k-grid-update').css("margin-left" , "65px" );
             //e.sender.element.find('.k-grid-cancel').css("margin-left", "-130px");
        }

        export function supressAnyKeyEvent(Id: string): void {
            $("#" + Id).data('kendoGrid').table.off("keydown");
            $("#" + Id).data('kendoGrid').table.off("keypress");
            $("#" + Id).data('kendoGrid').table.off("keyup");
        }
         
        export function onCellDblClick(onDoubleClick: Function, Id: string) {
            $("#" + Id).children(".k-grid-content").find("table > tbody > tr > td[role='gridcell']").off("dblclick");
            $("#" + Id).children(".k-grid-content").find("table > tbody > tr > td[role='gridcell']").on("dblclick", (e: JQueryEventObject) => { eval("" + onDoubleClick + ""); });
        }
         
        export function onRowDblClick(onDoublClick: string, Id: string) {
           
            $("#" + Id).children(".k-grid-content").find("table > tbody > tr[role='row']").off("dblclick");

            $("#" + Id).children(".k-grid-content").find("table > tbody > tr[role='row']").on("dblclick",(e: JQueryEventObject) => {  eval(onDoublClick + "(e)"); });
        }

        export function onInitCallback(callback: string, Id: string) {
            
            eval(callback + "(" + Id + ")");

        }
         
        export function getUpdateButton(editText: string): string {
            return "<span class=\"k-icon k-update\" ></span>" + editText;
        }
         
        export function getCancelButton(cancelText): string {
            return "<span class=\"k-icon k-cancel\" ></span>" + cancelText;
        }
         
        export function doEditOperation(opType: string, gd: kendo.ui.Grid): void {
            if (opType == 'a') gd.addRow(); else if (opType == 'e') { var cR = gd.select().closest('tr'); if (gd.dataItem(cR)) gd.editRow(cR); }
        }
         
        export function getInitialFilterOfGrid(gId: string): any {
            return ns_Search.getInitialFilterOfGrid(gId);
        }

        export function ifRefreshCanApplyAccordingToFilter(gId: string): boolean {
            if (ns_Search.ifGridHasInitialFilterRule(gId))
                return false;
            else
                return true;
        }

        export function doWithInitialOrClearFilter(gId: string, hasInitial: boolean) {
            var grd = $('#' + gId).data('kendoGrid');
            if (grd.dataSource.transport.cache._store) grd.dataSource.transport.cache._store = {};
            eval(gId + "_gridInitialized=false;");
            if (!hasInitial) grd.dataSource.filter('');
            else grd.dataSource.filter(getInitialFilterOfGrid(gId));
        }


        export function gridAdTemplate(tempAddress: string, viewModelTypeFullName: string, opType: string, gId: string): void {
            var that = this;
            var grd = $("#" + gId + "").data('kendoGrid'),
                doAj = true;

            if (!grd.options.editable.template) {
                if (opType == 'e') {
                    if (!grd.dataItem(grd.select().closest('tr'))) doAj = false;
                }
                if (doAj) {
                    $.ajax({
                        url: getAreaUrl("Template", "Get"),
                        contentType: "application/json; charset=utf-8",
                        type: "GET",
                        data: { templateUrl: tempAddress, viewModelFullName: viewModelTypeFullName },
                        error: (data: any) => {
                        } ,
                        success: (data: any, textStatus: string, jqXHR: JQueryXHR) => {
                            grd.options.editable.template = data; GridOperations.doEditOperation(opType, grd);
                        }
                    });     
                }
            }
            else
                GridOperations.doEditOperation(opType, grd);
        }

        export function gridRemovalTemplate(gId:string, removalMsg:string):string {
            var okButton = "<a id=\"wm_" + gId + "_btn_yes\" style=\"text-align:center !important;  \" class=\"k-button k-button-icontext\"  ><span class=\"k-icon k-i-tick\" ></span>بله</a>"
                , cancelButton = "<a id=\"wm_" + gId + "_btn_no\" style=\"text-align:center !important;  \" class=\"k-button k-button-icontext\"  ><span class=\"k-icon k-i-cancel\" ></span>خیر</a>"
                , template = "<span id=\"" + gId + "_delete_confirm_message\">" +
                "<div id=\"" + "wm_" + gId + "\">" +
                "<div style=\"display: block; height: 35px; text-align: center; line-height: 30px;\">" +
                "<span>آیا از حذف اطمینان دارید؟</span>" +
                "</div>" +
                "<div style=\"display: block; text-align: center;\">" +
                okButton + "&nbsp;  &nbsp;  &nbsp;" +
                cancelButton +
                "</div>" +
                "</div>" +
                "</span>";
            return template;
        }

        export function onEditEventHandler(e, grdWindowTitleOnEdit, grdWindowTitleOnAdd) {
            var dialogTitle = grdWindowTitleOnEdit;
            e.container.find('.k-grid-update')[0].innerHTML = getUpdateButton('ذخیره');
            e.container.find('.k-grid-cancel')[0].innerHTML = getCancelButton('انصراف');
            if (e.model.isNew()) { dialogTitle = 'جدید'; } else { dialogTitle = 'ویرایش'; }
            e.container.kendoWindow('title', dialogTitle);
        }

        export function onDataBoundCustomCode(customDataBoundCode, args) {
            if (typeof customDataBoundCode == 'function' && args && typeof args == 'object') {
                customDataBoundCode(args);
            }
            else if (typeof customDataBoundCode == 'function' && !args) {
                customDataBoundCode;
            }
        }

    }

}






 