

    /*                                  */
    var gor_checkBoxItemClicked = true;
var gor_currentlyInitializedPages = {};
var gor_addedAccountAssignObjects = { page: null, data: [] };
var gor_realDataTobeSend = [];
var gor_selectedKeyValueItems = {};

function gor_checkAllInRoleOranizationAccount(ele) {
    gor_checkBoxItemClicked = false;
    if ($(ele).prop('checked')) {
        gor_selectAllChecked = true;
        gor_checkAllGridCheckBoxes(true);
    }
    else {
        gor_selectAllChecked = false;
        gor_checkAllGridCheckBoxes(false);
    }
        
}
function gor_updateDataItemStateInCheckboxStateArray(itemId, hassAccess) {
    if (!gor_selectedKeyValueItems) {
        gor_selectedKeyValueItems = {};
    }
    gor_setDataItemOfRealData(itemId, hassAccess);
    gor_selectedKeyValueItems[itemId] = hassAccess;
}

function gor_setDataItemOfRealData(itemId , access) {
    var item = gor_getItemFromDataSource(itemId);
   
    item.HasAccess = access;
    if (!_.contains(gor_realDataTobeSend, item)) {
        //Adds the correspondent rowId model data to gor_realDataTobeSend
        gor_realDataTobeSend.push(item);
    }
    else {
        var tI = _.find(gor_realDataTobeSend, function (ti) {
            return ti.id == item.id;
        });
        tI.HasAccess = item.HasAccess;
        //Removes the correspondent rowId model data from gor_realDataTobeSend
        gor_realDataTobeSend = _.without(gor_realDataTobeSend, item);
        //Then, push it again with upadted state.
        gor_realDataTobeSend.push(tI);
    }
        
}

function gor_getItemFromDataSource(id) {
    var dataItem = _.find($("#gridCompanyChartRole").data("kendoGrid").dataSource._data , function (data_source_item) {
        return data_source_item.id == id;
    });
    return dataItem;
}

function gor_IfSelectedObjHasAnyItem() {
    var f = false;
    if (!gor_selectedKeyValueItems) {
        return false;
    }
    $.each(gor_selectedKeyValueItems , function(key , val) { f = true; return;});
    if (f) return true; else return false;
}

function gor_checkOrNotCheckItemsFromSelectedObject(row) {
    var rowId = row.attr('chk-rowid');
    if(gor_selectedKeyValueItems[rowId])
        row.find("input.check_row").prop("checked", true);
    else
        row.find("input.check_row").prop("checked", false);
}

function gor_onOADataBound(e) {
    //Initial condition of filling.
    //if (!gor_IfSelectedObjHasAnyItem()) {
    e.sender.tbody.find("tr").each(function () {
        var $tr = $(this);
                
        var uid = $tr.attr("data-uid");
        var dataItem = _.find(e.sender.dataSource._data , function (data_source_item) {
            return data_source_item.uid === uid;
        });
        $tr.attr('chk-rowid', dataItem.id);
        if (gor_selectedKeyValueItems[$tr.attr('chk-rowid')] === undefined) {
            if (dataItem.HasAccess) {
                $tr.find("input.check_row").prop("checked" , true);
            }
            else {
                $tr.find("input.check_row").prop("checked" , false);
            }
            gor_updateDataItemStateInCheckboxStateArray(dataItem.id, dataItem.HasAccess);
        }
        else {
            $tr.find("input.check_row").prop("checked", gor_selectedKeyValueItems[$tr.attr('chk-rowid')]);
        }
        dataItem.dirty = true;
       
    });
   

    $('.check_row').on('click', function (e) {
        var hasAccess;
        var rowId = $(this).closest('tr').attr('chk-rowid');
        hA = $(this).prop('checked') == true ? true : false;
        gor_updateDataItemStateInCheckboxStateArray(rowId, hA)
    });

    
        

            
    
}

