/// <reference path="generaltools.ts" />
/// <reference path="../typings/kendo/kendo.all.d.ts" />
/// <reference path="dialogbox.ts" />

module Lookup {
    var _currentlkp: Lkup = null;
    class LkupInfo {
        viewModelName: string
        ViewInfoName: string
        lookupName: string
        Id: string
        displayName: string
        valueName: string
        bindingName: string
        isMultiselect: string
        onOpen: Function
        onClose: Function
    }

    export class Lkup {
        private _btnConfirm: JQuery = null;
        private _btnCancel: JQuery = null;
        private _win: kendo.ui.Window = null;
        private _lookupInfo: LkupInfo = null;
        public _grid: kendo.ui.Grid = null;
        private _tree: kendo.ui.TreeView = null;
        private _type: string = null;

        public onClose: Function = null;
        public onOpen: Function = null;

        public _outerHtml: JQuery = null;

        constructor(typ: string) {
            this._type = typ;
        }

        public _initLookup(viewModelName: string, ViewInfoName: string, lookupName: string, Id: string, displayName: string, valueName: string, bindingName: string, isMultiselect: string) {

            var lkpInfo: LkupInfo = new LkupInfo();

            lkpInfo.viewModelName = viewModelName;

            lkpInfo.ViewInfoName = ViewInfoName;

            lkpInfo.lookupName = lookupName;

            //id which is related to Grid or Tree
            lkpInfo.Id = Id;

            lkpInfo.displayName = displayName;

            lkpInfo.valueName = valueName;

            lkpInfo.bindingName = bindingName;

            lkpInfo.isMultiselect = isMultiselect;

            lkpInfo.onClose = this.onClose;

            lkpInfo.onOpen = this.onOpen;

            this._lookupInfo = lkpInfo;
        }

        public _createControl(title: string, width: number, height: number): JQuery {
            var info = this._lookupInfo;

            var container: JQuery = $("<span>");

            if (info.isMultiselect == "False") {

                var textbox: JQuery = $("<input/>", { id: info.lookupName, type: 'text', "class": 'k-textbox' });
                var hidden: JQuery = $("<input/>", { id: info.bindingName, type: 'hidden' });
                container.html(<any>textbox);
                container.append(hidden);
            }
            else {

                var multiSelect: JQuery = $("<select>", {
                    id: info.bindingName,
                    "data-role": "multiple",
                    "data-placeholder": "انتخاب نشده",
                    "data-text-field": info.displayName,
                    "data-value-field": info.valueName
                });

                container.css("class", "rp-lookup");
                container.html(<any>multiSelect);


            }

            var button = this._createButton(title, width, height);

            container.append(button);

            return container;

        }

