function onDblClick(e) {
}
function onEditRole(e) {
    var windowOptions = null;
    if (e.model.isNew()) {
        e.container.kendoWindow("title", "تعریف نقش");
    }
    else {
        e.container.kendoWindow("title", "ویرایش نقش");
    }
}
