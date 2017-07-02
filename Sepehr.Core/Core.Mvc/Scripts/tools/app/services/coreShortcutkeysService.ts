
var coreShortcutKeysService = new SepehrModule.MainModule('coreShortcutKeysService', []);

coreShortcutKeysService.addService("enableShortcutKeys", [() => {
    return (container: JQuery) => {
        $(container).on("keyup",(e) => {
            
            if (e.altKey && e.keyCode == 71 /* letter g*/) {
                var id :string = "";
                $.each(container.find("[kendo-grid]"),(index, element) => {
                   
                    var grid = $(element).data("kendoGrid"),
                        table: JQuery = grid.table;
                    if (!table.is(document.activeElement)) {
                        id = $(element).attr("kendo-grid");
                        if (table.has("tr").length > 0) {
                            grid.select(table.find("tr:first"));
                        }
                        else {
                            table.focus().find(".k-scrollbar.k-scrollbar-vertical").scrollTop(0);
                        }
                        return false;
                    }
                });
              
                //$.each(container.find("[kendo-grid]:not( [kendo-grid=" + id + "])"),(index, element) => {
                //    var grid = $(element).data("kendoGrid");
                //    grid.clearSelection();
                //});
            }
        });
    }
}]);