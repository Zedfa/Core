var Lookup;
(function (Lookup) {
    var _currentlkp = null;
    var LkupInfo = (function () {
        function LkupInfo() {
        }
        return LkupInfo;
    }());
    var Lkup = (function () {
        function Lkup(typ) {
            this._btnConfirm = null;
            this._btnCancel = null;
            this._win = null;
            this._lookupInfo = null;
            this._grid = null;
            this._tree = null;
            this._type = null;
            this.onClose = null;
            this.onOpen = null;
            this._outerHtml = null;
            this._type = typ;
        }
        Lkup.prototype._initLookup = function (viewModelName, ViewInfoName, lookupName, Id, displayName, valueName, bindingName, isMultiselect) {
            var lkpInfo = new LkupInfo();
            lkpInfo.viewModelName = viewModelName;
            lkpInfo.ViewInfoName = ViewInfoName;
            lkpInfo.lookupName = lookupName;
            lkpInfo.Id = Id;
            lkpInfo.displayName = displayName;
            lkpInfo.valueName = valueName;
            lkpInfo.bindingName = bindingName;
            lkpInfo.isMultiselect = isMultiselect;
            lkpInfo.onClose = this.onClose;
            lkpInfo.onOpen = this.onOpen;
            this._lookupInfo = lkpInfo;
        };
        Lkup.prototype._createControl = function (title, width, height) {
            var info = this._lookupInfo;
            var container = $("<span>");
            if (info.isMultiselect == "False") {
                var textbox = $("<input/>", { id: info.lookupName, type: 'text', "class": 'k-textbox' });
                var hidden = $("<input/>", { id: info.bindingName, type: 'hidden' });
                container.html(textbox);
                container.append(hidden);
            }
            else {
                var multiSelect = $("<select>", {
                    id: info.bindingName,
                    "data-role": "multiple",
                    "data-placeholder": "انتخاب نشده",
                    "data-text-field": info.displayName,
                    "data-value-field": info.valueName
                });
                container.css("class", "rp-lookup");
                container.html(multiSelect);
            }
            var button = this._createButton(title, width, height);
            container.append(button);
            return container;
        };
        Lkup.prototype._createButton = function (title, width, height) {
            var info = this._lookupInfo;
            var type = this._type;
            var container = $("<button/>", {
                id: 'btn_' + info.lookupName,
                "class": "k-space-right k-button",
                onclick: "Lookup.load" + type + "('" + title + "','"
                    + info.viewModelName + "','"
                    + info.ViewInfoName + "','"
                    + info.lookupName + "','"
                    + info.Id + "','"
                    + info.displayName + "','"
                    + info.valueName + "','"
                    + info.bindingName + "','"
                    + info.isMultiselect + "',"
                    + width + "," +
                    height + ")"
            });
            var icon = $("<span/>", { "class": "k-icon l-i-button" });
            container.html(icon);
            return container;
        };
        Lkup.prototype._initWindow = function (dialog) {
            var that = this;
            that._win = dialog._win;
            that = this;
            that._open(that._lookupInfo.onOpen);
            that._btnConfirm = dialog._btnConfirm;
            that._btnConfirm.click(function (e) {
                that._confirm();
            });
            that._btnCancel = dialog._btnCancel;
            that._btnCancel.click(function (e) {
                that._close();
            });
        };
        Lkup.prototype._loadGrid = function (title, width, height) {
            var info = this._lookupInfo;
            var tempUrl = "/Views/Lookup/_Grid.cshtml";
            tempUrl = "~/Areas/" + "Core" + tempUrl;
            var sendingdData = {
                templateUrl: tempUrl,
                viewModel: info.viewModelName,
                viewInfo: info.ViewInfoName,
                lookup: info.lookupName,
                isMultiSelect: info.isMultiselect,
                propertyNameForDisplay: info.displayName,
                propertyNameForValue: info.valueName,
                propertyNameForBinding: info.bindingName
            };
            var dlg = DialogBox.ShowDialog(getAreaUrl("Template", "UploadGridLookup"), title, "", width, height, sendingdData);
            this._initWindow(dlg);
            var that = this;
            dlg._win.bind("refresh", function (e) {
                that._grid = e.sender.element.find("[data-role=grid]").data("kendoGrid");
            });
        };
        Lkup.prototype._loadTree = function (title, width, height) {
            var info = this._lookupInfo;
            var tempUrl = "/Views/Lookup/_TreeView.cshtml";
            tempUrl = "~/Areas/Core" + tempUrl;
            var sendingdData = {
                templateUrl: tempUrl,
                viewModel: info.viewModelName,
                ViewInfo: info.ViewInfoName,
                lookup: info.lookupName,
                isMultiSelect: info.isMultiselect,
                propertyNameForDisplay: info.displayName,
                propertyNameForBinding: info.bindingName
            };
            var dlg = DialogBox.ShowDialog(getAreaUrl("Template", "UploadTreeLookup"), title, "", width, height, sendingdData);
            this._initWindow(dlg);
            var that = this;
            dlg._win.bind("refresh", function (e) {
                that._tree = e.sender.element.find("[data-role=treeview]").data("kendoTreeView");
            });
        };
        Lkup.prototype._confirm = function () {
            var that = this;
            if (that._type == "grid") {
                that._submitGrid();
            }
            else {
                that._submitTree();
            }
            that._close();
        };
        Lkup.prototype._close = function () {
            var that = this;
            var onClosefunc = that._lookupInfo.onClose;
            if (onClosefunc != null && $.isFunction(onClosefunc)) {
                onClosefunc();
            }
            that._win.close();
        };
        Lkup.prototype._open = function (openfunc) {
            if (openfunc != null && $.isFunction(openfunc)) {
                openfunc();
            }
        };
        Lkup.prototype._submitGrid = function () {
            var info = this._lookupInfo;
            var grid = this._grid;
            var selectedItem = grid.select().is("td") ? grid.select().parent() : grid.select();
            if (info.isMultiselect != "False")
                this._setMultiSelectForGrid(selectedItem);
            else
                this._setTextBoxForGrid(selectedItem);
        };
        Lkup.prototype._setTextBoxForGrid = function (selectedItem) {
            var grid = this._grid;
            var info = this._lookupInfo;
            var diplayText = grid.dataItem(selectedItem)[info.displayName];
            var propertyValue = grid.dataItem(selectedItem)[info.valueName];
            $("#" + info.lookupName).val(diplayText);
            $("#" + info.lookupName).trigger("change");
            $("#" + info.bindingName).val(propertyValue);
            $("#" + info.bindingName).trigger("change");
        };
        Lkup.prototype._setMultiSelectForGrid = function (selectedItems) {
            var grid = this._grid;
            var info = this._lookupInfo;
            var selectedData = [];
            var selectedValues = [];
            for (var i = 0; i < grid.select().length; i++) {
                var data = JSON.parse('{"' + info.displayName + '" :"' + grid.dataItem(selectedItems[i])[info.displayName] + '", "'
                    + info.valueName + '":"' + grid.dataItem(selectedItems[i])[info.valueName] + '"}');
                selectedData.push(data);
                selectedValues.push(selectedData[i][info.valueName]);
            }
            var multiSelect = $("#" + info.bindingName).data("kendoMultiSelect");
            multiSelect.dataSource.data(selectedData);
            multiSelect.value(selectedValues);
            multiSelect.trigger("change");
        };
        Lkup.prototype._submitTree = function () {
            var info = this._lookupInfo;
            var tree = this._tree;
            var modelKey = tree.dataSource.options.schema.model.id;
            if (info.isMultiselect == "False") {
                var selectedItem = tree.select();
                var selectedData = tree.dataItem(selectedItem);
                this._setTextBoxForTree(selectedItem, selectedData, modelKey);
            }
            else {
                var selectedList = $("#" + info.Id + " :checked");
                this._setMultiselectForTree(selectedList, modelKey);
            }
        };
        Lkup.prototype._setTextBoxForTree = function (selectedItem, selectedData, key) {
            var info = this._lookupInfo;
            $("#" + info.lookupName).val(selectedData[info.displayName]);
            $("#" + info.lookupName).trigger("change");
            $("#" + info.bindingName).val(selectedData[key]);
            $("#" + info.bindingName).trigger("change");
        };
        Lkup.prototype._setMultiselectForTree = function (selectedList, key) {
            var selectedData = [];
            var selectedValues = [];
            var info = this._lookupInfo;
            var multiSelect = $("#" + info.bindingName).data("kendoMultiSelect");
            if (selectedList.length > 0) {
                for (var i = 0; i < selectedList.length; i++) {
                    var selectedItem = $($(selectedList[i]).closest(".k-item").find(".k-in")[0]).text();
                    var data = JSON.parse('{"' + info.displayName + '" :"' + selectedItem + '", "' + key + '":' + $(selectedList[i]).attr("value") + '}');
                    selectedData.push(data);
                    selectedValues.push(selectedData[i][key]);
                }
                multiSelect.dataSource.data(selectedData);
                multiSelect.value(selectedValues);
                multiSelect.trigger("change");
            }
        };
        return Lkup;
    }());
    Lookup.Lkup = Lkup;
    function createControl(type, title, viewModelName, ViewInfoName, lookupName, displayName, valueName, bindingName, isMultiselect, width, height) {
        var lkp = new Lkup(type);
        lkp._initLookup(viewModelName, ViewInfoName, lookupName, "lookupGrid_" + lookupName, displayName, valueName, bindingName, isMultiselect);
        lkp._outerHtml = lkp._createControl(title, width, height);
        _currentlkp = lkp;
        return lkp;
    }
    Lookup.createControl = createControl;
    function loadGrid(title, viewModelName, ViewInfoName, lookupName, Id, displayName, valueName, bindingName, isMultiselect, width, height) {
        var lkp = new Lkup("grid");
        lkp._initLookup(viewModelName, ViewInfoName, lookupName, Id, displayName, valueName, bindingName, isMultiselect);
        lkp._loadGrid(title, width, height);
        _currentlkp = lkp;
    }
    Lookup.loadGrid = loadGrid;
    function loadTree(title, viewModelName, ViewInfoName, lookupName, Id, displayName, bindingName, isMultiselect, width, height) {
        var lkp = new Lkup("tree");
        lkp._initLookup(viewModelName, ViewInfoName, lookupName, Id, displayName, "", bindingName, isMultiselect);
        lkp._loadTree(title, width, height);
        _currentlkp = lkp;
    }
    Lookup.loadTree = loadTree;
    function confirm() {
        _currentlkp._confirm();
    }
    Lookup.confirm = confirm;
})(Lookup || (Lookup = {}));
