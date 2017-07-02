/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/kendo/kendo.all.d.ts" />
/// <reference path="../../scripts/tools/dialogbox.ts" />
/// <reference path="../../scripts/tools/generaltools.ts" />

////// Top Scripts

var _CompanyIdViewElement;
$(() => {

    $('#SaveCompanyViewElement').click((e: JQueryEventObject) => {


        var validatorSelectedCompanyNameViewElemen = $("#SelectedCompanyNameViewElement").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (!validatorSelectedCompanyNameViewElemen.validate())
            return;

        var checkedList =
            $("#treeCompanyviewElement .k-item input[type=checkbox]:checked").closest(".k-item");

        var uncheckedList =
            $('#treeCompanyviewElement .k-item input[type=checkbox]:not(:checked)').closest(".k-item");


        var resultCheckedItem = [];
        var resultUnCheckedItem = [];
        var selectedNode: string = "";
        var unCheckedNodeNode: string = "";


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


            success: (result: any, textStatus: string, jqXHR: JQueryXHR) => {


                if (result.Success == "False") {
                    DialogBox.ShowError(result.responseText, 200, 100, true);

                }
                else {
                    DialogBox.ShowNotify("", "اطلاعات با موفقیت ثبت شد");
                }
            },

            error: (jqXHR: JQueryXHR, textStatus: string, errorThrow: string) => {


                DialogBox.ShowError("خطایی رخ داده است لطفا دوباره سعی کنید", 250, 20, true);
            }
        });
    });


    $('input[type=checkbox]').click((e: JQueryEventObject) => {

        var li = $(e.target).closest("li");
        var id = $("input:hidden", li).attr("id");
        var treeView = $("#treeCompanyviewElement").data("kendoTreeView");
        var node = treeView.dataSource.get(id);
        if (node.checked) {
            console.log('checked');
        } else {
            console.log('not checked');
        }
    });

    $('#SelectedCompanyIdViewElement').change((e: JQueryEventObject) => {

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



///// Bottom Scripts


