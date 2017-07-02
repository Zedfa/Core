/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/tools/dialogbox.ts" />
/// <reference path="../../scripts/tools/generaltools.ts" />
/// <reference path="../../scripts/typings/kendo/kendo.all.d.ts" />


interface TreeViewChangeEvent extends kendo.ui.TreeViewEvent {
    field?: any;
}

$(() => {

    $('#SaveViewElement').click((e: JQueryEventObject) => {
        
        var validatorSelectedRoleNameViewElement = $("#SelectedRoleNameViewElement").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (!validatorSelectedRoleNameViewElement.validate()) {
            //e.preventDefault();
            return;
        }
        var checkedList =
            $("#treeViewElement .k-item input[type=checkbox]:checked").closest(".k-item");

        var uncheckedList = $('#treeViewElement .k-item input[type=checkbox]:not(:checked)').closest(".k-item");
        var resultCheckedItem = new Array<string>();
        var resultUnCheckedItem = new Array<string>();
        var selectedNode: string = "";
        var unCheckedNodeNode: string = "";


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
            success: (result: any, textStatus: string, jqXHR: JQueryXHR) => {
                DialogBox.ShowNotify("", "اطلاعات با موفقیت ثبت شد");
            },

            error: (jqXHR: JQueryXHR, textStatus: string, errorThrow: string) => {
                DialogBox.ShowError("خطایی رخ داده است لطفا دوباره سعی کنید", 250, 20, true);
            }
        });
    });


    $('input[type=checkbox]').click((e: JQueryEventObject) => {
        var li = $(e.target).closest("li");
        var id = $("input:hidden", li).attr("id");
        var treeView = $("#treeviewElement").data("kendoTreeView");
        var node = treeView.dataSource.get(id);
        if (node.checked) {
            console.log('checked');
        } else {
            console.log('not checked');
        }
    });
});



var _roleId;

function addData(data) {
    return { selectedRole: _roleId };
}


function OnChangeViewElement(parameters?: any): void {
    var treeview = $('#treeviewElement').data('kendoTreeView');
    treeview.dataSource.bind("change",(e: TreeViewChangeEvent) => {
        if (e.field == "checked") {
            alert("checked");
        }
    });
};

function OntreeviewElementBound(parameters?: any): void {
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


$('#SelectedRoleIdViewElement').change((e: JQueryEventObject) => {
    _roleId = $("#SelectedRoleIdViewElement").val();
    if (_roleId != "") {
        $('#treeviewElement').data('kendoTreeView').dataSource.read({ id: null, selectedRole: _roleId });
        $("#treeviewElement").css("display", "block");
    }
});


