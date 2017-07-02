/// <reference path="../../scripts/typings/kendo/kendo.all.d.ts" />
/// <reference path="../../scripts/customtypescriptextensions/kendoextentions.d.ts" />
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
function onDblClick(e) {

}

function onEditRole(e:kendo.ui.GridEditEvent) {
var windowOptions:kendo.ui.WindowOptions = null
    if (e.model.isNew()) {
        e.container.kendoWindow("title", "تعریف نقش");
    } else {
        e.container.kendoWindow("title", "ویرایش نقش");
    }

}