var coreShortcutKeysService = new SepehrModule.MainModule('coreShortcutKeysService', []);
coreShortcutKeysService.addService("enableShortcutKeys", [function () {
        return function (container) {
            $(container).on("keyup", function (e) {
                if (e.altKey && e.keyCode == 71) {
                    var id = "";
                    $.each(container.find("[kendo-grid]"), function (index, element) {
                        var grid = $(element).data("kendoGrid"), table = grid.table;
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
                }
            });
        };
    }]);
