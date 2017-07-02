
module Template {
    interface CustomDataSource extends kendo.data.DataSource {
        reader: any
        _sync(): void
    }

    class CustomNode extends kendo.data.Node {
        public Id: any
        public HasChildren: boolean
        public hasChildren: boolean
        public Node: kendo.data.Node
    }

    class CustomTreeView extends kendo.ui.TreeView {
        public dataSource: CustomDataSource
        public dataItem(e: JQuery): CustomNode {
            var superNode = super.dataItem(e);
            var derivedNode = new CustomNode();
            derivedNode.Node = superNode;
            derivedNode.Id = superNode.id;
            return derivedNode;
        }
    }

    class Temp {
        _width: number = null;
        _height: number = null;
        _tree: CustomTreeView = null;
        _dataSource: CustomDataSource = null;
        _win: kendo.ui.Window = null;
        _treeTemplateName: string = null;
        _idField: string = undefined;
        _model: any = null;

        public _show(viewModelName: string, winTitle: string, type: string, helpUrl: string) {

            var tempUrl = "/Views/Shared/EditorTemplates/" + viewModelName + "Template.cshtml";// "/Views/Lookup/_Grid.cshtml";
            if (window.areaName) {
                tempUrl = "~/Areas/" + window.areaName + tempUrl;
            }
            else {
                tempUrl = "~" + tempUrl;

            }

            var sendingdData = {
                templateUrl: tempUrl,//"~/Views/Shared/EditorTemplates/" + viewModelName + "Template.cshtml",
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

                DialogBox.ShowDialog(helpUrl, winTitle, null, 600, 400, helpData , false);

            }

        }

        public _insert() {

            var newRecord = new this._dataSource.reader.model();
            this._idField = newRecord._childrenOptions.schema.model.id;
            var selectedNode = this._tree.select();

            //insertion of node to root
            if (selectedNode.length == 0) {
                this._tree.append(newRecord);
            }
            //insertion of node as a child
            else {
                var parentModel = this._tree.dataItem(selectedNode);
                newRecord.ParentId = parentModel[this._idField];
                this._tree.append(newRecord, selectedNode);
            }

            this._bind(newRecord, false);

        }

        public _update() {
            var selectedNode = this._tree.select();
            var model: CustomNode = this._tree.dataItem(selectedNode);
            model.HasChildren = model.hasChildren;
            this._bind(model, true);
        }

        public _delete() {
            var selectedNode = this._tree.select();
            var model = this._tree.dataItem(selectedNode);
            this._tree.remove(selectedNode);
            this._bind(model, false);
        }

        public _setParams(data: kendo.data.DataSourceTransportParameterMapData, operation: string) {
           
            if (operation == 'read' && (data.filter || data.sort || data.group)) {
                var mvcAjaxTransport: kendo.data.DataSourceTransport = new kendo.data.transports["aspnetmvc-ajax"]();

                return mvcAjaxTransport.parameterMap(data, operation);
            }
            else
                return data;

        }

        public _bind(model, isInEditMode) {
            var template = this;
            template._model = model;
            kendo.data.binders.vm = kendo.data.Binder.extend({

                init: (target, bindings, options) => {

                    kendo.data.Binder.fn.init.call(kendo.data.binders.vm, target, bindings, options);

                    var that = kendo.data.binders.vm;

                    var path = bindings.vm.path;

                    if (model.isNew() && bindings.vm.source.defaults[path] == undefined) {

                        var defaultVal = '';// bindings.vm.source.defaults[path] == undefined ? '' : bindings.vm.source.defaults[path];
                        bindings.vm.set(defaultVal);

                    }

                    //In order to set value in edit mode
                    if (model != undefined && isInEditMode) {

                        $(target).val(model[path]);
                    }

                    if (target.type == "submit") {
                        switch (path) {
                            case "save":
                               
                                $(that.element).on("click", (args: JQueryEventObject) => {
                                   
                                    var tempValidate = $(template._win.element).kendoValidator().data("kendoValidator"),
                                        status = $(".status");
                                    if (tempValidate.validate()) {
                                        template.save(args);
                                    }
                                    else
                                        args.preventDefault();
                                });
                                break;

                            case "cancel":
                               
                                $(that.element).on("click", (args: JQueryEventObject) => {
                                    template.cancel(args);
                                });
                                break;

                        }

                    }
                },
                refresh: () => {

                }
                
            });
            kendo.bind($(this._treeTemplateName), model);
        }


        save(args: JQueryEventObject) {
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
        }

        cancel(args: JQueryEventObject){
                    this._dataSource.cancelChanges();
                    this._win.close();
        }

    }

    export function show(viewModelName: string, title: string, elementName: string, treeTemplateName: string, httpType: string, helpUrl: string, width: number, height: number): void {

        var displayTemplat = new Temp();

        displayTemplat._tree = $("#" + elementName).data("kendoTreeView");

        displayTemplat._dataSource = displayTemplat._tree.dataSource;

        displayTemplat._treeTemplateName = (treeTemplateName == "" || treeTemplateName == undefined) ? "body" : treeTemplateName;

        displayTemplat._width = (typeof width == "number" ? width : 500);

        displayTemplat._height = (typeof height == "number" ? height : 300);

        displayTemplat._show(viewModelName, title, httpType, helpUrl);

    }

    export function setParams(data: kendo.data.DataSourceTransportParameterMapData, operation: string) {
        var additionalTemplate = new Temp();

        return additionalTemplate._setParams(data, operation);
    }
}



