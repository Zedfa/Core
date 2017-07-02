var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Template;
(function (Template) {
    var CustomNode = (function (_super) {
        __extends(CustomNode, _super);
        function CustomNode() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        return CustomNode;
    }(kendo.data.Node));
    var CustomTreeView = (function (_super) {
        __extends(CustomTreeView, _super);
        function CustomTreeView() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        CustomTreeView.prototype.dataItem = function (e) {
            var superNode = _super.prototype.dataItem.call(this, e);
            var derivedNode = new CustomNode();
            derivedNode.Node = superNode;
            derivedNode.Id = superNode.id;
            return derivedNode;
        };
        return CustomTreeView;
    }(kendo.ui.TreeView));
    var Temp = (function () {
        function Temp() {
            this._width = null;
            this._height = null;
            this._tree = null;
            this._dataSource = null;
            this._win = null;
            this._treeTemplateName = null;
            this._idField = undefined;
            this._model = null;
        }
        Temp.prototype._show = function (viewModelName, winTitle, type, helpUrl) {
            var tempUrl = "/Views/Shared/EditorTemplates/" + viewModelName + "Template.cshtml";
            if (window.areaName) {
                tempUrl = "~/Areas/" + window.areaName + tempUrl;
            }
            else {
                tempUrl = "~" + tempUrl;
            }
            var sendingdData = {
                templateUrl: tempUrl,
                viewModel: viewModelName
            };
            if ((type == "Post") || (type == "Put" && this._tree.select().length > 0)) {
                var dlg = DialogBox.ShowDialog(getAreaUrl("Template", "Show"), winTitle, "", this._width, this._height, sendingdData);
                this._win = dlg._win;
                var that = this;
                this._win.bind("refresh", function (e) {
                    that._win = e.sender;
                    switch (type) {
                        case "Post":
                            that._insert();
                            break;
                        case "Put":
                            that._update();
                            break;
                    }
                });
            }
            else if (type == "Delete" && this._tree.select().length > 0) {
                var message = "آیا از حذف اطمینان دارید ؟";
                var deleteDialog = DialogBox.ShowNotify(winTitle, message, 350, 65, true);
                this._win = deleteDialog._win;
                var that = this;
                this._win.bind("activate", function (e) {
                    that._win = e.sender;
                    that._delete();
                });
            }
            else if (type == "Help") {
                var helpData = { viewModelName: viewModelName.replace("ViewModel", "") };
                DialogBox.ShowDialog(helpUrl, winTitle, null, 600, 400, helpData, false);
            }
        };
        Temp.prototype._insert = function () {
            var newRecord = new this._dataSource.reader.model();
            this._idField = newRecord._childrenOptions.schema.model.id;
            var selectedNode = this._tree.select();
            if (selectedNode.length == 0) {
                this._tree.append(newRecord);
            }
            else {
                var parentModel = this._tree.dataItem(selectedNode);
                newRecord.ParentId = parentModel[this._idField];
                this._tree.append(newRecord, selectedNode);
            }
            this._bind(newRecord, false);
        };
        Temp.prototype._update = function () {
            var selectedNode = this._tree.select();
            var model = this._tree.dataItem(selectedNode);
            model.HasChildren = model.hasChildren;
            this._bind(model, true);
        };
        Temp.prototype._delete = function () {
            var selectedNode = this._tree.select();
            var model = this._tree.dataItem(selectedNode);
            this._tree.remove(selectedNode);
            this._bind(model, false);
        };
        Temp.prototype._setParams = function (data, operation) {
            if (operation == 'read' && (data.filter || data.sort || data.group)) {
                var mvcAjaxTransport = new kendo.data.transports["aspnetmvc-ajax"]();
                return mvcAjaxTransport.parameterMap(data, operation);
            }
            else
                return data;
        };
        Temp.prototype._bind = function (model, isInEditMode) {
            var template = this;
            template._model = model;
            kendo.data.binders.vm = kendo.data.Binder.extend({
                init: function (target, bindings, options) {
                    kendo.data.Binder.fn.init.call(kendo.data.binders.vm, target, bindings, options);
                    var that = kendo.data.binders.vm;
                    var path = bindings.vm.path;
                    if (model.isNew() && bindings.vm.source.defaults[path] == undefined) {
                        var defaultVal = '';
                        bindings.vm.set(defaultVal);
                    }
                    if (model != undefined && isInEditMode) {
                        $(target).val(model[path]);
                    }
                    if (target.type == "submit") {
                        switch (path) {
                            case "save":
                                $(that.element).on("click", function (args) {
                                    var tempValidate = $(template._win.element).kendoValidator().data("kendoValidator"), status = $(".status");
                                    if (tempValidate.validate()) {
                                        template.save(args);
                                    }
                                    else
                                        args.preventDefault();
                                });
                                break;
                            case "cancel":
                                $(that.element).on("click", function (args) {
                                    template.cancel(args);
                                });
                                break;
                        }
                    }
                },
                refresh: function () {
                }
            });
            kendo.bind($(this._treeTemplateName), model);
        };
        Temp.prototype.save = function (args) {
            if (this._model.level() > 0) {
                var parentModel = this._model.parentNode();
                parentModel.HasChildren = parentModel.hasChildren;
                var operation = this._model.isNew() ? "create" : "update";
                parentModel.children.transport.parameterMap = this._setParams;
                parentModel.children.sync();
            }
            else {
                this._dataSource.sync();
            }
            this._win.close();
        };
        Temp.prototype.cancel = function (args) {
            this._dataSource.cancelChanges();
            this._win.close();
        };
        return Temp;
    }());
    function show(viewModelName, title, elementName, treeTemplateName, httpType, helpUrl, width, height) {
        var displayTemplat = new Temp();
        displayTemplat._tree = $("#" + elementName).data("kendoTreeView");
        displayTemplat._dataSource = displayTemplat._tree.dataSource;
        displayTemplat._treeTemplateName = (treeTemplateName == "" || treeTemplateName == undefined) ? "body" : treeTemplateName;
        displayTemplat._width = (typeof width == "number" ? width : 500);
        displayTemplat._height = (typeof height == "number" ? height : 300);
        displayTemplat._show(viewModelName, title, httpType, helpUrl);
    }
    Template.show = show;
    function setParams(data, operation) {
        var additionalTemplate = new Temp();
        return additionalTemplate._setParams(data, operation);
    }
    Template.setParams = setParams;
})(Template || (Template = {}));
