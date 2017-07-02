var _CompanyIdViewElement;
$(function () {
    $('#SaveCompanyViewElement').click(function (e) {
        var validatorSelectedCompanyNameViewElemen = $("#SelectedCompanyNameViewElement").kendoValidator().data("kendoValidator"), status = $(".status");
        if (!validatorSelectedCompanyNameViewElemen.validate())
            return;
        var checkedList = $("#treeCompanyviewElement .k-item input[type=checkbox]:checked").closest(".k-item");
        var uncheckedList = $('#treeCompanyviewElement .k-item input[type=checkbox]:not(:checked)').closest(".k-item");
        var resultCheckedItem = [];
        var resultUnCheckedItem = [];
        var selectedNode = "";
        var unCheckedNodeNode = "";
        for (var i = 0; i < checkedList.length; i++) {
            var node = $('#treeCompanyviewElement').data('kendoTreeView').dataItem(checkedList[i]);
            resultCheckedItem[i] = node.id;
            selectedNode += node.id + ",";
        }
        for (var i = 0; i < uncheckedList.length; i++) {
            var node = $('#treeCompanyviewElement').data('kendoTreeView').dataItem(uncheckedList[i]);
            resultUnCheckedItem[i] = node.id;
            unCheckedNodeNode += node.id + ",";
        }
        $.ajax({
            url: getAreaUrl("CompanyViewElement", "PostEntity"),
            type: 'POST',
            data: { selectedNode: selectedNode, unCheckedNode: unCheckedNodeNode, companyIdViewElement: _CompanyIdViewElement },
            success: function (result, textStatus, jqXHR) {
                if (result.Success == "False") {
                    DialogBox.ShowError(result.responseText, 200, 100, true);
                }
                else {
                    DialogBox.ShowNotify("", "اطلاعات با موفقیت ثبت شد");
                }
            },
            error: function (jqXHR, textStatus, errorThrow) {
                DialogBox.ShowError("خطایی رخ داده است لطفا دوباره سعی کنید", 250, 20, true);
            }
        });
    });
    $('input[type=checkbox]').click(function (e) {
        var li = $(e.target).closest("li");
        var id = $("input:hidden", li).attr("id");
        var treeView = $("#treeCompanyviewElement").data("kendoTreeView");
        var node = treeView.dataSource.get(id);
        if (node.checked) {
            console.log('checked');
        }
        else {
            console.log('not checked');
        }
    });
    $('#SelectedCompanyIdViewElement').change(function (e) {
        _CompanyIdViewElement = $("#SelectedCompanyIdViewElement").val();
        $("#SelectedCompanyIdViewElement").val(_CompanyIdViewElement);
        if (_CompanyIdViewElement != "") {
            $('#treeCompanyviewElement').data('kendoTreeView').dataSource.read({ id: null, selectedCompanyIdViewElement: _CompanyIdViewElement });
            $("#treeCompanyviewElement").css("display", "block");
        }
    });
});
function addDatatreeCompanyviewElement(data) {
    return { selectedCompanyIdViewElement: _CompanyIdViewElement };
}