function gor_checkAllGridCheckBoxes(checkAll) {
    if (checkAll) {
        $("#gridCompanyChartRole").data("kendoGrid").tbody.find("tr").each(function (tr) {
            var rowId = $(this).attr('chk-rowid');
            $(this).find('.check_row').prop('checked', true);
            gor_updateDataItemStateInCheckboxStateArray(rowId, true)
        });
    }
    else {
        $("#gridCompanyChartRole").data("kendoGrid").tbody.find("tr").each(function (tr) {
            var rowId = $(this).attr('chk-rowid');
            $(this).find('.check_row').prop('checked', false);
            gor_updateDataItemStateInCheckboxStateArray(rowId, false)
        });
    }
}

function gor_decideWhichRowIdMustBePutInSelected(cb) {
   
    var grd = $("#gridCompanyChartRole");
    var selectedVals = [];
    var grd = $("#gridCompanyChartRole");
    var selectedRowIds = grd.attr('chks-selected');
    if (selectedRowIds != null && selectedRowIds != '') {
        selectedVals = selectedRowIds.split(',');
    }

    var row = cb.closest('tr');
    var rowId = row.attr('chk-rowid');
    var hasAccess;
    if (cb.is(':checked')) {
        selectedVals.push(rowId);
        hasAccess = true;
    } else {
        selectedVals = _.without(selectedVals, rowId);
        hasAccess = false;
    }
        
    grd.attr('chks-selected', selectedVals);
    gor_updateRealDataTobeSend(rowId, grd, hasAccess);
}

function gor_updateRealDataTobeSend(rowId, grdSel, hasAccess) {
   
    var grdObj = grdSel.data("kendoGrid");
    var trItm = grdObj.dataItem(grdObj.tbody.find("tr[chk-rowid='" + rowId + "']"));
    trItm.HasAccess = hasAccess;
    if (!_.contains(gor_realDataTobeSend, trItm)) {
        //Adds the correspondent rowId model data to gor_realDataTobeSend
        gor_realDataTobeSend.push(trItm);
    }
    else {
        var tI = _.find(gor_realDataTobeSend, function (ti) {
            return ti.AccountNo == trItm.AccountNo;
        });
        tI.HasAccess = trItm.HasAccess;
        //Removes the correspondent rowId model data from gor_realDataTobeSend
        gor_realDataTobeSend = _.without(gor_realDataTobeSend, trItm);
        //Then, push it again with upadted state.
        gor_realDataTobeSend.push(tI);
    }
    trItm.dirty = true;
}

function gor_AssignAccount_onSaveChanges(e) {
    gor_addedAccountAssignObjects.page = e.sender.dataSource.page();
    gor_addedAccountAssignObjects.data = e.sender.dataSource.data();
    e.sender.dataSource.data(gor_realDataTobeSend);
}

function gor_syncComplete(e) {
    e.sender.data(gor_addedAccountAssignObjects.data);
    e.sender.page(gor_addedAccountAssignObjects.page);
}
/*                                                      */
var addedToRoot = false;

function onSelect(e) {
   
    var data = $('#treeviewCompanyChart').data('kendoTreeView').dataItem(e.node);
    selectedNodeId = data.id;
       
    // $("#gridCompanyChartRole").attr('chks-selected', []);
    gor_selectAllChecked = false;
    gor_checkBoxItemClicked = true;
    gor_currentlyInitializedPages = {};
    gor_realDataTobeSend = [];
    gor_selectedKeyValueItems = {};
    $("#gridCompanyChartRole").data("kendoGrid").dataSource.read();
    $("#gridCompanyChartRole").css("display", "block");
    //visible delete button
    $("#removeNode").css('visibility', 'visible');
    //$('#masterCheckBox').prop('checked', false);
}

function onRequestEndgridCompanyChartRole(e) {
    if (e.type == "update") {
        if (e.response = "Created") {
            DialogBox.ShowNotify("", "اطلاعات با موفقیت ثبت شد");
            return;
        }
    }
}
$("#chkAddNode").change(function () {
   
    if (this.checked) {
        $("#appendNodeToSelected").val('اضافه به ریشه');
        addedToRoot = true;
    } else {
        $("#appendNodeToSelected").val('اضافه');
        addedToRoot = false;
    }
});