        public _createButton(title: string, width: number, height: number): JQuery {

            var info: LkupInfo = this._lookupInfo;
            var type: string = this._type;

            var container: JQuery = $("<button/>", {
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

            var icon: JQuery = $("<span/>", { "class": "k-icon l-i-button" });
            container.html(<any>icon);
            return container;

        }

        public _initWindow(dialog: DialogBox.Dialog) {

            var that = this;

            that._win = dialog._win;
            that = this;

            that._open(that._lookupInfo.onOpen);

            that._btnConfirm = dialog._btnConfirm;

            that._btnConfirm.click((e: JQueryEventObject) => {
                that._confirm();
            });

            that._btnCancel = dialog._btnCancel;

            that._btnCancel.click((e: JQueryEventObject) => {
                that._close();
            });


        }

        public _loadGrid(title: string, width: string, height: string) {
            var info = this._lookupInfo;
            var tempUrl = "/Views/Lookup/_Grid.cshtml";
            //TO DO: Why we need to get areaname!!!
            //if (window.areaName) {
                tempUrl = "~/Areas/" + "Core" + tempUrl;
            //}
            //else {
            //    tempUrl = "~" + tempUrl;
            //}

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

            dlg._win.bind("refresh",(e) => {
                
                that._grid = e.sender.element.find("[data-role=grid]").data("kendoGrid");
                //that._grid = $("#" + info.Id).data("kendoGrid");
            });



        }

        public _loadTree(title: string, width: number, height: number) {

            var info = this._lookupInfo;
            var tempUrl: string = "/Views/Lookup/_TreeView.cshtml";

            //if (window.areaName) {
               tempUrl = "~/Areas/Core" + tempUrl;
            //}
            //else {
               // tempUrl = "~" + tempUrl;
            //}
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

            dlg._win.bind("refresh",(e) => {
                that._tree = e.sender.element.find("[data-role=treeview]").data("kendoTreeView");
            });


        }

        public _confirm(): void {

            var that = this;

            if (that._type == "grid") {

                that._submitGrid();
            }
            else {

                that._submitTree();
            }
            that._close();
        }

        public _close() {

            var that = this;
            var onClosefunc: Function = that._lookupInfo.onClose;

            if (onClosefunc != null && $.isFunction(onClosefunc)) {
                onClosefunc();
            }

            that._win.close();

        }

        public _open(openfunc: Function) {
            if (openfunc != null && $.isFunction(openfunc)) {
                openfunc();
            }
        }
        //--------------Grid--------------------
        public _submitGrid() {

            var info = this._lookupInfo;
           
            // this._grid = $("#" + info.Id).data("kendoGrid");

            var grid = this._grid;

            var selectedItem: JQuery = grid.select().is("td") ? grid.select().parent() : grid.select();

            if (info.isMultiselect != "False")
                this._setMultiSelectForGrid(selectedItem);
            else
                this._setTextBoxForGrid(selectedItem);

        }

        public _setTextBoxForGrid(selectedItem: JQuery) {

            var grid = this._grid;

            var info = this._lookupInfo;

            var diplayText = grid.dataItem(selectedItem)[info.displayName];

            var propertyValue = grid.dataItem(selectedItem)[info.valueName];

            $("#" + info.lookupName).val(diplayText);

            $("#" + info.lookupName).trigger("change");

            $("#" + info.bindingName).val(propertyValue);

            $("#" + info.bindingName).trigger("change");

        }

        public _setMultiSelectForGrid(selectedItems) {

            var grid = this._grid;

            var info = this._lookupInfo;

            var selectedData = [];

            var selectedValues = [];

            for (var i = 0; i < grid.select().length; i++) {

                //  var data = '{"' + displayName + '" :"' + grid.dataItem(selectedItems[i])[displayName] + '", "' + valueName + '":"' + grid.dataItem(selectedItems[i])[valueName] + '"}';

                var data: JSON = JSON.parse('{"' + info.displayName + '" :"' + grid.dataItem(selectedItems[i])[info.displayName] + '", "'
                    + info.valueName + '":"' + grid.dataItem(selectedItems[i])[info.valueName] + '"}');

                selectedData.push(data);

                selectedValues.push(selectedData[i][info.valueName]);
            }

            var multiSelect: kendo.ui.MultiSelect = $("#" + info.bindingName).data("kendoMultiSelect");

            //$("#" + info.bindingName).change(function () { alert("multiselect changed") });

            multiSelect.dataSource.data(selectedData);

            multiSelect.value(selectedValues);

            multiSelect.trigger("change");

            //  multiSelect.value(selectedValues);

            // multiSelect.element.change(function (e) { multiSelect.value(selectedValues); });

            // multiSelect.element.trigger("change");
            //  $("#" + info.bindingName).change();
            // $("#" + info.bindingName).trigger("change");
            //multiSelect.trigger("change");
            // multiSelect.dataSource.sync();

            //  multiSelect.refresh();
            //   multiSelect.wrapper.trigger("change");

        }

        //----------Tree View---------------------------------------------

        public _submitTree() {

            var info = this._lookupInfo;

            //this._tree = $("#" + info.Id).data("kendoTreeView");

            var tree: kendo.ui.TreeView = this._tree;

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
        }

        public _setTextBoxForTree(selectedItem, selectedData, key) {


            var info = this._lookupInfo;

            $("#" + info.lookupName).val(selectedData[info.displayName]);

            $("#" + info.lookupName).trigger("change");

            $("#" + info.bindingName).val(selectedData[key]);

            //$("#" + info.bindingName).val(selectedData.id);


            $("#" + info.bindingName).trigger("change");
        }

        public _setMultiselectForTree(selectedList: JQuery, key: any) {

            var selectedData = [];

            var selectedValues = [];

            var info = this._lookupInfo;

            var multiSelect: kendo.ui.MultiSelect = $("#" + info.bindingName).data("kendoMultiSelect");

            if (selectedList.length > 0) {

                for (var i = 0; i < selectedList.length; i++) {

                    var selectedItem = $($(selectedList[i]).closest(".k-item").find(".k-in")[0]).text();

                    var data: JSON = JSON.parse('{"' + info.displayName + '" :"' + selectedItem + '", "' + key + '":' + $(selectedList[i]).attr("value") + '}');

                    selectedData.push(data);

                    selectedValues.push(selectedData[i][key]);
                }

                multiSelect.dataSource.data(selectedData);

                multiSelect.value(selectedValues);

                multiSelect.trigger("change");
            }

        }
    }

    export function createControl(type: string, title: string, viewModelName: string, ViewInfoName: string, lookupName, displayName: string, valueName: string, bindingName: string, isMultiselect: string, width: number, height: number): Lkup {

        var lkp = new Lkup(type);

        lkp._initLookup(viewModelName, ViewInfoName, lookupName, "lookupGrid_" + lookupName, displayName, valueName, bindingName, isMultiselect);

        lkp._outerHtml = lkp._createControl(title, width, height);

        _currentlkp = lkp;

        return lkp;

    }

    export function loadGrid(title, viewModelName, ViewInfoName, lookupName, Id, displayName, valueName, bindingName, isMultiselect, width, height): void {

        var lkp = new Lkup("grid")

        lkp._initLookup(viewModelName, ViewInfoName, lookupName, Id, displayName, valueName, bindingName, isMultiselect);

        lkp._loadGrid(title, width, height);

        _currentlkp = lkp;

    }

    export function loadTree(title, viewModelName, ViewInfoName, lookupName, Id, displayName, bindingName, isMultiselect, width, height): void {

        var lkp = new Lkup("tree");

        lkp._initLookup(viewModelName, ViewInfoName, lookupName, Id, displayName, "", bindingName, isMultiselect);

        lkp._loadTree(title, width, height);

        _currentlkp = lkp;
    }

    export function confirm() {
        _currentlkp._confirm();
    }

}





