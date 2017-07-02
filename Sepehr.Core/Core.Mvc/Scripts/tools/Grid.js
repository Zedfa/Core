var ns_Grid;
(function (ns_Grid) {
    var GridOperations;
    (function (GridOperations) {
        function onEditWindowOpen(e) {
        }
        GridOperations.onEditWindowOpen = onEditWindowOpen;
        function supressAnyKeyEvent(Id) {
            $("#" + Id).data('kendoGrid').table.off("keydown");
            $("#" + Id).data('kendoGrid').table.off("keypress");
            $("#" + Id).data('kendoGrid').table.off("keyup");
        }
        GridOperations.supressAnyKeyEvent = supressAnyKeyEvent;
        function onCellDblClick(onDoubleClick, Id) {
            $("#" + Id).children(".k-grid-content").find("table > tbody > tr > td[role='gridcell']").off("dblclick");
            $("#" + Id).children(".k-grid-content").find("table > tbody > tr > td[role='gridcell']").on("dblclick", function (e) { eval("" + onDoubleClick + ""); });
        }
        GridOperations.onCellDblClick = onCellDblClick;
        function onRowDblClick(onDoublClick, Id) {
            $("#" + Id).children(".k-grid-content").find("table > tbody > tr[role='row']").off("dblclick");
            $("#" + Id).children(".k-grid-content").find("table > tbody > tr[role='row']").on("dblclick", function (e) { eval(onDoublClick + "(e)"); });
        }
        GridOperations.onRowDblClick = onRowDblClick;
        function onInitCallback(callback, Id) {
            eval(callback + "(" + Id + ")");
        }
        GridOperations.onInitCallback = onInitCallback;
        function getUpdateButton(editText) {
            return "<span class=\"k-icon k-update\" ></span>" + editText;
        }
        GridOperations.getUpdateButton = getUpdateButton;
        function getCancelButton(cancelText) {
            return "<span class=\"k-icon k-cancel\" ></span>" + cancelText;
        }
        GridOperations.getCancelButton = getCancelButton;
        function doEditOperation(opType, gd) {
            if (opType == 'a')
                gd.addRow();
            else if (opType == 'e') {
                var cR = gd.select().closest('tr');
                if (gd.dataItem(cR))
                    gd.editRow(cR);
            }
        }
        GridOperations.doEditOperation = doEditOperation;
        function getInitialFilterOfGrid(gId) {
            return ns_Search.getInitialFilterOfGrid(gId);
        }
        GridOperations.getInitialFilterOfGrid = getInitialFilterOfGrid;
        function ifRefreshCanApplyAccordingToFilter(gId) {
            if (ns_Search.ifGridHasInitialFilterRule(gId))
                return false;
            else
                return true;
        }
        GridOperations.ifRefreshCanApplyAccordingToFilter = ifRefreshCanApplyAccordingToFilter;
        function doWithInitialOrClearFilter(gId, hasInitial) {
            var grd = $('#' + gId).data('kendoGrid');
            if (grd.dataSource.transport.cache._store)
                grd.dataSource.transport.cache._store = {};
            eval(gId + "_gridInitialized=false;");
            if (!hasInitial)
                grd.dataSource.filter('');
            else
                grd.dataSource.filter(getInitialFilterOfGrid(gId));
        }
        GridOperations.doWithInitialOrClearFilter = doWithInitialOrClearFilter;
        function gridAdTemplate(tempAddress, viewModelTypeFullName, opType, gId) {
            var that = this;
            var grd = $("#" + gId + "").data('kendoGrid'), doAj = true;
            if (!grd.options.editable.template) {
                if (opType == 'e') {
                    if (!grd.dataItem(grd.select().closest('tr')))
                        doAj = false;
                }
                if (doAj) {
                    $.ajax({
                        url: getAreaUrl("Template", "Get"),
                        contentType: "application/json; charset=utf-8",
                        type: "GET",
                        data: { templateUrl: tempAddress, viewModelFullName: viewModelTypeFullName },
                        error: function (data) {
                        },
                        success: function (data, textStatus, jqXHR) {
                            grd.options.editable.template = data;
                            GridOperations.doEditOperation(opType, grd);
                        }
                    });
                }
            }
            else
                GridOperations.doEditOperation(opType, grd);
        }
        GridOperations.gridAdTemplate = gridAdTemplate;
        function gridRemovalTemplate(gId, removalMsg) {
            var okButton = "<a id=\"wm_" + gId + "_btn_yes\" style=\"text-align:center !important;  \" class=\"k-button k-button-icontext\"  ><span class=\"k-icon k-i-tick\" ></span>بله</a>", cancelButton = "<a id=\"wm_" + gId + "_btn_no\" style=\"text-align:center !important;  \" class=\"k-button k-button-icontext\"  ><span class=\"k-icon k-i-cancel\" ></span>خیر</a>", template = "<span id=\"" + gId + "_delete_confirm_message\">" +
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
        GridOperations.gridRemovalTemplate = gridRemovalTemplate;
        function onEditEventHandler(e, grdWindowTitleOnEdit, grdWindowTitleOnAdd) {
            var dialogTitle = grdWindowTitleOnEdit;
            e.container.find('.k-grid-update')[0].innerHTML = getUpdateButton('ذخیره');
            e.container.find('.k-grid-cancel')[0].innerHTML = getCancelButton('انصراف');
            if (e.model.isNew()) {
                dialogTitle = 'جدید';
            }
            else {
                dialogTitle = 'ویرایش';
            }
            e.container.kendoWindow('title', dialogTitle);
        }
        GridOperations.onEditEventHandler = onEditEventHandler;
        function onDataBoundCustomCode(customDataBoundCode, args) {
            if (typeof customDataBoundCode == 'function' && args && typeof args == 'object') {
                customDataBoundCode(args);
            }
            else if (typeof customDataBoundCode == 'function' && !args) {
                customDataBoundCode;
            }
        }
        GridOperations.onDataBoundCustomCode = onDataBoundCustomCode;
    })(GridOperations = ns_Grid.GridOperations || (ns_Grid.GridOperations = {}));
})(ns_Grid || (ns_Grid = {}));
