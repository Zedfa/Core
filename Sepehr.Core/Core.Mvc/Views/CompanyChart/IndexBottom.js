

    var selectedNodeId;
var title;


var gridCompanyChartRole = $("#gridCompanyChartRole").data("kendoGrid");

gridCompanyChartRole.dataSource.transport.options.read.url = function (data) {
   
    return getAreaUrl("CompanyRole","GetRoles")+"?CompanyChartId=" + selectedNodeId;
};

var treeview = $("#treeview").data("kendoTreeView"),
    handleTextBox = function (callback) {
        return function (e) {
            if (e.type != "keypress" || kendo.keys.ENTER == e.keyCode) {
                callback(e);
            }
        };
    };


//invisible delete button
$("#removeNode").css('visibility', 'hidden');

var wndRemoveCompanyChart = $("#modalWindow").kendoWindow({
    title: "حذف",
    modal: true,

    visible: false,

    resizable: false,

    width: 400

}).data("kendoWindow");

var isSetClickCompanyChartRemoveEvent = false;
$("#removeNode").click(function () {
    var selectedNode = treeview.select();
    wndRemoveCompanyChart.center();
    wndRemoveCompanyChart.open();

    if (!isSetClickCompanyChartRemoveEvent) {
        $("#btnCompanyChartYes").click(function () {

            if (selectedNodeId == null) {
                DialogBox.ShowError("گروه سازمانی را انتخاب نمائید", 250, 20, true);
                return;
            }


            $.ajax({
                url: getAreaUrl( "CompanyChart","Delete"),
                // url: "../api/CompanyChartApi",
                type: 'Delete',

                data: {
                    selectedNodeId: selectedNodeId,
                },

                success: function (result) {

                    treeview.remove(selectedNode);
                    $("#gridCompanyChartRole").css("display", "none");
                    selectedNodeId = null;
                    //invisible delete button
                    $("#removeNode").css('visibility', 'hidden');


                },

                error: function (xhr) {
                    DialogBox.ShowError("خطا در حذف", 250, 20);
                }
            });

            wndRemoveCompanyChart.close();

        });
        isSetClickCompanyChartRemoveEvent = true;

    }


    $("#btnCompanyChartNo").click(function () {

        wndRemoveCompanyChart.close();

    });



    // });


    var append = handleTextBox(function (e) {


        var selectedNode = treeview.select();



        // passing a falsy value as the second append() parameter
        // will append the new node to the root group
        if (selectedNode.length == 0 || addedToRoot) {
            selectedNodeId = null;
            selectedNode = null;
        }

        title = $("#appendNodeText").val();

        var organizationChartTitle = $("#appendNodeText").kendoValidator().data("kendoValidator"),
          status = $(".status");

        if (!organizationChartTitle.validate())
            return;

        $.ajax({
            url: getAreaUrl("CompanyChart","Create"),

            //url: "../api/CompanyChartApi",
            type: 'POST',

            data: {

                SelectedNodeId: selectedNodeId,
                title: title


            },

            success: function (result) {

               

                if (selectedNode == null) {
                    var addedParnt = treeview.dataItem(treeview.append({ text: title }, selectedNode));
                    var node = treeview.findByUid(addedParnt.uid);
                    node.find(">div>.k-in").contents(":last").replaceWith(title);
                    treeview.dataItem(node).set('id', result.id);
                    addedParnt.loaded(false);
                    addedParnt.load();

                }


                if (selectedNode != null) {
                    treeview.append({ text: title }, selectedNode);
                    var selectedDataItem = treeview.dataItem(selectedNode);
                    selectedDataItem.loaded(false);
                    selectedDataItem.load();

                }
                $("#appendNodeText").val("");


            },


            error: function (xhr) {

                DialogBox.ShowError("خطا رخ داده لطفا دوباره سعی نمائید", 250, 20);
            }
        });


    });

    $("#appendNodeToSelected").click(append);
    $("#appendNodeText").keypress(append);

    

});
