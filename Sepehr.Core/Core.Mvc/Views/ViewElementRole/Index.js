$(function () {
    $('#SaveViewElement').click(function (e) {
        var validatorSelectedRoleNameViewElement = $("#SelectedRoleNameViewElement").kendoValidator().data("kendoValidator"), status = $(".status");
        if (!validatorSelectedRoleNameViewElement.validate()) {
            return;
        }
        var checkedList = $("#treeViewElement .k-item input[type=checkbox]:checked").closest(".k-item");
        var uncheckedList = $('#treeViewElement .k-item input[type=checkbox]:not(:checked)').closest(".k-item");
        var resultCheckedItem = new Array();
        var resultUnCheckedItem = new Array();
        var selectedNode = "";
        var unCheckedNodeNode = "";
        for (var i = 0; i < checkedList.length; i++) {
            var node = $('#treeviewElement').data('kendoTreeView').dataItem(checkedList[i]);
            resultCheckedItem[i] = node.id;
            selectedNode += node.id + ",";
        }
        for (var i = 0; i < uncheckedList.length; i++) {
            var node = $('#treeviewElement').data('kendoTreeView').dataItem(uncheckedList[i]);
            resultUnCheckedItem[i] = node.id;
            unCheckedNodeNode += node.id + ",";
        }
        $.ajax({
            url: getAreaUrl("ViewElementRole", "PostEntity"),
            type: 'POST',
            data: { selectedNode: selectedNode, unCheckedNode: unCheckedNodeNode, roleId: _roleId },
            success: function (result, textStatus, jqXHR) {
                DialogBox.ShowNotify("", "اطلاعات با موفقیت ثبت شد");
            },
            error: function (jqXHR, textStatus, errorThrow) {
                DialogBox.ShowError("خطایی رخ داده است لطفا دوباره سعی کنید", 250, 20, true);
            }
        });
    });
    $('input[type=checkbox]').click(function (e) {
        var li = $(e.target).closest("li");
        var id = $("input:hidden", li).attr("id");
        var treeView = $("#treeviewElement").data("kendoTreeView");
        var node = treeView.dataSource.get(id);
        if (node.checked) {
            console.log('checked');
        }
        else {
            console.log('not checked');
        }
    });
});
var _roleId;
function addData(data) {
    return { selectedRole: _roleId };
}
function OnChangeViewElement(parameters) {
    var treeview = $('#treeviewElement').data('kendoTreeView');
    treeview.dataSource.bind("change", function (e) {
        if (e.field == "checked") {
            alert("checked");
        }
    });
}
;
function OntreeviewElementBound(parameters) {
    var treeView = $('#treeviewElement').data('kendoTreeView');
    if (treeView != null && eval("CurrentlyActiveGroupTreeNode") != null) {
        var selectedDataItem = treeView.dataItem(eval("CurrentlyActiveGroupTreeNode"));
        selectedDataItem.haschildren = true;
        selectedDataItem.loaded(false);
        selectedDataItem.load();
        treeView.one("dataBound", function () {
            treeView.expand(eval("CurrentlyActiveGroupTreeNode"));
        });
    }
}
$('#SelectedRoleIdViewElement').change(function (e) {
    _roleId = $("#SelectedRoleIdViewElement").val();
    if (_roleId != "") {
        $('#treeviewElement').data('kendoTreeView').dataSource.read({ id: null, selectedRole: _roleId });
        $("#treeviewElement").css("display", "block");
    }
});
