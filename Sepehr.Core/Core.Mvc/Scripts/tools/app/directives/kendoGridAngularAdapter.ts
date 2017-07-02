/// <reference path="../../gridsearch.ts" />
/// <reference path="../../../custom-types/validators.ts" />
/// <reference path="../../generaltools.ts" />

var kendoGridAngularAdapterModule = new SepehrModule.MainModule("kendoGridViewModule", ["grdSearchServiceModule", "coreValidationService"]);
kendoGridAngularAdapterModule.addDirective('gridView', ["$compile", "$http", "$q", "grdSearchService", "validate", function ($compiler, $http, $q, grdSearchService, validate) {
    function getTemplate(url) {

        var deferred = $q.defer();
        $http.get(url)
            .success(function (data) {
                deferred.resolve(data);
            })
            .error(function (result) {
            });
        return deferred.promise;
    }
    function getAdTemplate(tempAddress, viewModelTypeFullName) {

        var deferred = $q.defer();
        $http({
            url: "/core/Template/Get",
            method: "GET",
            params: { templateUrl: tempAddress, viewModelFullName: viewModelTypeFullName }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (result) {
        });
        return deferred.promise;
    }
    return {
        restrict: "E",
        scope: {
            gridName: "@",
            viewModelType: "@",
            viewModelProperty: "@",
            dtoType: "@",
            customAction: "=",
            onNewBtnClicked: "&",
            onEditBtnClicked: "&",
            onDeleteBtnClicked: "&",
            onRefresh: "&",
            tempUrl: "@",
            onEdit: "&",
            onLoad: "&",
            onDataBound: "&",
            onDataBinding: "&",
            onPreSave: "&",
            onChange: "&",
            onInit: "&",
            onPreDeleteCallback: "&",
            onPreUpdate: "&",
            onDoubleClick: "&",
            onPostDblclick: "&",
            htmlAttributes: "@",
            initialFilter: "=",
            cssStyles: "@",
            onUpdateStyle: "@",
            lazyLoadCallback: "&",
            onDeleteStyle: "@",
            onRefreshStyle: "@",
            onSearchStyle: "@",
            height: "@",
            width: "@",
            aeWidth: "@",
            aeHeight: "@",
            isLookup: "@",
            wiPropName: "@",
            requestParameters: "=",
            scrollPosition: "@"
        },
        replace: true,
        controller: function ($scope, $element) {
        },
        link: function (scope, elem, attrs) {


            scope.isLookup = !scope.isLookup ? null : scope.isLookup;
            scope.wiPropName = !scope.wiPropName ? null : scope.wiPropName;
            var grdJsnUrl = "/core/Template/GetGridTotalConfigurationOptions?gName=" + scope.gridName + "&viewModel=" + scope.viewModelType + "&property=" + (scope.viewModelProperty ? scope.viewModelProperty : "ViewInfo");
            if (scope.onDeleteCallback) {
                grdJsnUrl += "&onDelete=" + scope.onDeleteCallback;
            }
            if (scope.wiPropName) {

                grdJsnUrl += "&lkpPropName=" + scope.wiPropName;
            }
            if (scope.isLookup) {
                grdJsnUrl += "&isLookup=" + scope.isLookup;
            }
            var grdAddEditTempl = "";
            var hasSearch = false;
            var dom = elem;
            //var dtoType = scope.dtoType;
            var viewModelTypeFullName = scope.viewModelType;

            var gridObj = getTemplate(grdJsnUrl);
            var addEditGlossary = { add: { title: "جدید", update: "تایید", cancel: "انصراف" }, edit: { title: "ویرایش", update: "تایید", cancel: "انصراف" } };

            gridObj.then(function (res) {
                var grid = null;
                var addEdit = false;
                var remove = false;
                var winAE = null;
                var winDelete = null;
                //var modelsAddedSofar = [];
                var model = null;
                scope.toolbarTemplate = makeToolbarActions().html();
             
                scope.ajaxSuccess = false;
                scope.selectedItem = null;
                scope.isNewItemCommitted = true;
                var selectedInitialItem = null;
                var tempAddress = res["adTempAdd"];
                var domAttrs = attrs;
                var dtoType = res["dtoType"];
                dtoType = (dtoType === "undefined" || !dtoType) ? null : dtoType;
                var gridAddEditTempl;

                var initCreate = res["dataSource"]["transport"].create;
                scope.view = {};

                res.cache = false;

                if (attrs.requestParameters) {
                    res.dataSource.transport.read.data = function () {

                        var data = {};
                        for (var param in scope.requestParameters) {
                            data[param] = scope.requestParameters[param];
                        }
                        return data;
                    }
                }
                res["editable"] = null; //{ mode: null, createAt: "bottom" };
                res["dataSource"]["transport"].cache = true;
                res["dataSource"]["transport"].read.cache = false;
                //res.dataSource.transport.parameterMap = function (a, b, c) {


                //}
                //res.dataSource.requestStart = function (args) {

                //};
                //res.dataSource.requestEnd = function (args) {

                //};
                //res.excelExport = (args) => {

                //};

                var initUpdt = res["dataSource"]["transport"].update;
                res["dataBound"] = function (e) {


                    if (domAttrs.onDoubleClick) {
                        angular.forEach(e.sender.table.find("tr"), function (item) {
                            var itm = $(item).attr("ng-dblclick", "onDoubleClick({args:{scope:this, trUId:'" + $(item).attr('data-uid') + "'}})");
                            itm.find("td > span").removeAttr("ng-bind");
                            $compiler(itm)(scope);
                        });
                    }


                    scope.callOnDataBound();
                };
                scope.callOnDataBound = function () {

                    if (domAttrs.onDataBound) {
                        scope.onDataBound({ args: { scope: scope } });
                    }
                };
                res["dataBinding"] = function (e) {
                    scope.onDataBinding({ args: scope });
                };
                res["edit"] = function (e) {
                    if (scope.onEdit) {
                        scope.onEdit({ args: scope });
                    }
                };

                res["change"] = function (e) {
                    if (scope.onChange) {
                        scope.onChange({ args: scope });
                    }
                };


                if (scope.initialFilter) {
                    // res["autoBind"] = false;
                    res["dataSource"]["filter"] = scope.initialFilter;
                }

                //res.dataSource.transport.destroy = function (options) {
                //   
                //    //    options.success(response.Data[0]);
                //    //var cacheInfo = getCurrentCacheInfo().info;
                //    //$.each(cacheInfo.Data,(index, value) => {
                //    //    if (value[getModelIdName()] == response.Data[0][getModelIdName()]) {
                //    //        cacheInfo.Data.splice(index, 1);
                //    //        cacheInfo.Total -= 1;
                //    //        return false;
                //    //    }
                //    //});
                //    //debugger;
                //    //scope[scope.gridName].dataSource.success(cacheInfo);
                //};
                scope.openWin = (arg) => {
                  
                    //arg.sender.element.parent().addClass("modal-fixing");
                   // return false;
                };
                scope.activateWin = (arg) => {
                  
                  //  arg.sender.element.parent().attr("modal-fixing");
                    // return false;
                };
                if (addEdit) {
                    var aeWidth = !scope.aeWidth ? 410 : scope.aeWidth;
                    var aeHeight = !scope.aeHeight ? 100 : scope.aeHeight;
                    winAE = "<div kendo-window='winAe' k-modal='true' " + "k-width='" + aeWidth + "'  k-height='" + aeHeight + "' k-visible='false' " + 'k-on-open="win2visible=true" k-on-close="closeAdwin()" > </div>';
                    //winAE = "<div kendo-window='winAe' k-modal='true' k-height='" + aeHeight + "' k-visible='false' " + 'k-on-activate="activateWin(kendoEvent)" k-on-open="openWin(kendoEvent)" k-on-close="closeAdwin()" > </div>';

                    scope.closeAdwin = function () {
                        if (scope.ajaxSuccess == false && scope.opt == 'add') {
                            scope[scope.gridName].dataSource.remove(model);
                        }

                        else if (selectedInitialItem && scope.opt === 'edit' && scope.ajaxSuccess === false) {

                            var selItem = scope[scope.gridName].select().first().is("td") ? scope[scope.gridName].select().first().parent("tr") : scope[scope.gridName].select().first();
                            var selected = scope[scope.gridName].dataItem(selItem);
                            var changeDsArray = scope[scope.gridName]._data;
                            for (var i = 0; i < changeDsArray.length; i++) {
                                if (changeDsArray[i].uid == selectedInitialItem.uid) {
                                    selected = jQuery.extend(true, changeDsArray[i], selectedInitialItem);
                                    break;
                                }
                            }
                        }
                    };
                    scope.cancelItem = function () {
                        scope[scope.gridName].dataSource.cancelChanges(scope.selectedItem);
                        //if (scope.opt == "add") {
                        //    scope[scope.gridName].dataSource.remove(scope.selectedItem);
                        //    scope[scope.gridName].table.find("tr[data-uid=" + scope.selectedItem.uid+"]").remove();
                        //}
                        scope.opt = "",
                            scope.selectedItem = undefined;
                        //scope.view = undefined;

                        scope.winAe.close();
                    };
                    scope.editItem = function (opt) {

                        if (validate(scope.winAe.element) === true) {

                            callServer(opt, scope.winAe.element);
                        }
                    };
                }
                function makeToolbarActions() {
                    var initTlbr = res["toolbar"];
                    delete res["toolbar"];
                    var toolbarTemplate = $("<div class='toolbar'> </div>");
                    for (var i = 0; i < initTlbr.length; i++) {
                        switch (initTlbr[i].name) {
                            case "create":
                                var newBtn = '<button kendo-button ng-click="newItemWin()"> <span class="k-icon k-i-plus" ></span>' + initTlbr[i].text + '</button>';
                                toolbarTemplate.append(newBtn);
                                assignNewCallback();
                                break;
                            case "edit":
                                var editBtn = '<button kendo-button ng-click="editItemWin()"> <span class="k-icon k-i-pencil" ></span>' + initTlbr[i].text + '</button>';
                                toolbarTemplate.append(editBtn);
                                assignEditCallback();
                                break;
                            case "destroy":
                                var deleteBtn = '<button kendo-button ng-click="openDeleteWindow()"> <span class="k-icon k-delete" ></span>' + initTlbr[i].text + '</button>';
                                toolbarTemplate.append(deleteBtn);
                                assignDestroyCallbacks();
                                break;
                            case "search":
                                var searchBtn = '<button kendo-button ng-click="search()"> <span class="k-icon k-i-search" ></span>' + initTlbr[i].text + '</button>';
                                toolbarTemplate.append(searchBtn);
                                assignSearchCallback();
                                break;
                            case "refresh":
                                var refBtn = '<button kendo-button ng-click="refresh()"> <span class="k-icon k-i-refresh" ></span></button>';
                                toolbarTemplate.append(refBtn);
                                assignRefreshCallback();
                                break;
                            case "removeFilters":
                                var removeFilterBtn = '<button kendo-button ng-click="removeFilters()"> <span class="k-icon k-i-funnel-clear" ></span>' + initTlbr[i].text + '</button>';
                                toolbarTemplate.append(removeFilterBtn);
                                assignRemoveFilterBtnCallback();
                                break;
                            case "excel":

                                var excelBtn = '<button kendo-button ng-click="exportToExcel()"> <span class="k-icon export-excel" ></span>' + initTlbr[i].text + '</button>';
                                toolbarTemplate.append(excelBtn);
                                scope.exportToExcel = () => {
                                    scope[scope.gridName].saveAsExcel();
                                };
                                break;
                            case "pdf":
                                //var pdfBtn = '<script scr="~/Areas/Core/scripts/lib/pako_deflate.min.js"></script> <button kendo-button ng-click="exportToPDF()"> <span class="k-icon export-pdf" ></span>' + initTlbr[i].text + '</button>';
                                var pdfBtn = '<button kendo-button ng-click="exportToPDF()"> <span class="k-icon export-pdf" ></span>' + initTlbr[i].text + '</button>';

                                toolbarTemplate.append(pdfBtn);
                                scope.exportToPDF = () => {

                                    //scope[scope.gridName].pdf.allPages = true,
                                    // scope[scope.gridName].pdf.paperSize = "A4",
                                    // scope[scope.gridName].pdf.margin = { top: "3cm", right: "1cm", bottom: "1cm", left: "1cm" };
                                    //scope[scope.gridName].pdf.landscape= true,


                                    scope[scope.gridName].saveAsPDF();
                                };
                                break;
                            case "customAction":
                               
                                var custElem = '<button kendo-button ng-click="handleCustomAction(\'' + initTlbr[i].clickHandler + '\')"> <span></span>' + initTlbr[i].text + '</button>';

                                toolbarTemplate.append(custElem);

                                scope.handleCustomAction = (funcName) => {
                                    scope.$parent[funcName](scope);
                                };
                                break;
                        }
                    }

                    return toolbarTemplate;
                }

                function assignNewCallback() {
                    scope.newItemWin = function () {
                        setAdTemplate('add');
                        scope.winAe.open();
                    };
                    addEdit = true;
                }
                function assignEditCallback() {
                    scope.editItemWin = function () {
                        var selected = scope[scope.gridName].select();
                        if (selected && selected.length > 0) {
                            setAdTemplate('edit');
                            scope.winAe.open();
                        }
                    };
                    addEdit = true;
                }
                function assignSearchCallback() {
                    hasSearch = true;
                    scope.search = function () {

                        grdSearchService.loadGridSearch(dom, scope, scope.gridName);
                    };
                }

                function assignRemoveFilterBtnCallback() {
                    scope.removeFilters = () => {


                        scope[scope.gridName].dataSource.filter(scope.initialFilter ? scope.initialFilter : []);
                        removeSearchTextInFooter(scope[scope.gridName].wrapper);
                    }
                };
                function removeSearchTextInFooter(container) {

                    var pager = container.find("div[data-role=pager]"); //$("[grid-name=" + scope.gridName + "] div[data-role=pager]");
                    pager.children("[class='search-text']").remove();
                };
                function assignRefreshCallback() {
                    scope.refresh = function () {
                        if (scope.onRefresh) {
                            scope.onRefresh({ args: { scope: scope } });
                        }
                        scope[scope.gridName]._requestInProgress = undefined;
                        //scope[scope.gridName].dataSource.transport.cache._store = {};
                        scope[scope.gridName].dataSource.transport.cache.clear();
                        scope.isNewItemCommitted = true;
                        scope[scope.gridName].dataSource.read();

                    };
                }
                function assignDestroyCallbacks() {
                    scope.closeDeleteWin = function () {
                    };
                    scope.openDeleteWindow = function () {
                        var selected = scope[scope.gridName].select();
                        if (selected && selected.length > 0) {
                            setDeleteRecordWindow("<p style='color:red;' >آیا از حذف رکورد مربوطه مطمئن هستید؟</p>", 'حذف', 'انصراف');
                            scope.winDelete.open();
                        }
                    };
                    winDelete = "<div kendo-window='winDelete' k-modal='true' " + "k-width='" + 410 + "'  k-height='100' k-visible='false' " + 'k-on-open="openWin(kendoEvent)" k-on-close="closeDeleteWin()" > </div>';

                    scope.deleteItem = function () {

                        var selected = scope[scope.gridName].select();
                        var selectedItems: Array<any> = [];

                        if (selected && selected.length > 0) {
                            //if (res.dataSource.batch) {

                            $.each(selected, (index, item: HTMLElement) => {
                                var selectedRow: HTMLElement = item;
                                if (res.selectable == "multiple cell" || res.selectable == "cell") {
                                    selectedRow = $(item).parent("tr")[0];
                                }
                                selectedItems.push(scope[scope.gridName].dataItem(selectedRow));
                            });


                            //}
                            // selected = scope[scope.gridName].select().first().is("td") ? scope[scope.gridName].select().first().parent("tr") : scope[scope.gridName].select().first();
                            //var selectedItem = scope[scope.gridName].dataItem(selected);
                            if (domAttrs.onPreDeleteCallback) {
                                scope.onPreDeleteCallback({ args: { scope: scope, item: res.dataSource.batch ? selectedItems : selectedItems[0] } });
                            }
                            $http({
                                url: res["dataSource"]["transport"].destroy.url,
                                method: 'DELETE',
                                data:  res.dataSource.batch ? selectedItems : selectedItems[0],
                                dataType: "json",
                                headers: { "Content-Type": "application/json;charset=utf-8" }
                            }).success(function (response, status) {
                               
                                if (status == 200 || response === "success" || response.Data && response.Data.length >= 1) {

                                    $.each(selectedItems, (selectedItemsIndex, item) => {

                                        scope[scope.gridName].dataSource.remove(item);

                                        var cacheInfo = getCurrentCacheInfo("destroy").info;
                                        $.each(cacheInfo.Data, (index, value) => {
                                            if (value[getModelIdName()] == response.Data[selectedItemsIndex][getModelIdName()]) {
                                                cacheInfo.Data.splice(index, 1);
                                                cacheInfo.Total -= 1;
                                                return false;
                                            }
                                        });

                                        scope[scope.gridName].dataSource.success(cacheInfo);
                                    });

                                    scope.winDelete.close();
                                }
                                //else {
                                //    scope.winDelete.close();
                                //}
                            }).error(function (err) {
                                //TODO: ajax call has encountered a problem and could not send deletion request to the server.
                                $.each(selectedItems, (selectedItemsIndex, item) => {
                                    scope[scope.gridName].dataSource.cancelChanges(item);
                                });
                                //scope.winDelete.close();
                            });
                        }
                    };
                    scope.cancelFromDeleting = function () {
                        scope.winDelete.close();
                    };
                    remove = true;
                }
                //function setNewlyModel(dat) {
                //    //mapServerObjectsToNewObjects(dat.Data[0], modelsAddedSofar);

                //    mapNewToCurrent(dat.Data[0]);

                //}

                //function mapServerObjectsToNewObjects(newComer, addedSofar) {


                //    mapNewToCurrent(newComer);
                //}

                function mapNewToCurrent(newItem) {


                    for (var propertyName in newItem) {

                        model.set(propertyName, newItem[propertyName]);
                    }

                    model.set("id", newItem[getModelIdName()]);
                    model.set("dirty", false);

                }

                function getModelIdName() {
                    return scope[scope.gridName].dataSource.options.schema.model.id;
                }

                function postEntity(successCallBack?: Function, errorCallback?: Function, finallyCallback?: Function) {


                    $http.post(initCreate.url, model).success(function (dat) {

                        if (dat.Data && dat.Data.length >= 1) {
                            //simple one item insertion
                            mapNewToCurrent(dat.Data[0]);

                            //scope.isNewItemCommitted = true;
                            scope.callOnDataBound();
                            // TODO : message that is created successfully.
                            scope.ajaxSuccess = true;


                        }
                        else {
                            scope[scope.gridName].dataSource.remove(model);
                            //scope.isNewItemCommitted = false;
                            scope.ajaxSuccess = false;
                            //scope.winAe.close();
                        }

                        if (successCallBack) {
                            successCallBack(dat);
                        }
                        scope.winAe.close();

                    }).error(function (err) {
                        //TODO:remove the newly inserted value from ds.
                        scope[scope.gridName].dataSource.remove(model);
                        scope.isNewItemCommitted = false;
                        scope.ajaxSuccess = false;
                        //scope.winAe.close();
                        if (errorCallback) {
                            errorCallback(err);
                        }
                    }).finally(function (result) {
                        finallyCallback(result);
                    });

                }
                function putEntity(successCallBack?: Function, errorCallback?: Function, finallyCallback?: Function) {
                    $http.put(initUpdt.url, model).success(function (dat) {

                        if (dat.Data && dat.Data.length >= 1) {

                            var cacheInfo = getCurrentCacheInfo("update").info;
                            $.each(cacheInfo.Data, (index, value) => {
                                if (value[getModelIdName()] == dat.Data[0][getModelIdName()]) {

                                    for (var field in value) {
                                        value[field] = dat.Data[0][field];
                                    }
                                    return false;
                                }
                            });

                            scope[scope.gridName].dataSource.success(cacheInfo);
                            //setNewlyModel(dat);
                            mapNewToCurrent(dat.Data[0]);
                            scope.callOnDataBound();
                            scope.ajaxSuccess = true;

                        }
                        else {

                            scope.ajaxSuccess = false;

                        }
                        if (successCallBack) {
                            successCallBack(dat);
                        }
                        scope.winAe.close();

                    }).error(function (err) {

                        scope.ajaxSuccess = false;

                        if (errorCallback) {
                            errorCallback(err);
                        }
                    }).finally(function (result) {
                        finallyCallback(result);
                    });
                    ;
                }

                function getCurrentCacheInfo(crudType = "read") {
                    var pageSize = scope[scope.gridName].dataSource.pageSize(),
                        parameters = {
                            sort: scope[scope.gridName].dataSource.sort(),
                            page: scope[scope.gridName].dataSource.page(),
                            pageSize: scope[scope.gridName].dataSource.pageSize(),
                            group: scope[scope.gridName].dataSource.group(),
                            filter: scope[scope.gridName].dataSource.filter()
                        };
                    var cacheKey = scope[scope.gridName].dataSource.transport.parameterMap(parameters, crudType);

                    var cacheData = scope[scope.gridName].dataSource.transport.cache.find(cacheKey);

                    return {
                        info: cacheData,
                        key: cacheKey
                    }
                }
                function progressBar(container: JQuery, show: boolean) {

                    kendo.ui.progress(container, show);
                }

                function callServer(opt, container) {

                    progressBar(container, true);


                    if (domAttrs.onPreSave) {
                        scope.onPreSave({ args: { grid: scope[scope.gridName], isNew: opt === 'add' ? true : false, model: model } });
                    }
                    if (opt === 'add') {

                        var finallyCallbackFunc = () => { progressBar(container, false); },
                            succesCallbackFunc = (result) => {
                                var entityList: Array<any> = result.Data;

                                if (scope.scrollPosition == "bottom") {

                                    $q.when().then(() => {

                                        scope[scope.gridName].options.editable = { mode: "incell", createAt: "bottom" };


                                        //scope[scope.gridName].dataSource.add(model);

                                    }).then(() => {

                                        var cacheInfo = getCurrentCacheInfo(),
                                            pageSize = scope[scope.gridName].dataSource.pageSize();

                                        if (res.dataSource.batch == false) {

                                            if (cacheInfo.info.Data.length < pageSize) {

                                                addResultToCache(cacheInfo, entityList[0]);
                                            }
                                            else {
                                                addResultToCache(cacheInfo, entityList[0], true)
                                            }
                                        }
                                        //when we have bulk insert in bottom
                                        else {

                                            bulkInsert(pageSize, cacheInfo, entityList, false);
                                        }
                                        setScrollPosition();
                                    });
                                }

                                else if (scope.scrollPosition == "top" /*&& res.dataSource.batch == true*/) {

                                    var cacheInfo = getCurrentCacheInfo(),
                                        pageSize = scope[scope.gridName].dataSource.pageSize();
                                    scope[scope.gridName].options.editable = { mode: "incell", createAt: "top" };

                                    bulkInsert(pageSize, cacheInfo, entityList);
                                }


                            }
                        postEntity(succesCallbackFunc, null, finallyCallbackFunc);

                    }
                    else if (opt === 'edit') {
                        var selectedRow = scope[scope.gridName].select(),
                            selectedItem = scope[scope.gridName].dataItem(selectedRow),
                            finallyCallbackFunc = () => {
                                progressBar(container, false);

                                var uidRow = scope[scope.gridName].dataSource.get(selectedItem.id).uid,
                                    row = scope[scope.gridName].table.find("tr[data-uid='" + uidRow + "']");
                                scope[scope.gridName].select(row);
                            };

                        putEntity(null, null, finallyCallbackFunc);
                    }
                }

                function setDeleteRecordWindow(content, deleteText, cancelText) {

                    scope.winDelete.content("<div class='k-edit-form-container' >" + content + "</div>");
                    var buttonActions = $('<div class="k-edit-buttons k-state-default" >' +
                        '<button class="k-button k-button-icontext k-primary k-grid-delete" ng-click="deleteItem()" autofocus="true" >' +
                        deleteText + '</button> ' + '<button class="k-button k-button-icontext k-grid-cancel" ng-click="cancelFromDeleting()" >' +
                        cancelText + '</button> ' + '</div>');
                    scope.winDelete.element.find("div.k-edit-form-container").append(buttonActions);

                    $compiler(scope.winDelete.element.find("div.k-edit-buttons"))(scope);
                }

                function addRecordTospecificLocation(model) {
                    var newlyInserted = null;

                    //if (scope.scrollPosition == "bottom") {

                    //    newlyInserted = model;


                    //}
                    //else {
                    //    newlyInserted = scope[scope.gridName].dataSource.insert(0, model);

                    //}

                    if (scope.scrollPosition == "top" && res.dataSource.batch == false) {
                        newlyInserted = scope[scope.gridName].dataSource.insert(0, model);
                    }
                    else {
                        newlyInserted = model;
                    }
                    // modelsAddedSofar.push(newlyInserted.uid);
                    scope.selectedItem = newlyInserted;
                    scope.view = scope.selectedItem;


                }
                function setAdTemplate(adOrEdit) {

                    model = null;
                    scope.ajaxSuccess = false;
                    var addEditObj = adOrEdit === 'add' ? addEditGlossary.add : addEditGlossary.edit;
                    scope.winAe.title(addEditObj.title);
                    scope.opt = adOrEdit;
                    scope.isNewItemCommitted = false;
                    if (adOrEdit == 'add') {

                        model = new scope[scope.gridName].dataSource.reader.model()

                        addRecordTospecificLocation(model);
                    }
                    else if (adOrEdit == 'edit') {
                        var selected = scope[scope.gridName].select().first().is("td") ? scope[scope.gridName].select().first().parent("tr") : scope[scope.gridName].select().first();
                        var selectedItem = scope[scope.gridName].dataItem(selected);
                        model = selectedItem;
                        selectedInitialItem = clone(scope[scope.gridName].dataItem(selected));//jQuery.extend(true, {}, scope[scope.gridName].dataItem(selected));
                        //viewModel = kendo.observable({
                        //    data: selectedItem
                        //});
                        scope.selectedItem = selectedItem;
                        scope.view = scope.selectedItem;
                    }


                    scope.winAe.content("<div class='k-edit-form-container' >" + gridAddEditTempl + "</div>");

                    var buttonActions = $('<div class="k-edit-buttons k-state-default" >' + '<button class="k-button k-button-icontext k-primary mainbtn k-grid-update" ng-click="editItem(opt)" >' + addEditObj.update + '</button> ' + '<button class="k-button k-button-icontext k-grid-cancel" ng-click="cancelItem()" >' + addEditObj.cancel + '</button> ' + '</div>');

                    var container = scope.winAe.element.find("div.k-edit-form-container");
                    container.append(buttonActions),
                        $compiler(scope.winAe.element.find("div.k-edit-buttons"))(scope);

                    $.each(scope[scope.gridName].columns, (i, col) => {

                        var validationAttrs: Array<ValidationAttribute> = new Array<ValidationAttribute>();

                        for (var rule in col.validationRules) {

                            var params: Array<any> = [];

                            for (var input in col.validationRules[rule]["params"]) {
                                var field = col.validationRules[rule]["params"][input];

                                if (scope.view.hasOwnProperty(field))
                                    params.push("view['" + field + "']");
                                else
                                    params.push(col.validationRules[rule]["params"][input])
                            }

                            validationAttrs.push({ type: rule, message: col.validationRules[rule].message, params: params });

                        }

                        if (validationAttrs.length > 0) {
                            var relatedElement = scope.winAe.element.find("#" + col.field);

                            if (relatedElement.length > 0 && !relatedElement.attr("price-format")) {
                                // relatedElement = scope.winAe.element.find("[lookup-id=" + col.field + "]");
                                relatedElement.attr("validations", JSON.stringify(validationAttrs));
                                $compiler(relatedElement)(scope);
                            }
                            else {
                                setValidationForDirective(scope.winAe.element, col, validationAttrs);
                            }


                        }
                    });
                    function setScopeForDirective(currentScope, newElement) {
                        for (var property in angular.element(newElement).scope()) {

                            if (property.charAt(0) !== "$" && property !== "validators" && property !== "constructor") {

                                currentScope[property] = angular.element(newElement).scope()[property];
                            }
                        }
                    }


                    function getDirectiveInfo(id: string, validationsValue: Array<any>): any {
                        var childScope = scope.$new(true, scope),
                            newElement: JQuery, relatedElement: JQuery,
                            hasTemplate: boolean = false;

                        childScope.validators = validationsValue;
                        childScope.view = scope.view;

                        //date picker
                        if (container.find("[input-id=" + id + "] input.hasDatepicker").length > 0) {

                            relatedElement = container.find("[input-id=" + id + "] input.hasDatepicker");

                            //childScope.inputId = relatedElement.parent().attr("input-id");

                            newElement = relatedElement.attr("validators", "validators");

                            setScopeForDirective(childScope, relatedElement);
                        }
                        //lookup
                        else if (container.find("[lookup-id=" + id + "]").length > 0) {

                            relatedElement = container.find("[lookup-id=" + id + "]"),
                                newElement = relatedElement.clone(true, true).attr("validators", "validators");

                        }
                        //price format 
                        else if (container.find("[price-format][id=" + id + "]").length > 0) {


                            relatedElement = container.find("[price-format][id=" + id + "]"),
                                newElement = relatedElement.attr("validations", "validators");
                            //childScope.priceValue = scope[relatedElement.attr("priceValue")]
                            //hasTemplate = true;
                            //
                            //childScope.priceValue = angular.element(newElement).scope().$eval("price-value");
                            // setScopeForDirective(childScope, newElement);
                        }

                        //newElement = newElement.attr("validators", "validators");

                        //if (hasTemplate) {
                        //    setScopeForDirectiveHasTemplatePge(childScope, relatedElement);
                        //}
                        //else {
                        //    setScopeForDirective(childScope, newElement);
                        //}

                        return {
                            element: newElement,
                            scope: childScope,
                            currentElement: relatedElement
                        }
                    }

                    function setValidationForDirective(container, col, validationsValue) {

                        var directiveInfo = getDirectiveInfo(col.field, validationsValue),
                            compiledDirective = $compiler(directiveInfo.element)(directiveInfo.scope);
                        if (compiledDirective.data()) {
                            directiveInfo.currentElement.replaceWith(compiledDirective);
                        }


                    }



                    if (attrs.onEdit) {
                        //if (scope.winAe.wrapper.width() <= 560) {
                        //    scope.winAe.wrapper.width(300);
                        //}
                        scope.onEdit({ args: { contentDom: scope.winAe.element.find("div.k-edit-form-container"), item: scope.selectedItem, sc: scope, isNew: adOrEdit === 'add' ? true : false } });
                    }
                }

                scope.gridOptions = res;

                var kendoGridElem = "<div kendo-grid='" + scope.gridName + "' options='gridOptions'  k-toolbar='[{ template: toolbarTemplate }]' ></div>";
                dom.html(kendoGridElem);
                if (addEdit) {
                    getAdTemplate(tempAddress, !dtoType ? viewModelTypeFullName : dtoType).then(function (dat) {
                        gridAddEditTempl = dat;

                    });

                    dom.append(winAE);
                }

                if (remove) {
                    dom.append(winDelete);
                }

                if (scope.onLoad) {

                    scope.onLoad({ args: { options: res } });
                }
                $compiler(dom)(scope);



            });




            scope.$on("kendoWidgetCreated", function (options, widget) {
                if (widget.options.name === "Grid") {

                    if (!ns_Search.widgetsSofar[scope.$id]) {
                        ns_Search.widgetsSofar[scope.$id] = { name: scope.gridName, atts: attrs, scp: scope, dom: dom };

                        var current = ns_Search.widgetsSofar[scope.$id];
                        scope.scrollPosition = scope.scrollPosition ? scope.scrollPosition : scope[scope.gridName].options.scrollable.position;
                        if (current.atts.onInit) {

                            current.scp.onInit({ args: current.scp });

                        }

                        //setShortcutKeyOnGrid($(current.dom).find("[kendo-grid]").data("kendoGrid"));
                        setShortcutKeyOnGrid(widget.element.data("kendoGrid"));
                        if (current.atts.height && current.atts.height != '') {
                            current.dom.find("div.k-grid-content").css("height", current.scp.height);
                        }

                        if (current.atts.width && current.atts.width != '') {
                            current.dom.find("div.k-grid-content").css("width", current.scp.width);
                        }
                        //widget.virtualScrollable.wrapper.css("height", "auto");
                        widget.content.find(".k-scrollbar").one("DOMSubtreeModified", function (events) {

                            setScrollPosition();
                        });
                        for (var item in ns_Search.widgetsSofar) {
                            var c = ns_Search.widgetsSofar[item];

                            if (c["name"] == scope.gridName && item != scope.$id) {
                                delete ns_Search.widgetsSofar[item];
                            }
                        }
                    }


                }

            });
            function addResultToCache(cacheInfo, entity, nextPage = false) {
                if (!nextPage) {
                    cacheInfo.info.Data.push(entity);
                    cacheInfo.info.Total += 1;
                    scope[scope.gridName].dataSource.success(cacheInfo.info);
                    //scope[scope.gridName].dataSource.transport.read(cacheData);
                }
                else {
                    cacheInfo.key.page = cacheInfo.key.page + 1;
                    scope[scope.gridName].dataSource.transport.cache.add(cacheInfo.key, entity);
                    //cacheData = scope[scope.gridName].dataSource.transport.cache.find(cacheKey);

                    scope[scope.gridName].dataSource.read();
                }
            }

            function bulkInsert(pageSize, cacheInfo, entityList, top: boolean = true) {

                var remainingRecords = pageSize - (cacheInfo.info ? cacheInfo.info.Data.length:0),
                    lastInsertedIndex = 0,
                    pageNo = entityList.length >= pageSize ? Math.ceil(entityList.length / pageSize) : 0,
                    mod = entityList.length > pageSize ? entityList.length % pageSize : 0;

                //add records in the remaining records of current page
                if (entityList.length < remainingRecords) {

                    for (var i = 0; i < entityList.length; i++) {

                        if (top) {
                            scope[scope.gridName].dataSource.insert(0, entityList[i]);

                        }
                        else {
                            scope[scope.gridName].dataSource.add(entityList[i]);
                            addResultToCache(cacheInfo, entityList[i])
                        }
                        lastInsertedIndex += 1;


                    }
                }
                //entity list is bigger than remainin records so add records as many as we can
                else {
                    for (var i = 0; i < remainingRecords; i++) {

                        if (top) {
                            scope[scope.gridName].dataSource.insert(0, entityList[i])
                        }
                        else {
                            scope[scope.gridName].dataSource.add(entityList[i]);
                            addResultToCache(cacheInfo, entityList[i])
                        }
                        lastInsertedIndex += 1;


                    }

                    if (mod == 0) {

                        mod = entityList.length - lastInsertedIndex;
                    }
                }
                //add records in new pages
                for (var i = 0; i < pageNo; i++) {

                    cacheInfo.key.page = cacheInfo.key.page + 1;

                    for (var j = 0; j < pageSize; i++) {


                        if (top) {
                            scope[scope.gridName].dataSource.insert(0, entityList[lastInsertedIndex])
                        }
                        else {
                            scope[scope.gridName].dataSource.add(entityList[lastInsertedIndex]);
                            scope[scope.gridName].dataSource.transport.cache.add(cacheInfo.key, entityList[lastInsertedIndex]);
                            scope[scope.gridName].dataSource.read();

                        }


                        lastInsertedIndex += 1;

                    }
                }
                //add records if we have Modulo 
                if (mod > 0) {
                    cacheInfo.key.page = cacheInfo.key.page + 1;
                    for (var i = 0; i < mod; i++) {

                        if (top) {
                            scope[scope.gridName].dataSource.insert(0, entityList[lastInsertedIndex])
                        }
                        else {
                            scope[scope.gridName].dataSource.add(entityList[lastInsertedIndex]);
                            scope[scope.gridName].dataSource.transport.cache.add(cacheInfo.key, entityList[lastInsertedIndex]);
                            scope[scope.gridName].dataSource.read();
                        }


                        lastInsertedIndex += 1;

                    }

                }



            }

            function setScrollPosition() {



                if (scope.scrollPosition == "bottom") {


                    var rowHeight = scope[scope.gridName].virtualScrollable.itemHeight,
                        pageSize = scope[scope.gridName].dataSource.pageSize();


                    var totalHeight = 0;
                    $.each(scope[scope.gridName].virtualScrollable.verticalScrollbar.children(), (index, scroller: Element) => {
                        totalHeight = $(scroller).height() + totalHeight;
                    });


                    scope[scope.gridName].virtualScrollable.verticalScrollbar.animate({ scrollTop: totalHeight });



                }
                else {
                    scope[scope.gridName].virtualScrollable.verticalScrollbar.animate({ scrollTop: 0 });
                }

            }
            function setShortcutKeyOnGrid(grid) {


                grid.table.on("keydown", function (e) {
                    //if (keys.indexOf(e.keyCode) >= 0) {
                    switch (e.keyCode) {
                        case 38:/*up*/
                        case 40:/*down*/
                            setTimeout(function () {

                                grid.select().removeClass("k-state-selected");
                                grid.select(grid.table.find("td.k-state-focused").closest("tr"));
                            });
                            break;
                        case 45:/*insert*/
                            try {
                                scope.newItemWin();
                            }
                            catch (ex) {
                                return;
                            }
                            break;
                        case 46:/*delete*/
                            try {
                                scope.openDeleteWindow();
                            }
                            catch (ex) {
                                return;
                            }
                            break;
                        case 113:/*f2*/
                            try {

                                scope.editItemWin()
                            }

                            catch (ex) {
                                return;
                            }
                            break;
                        case 115:/*f4*/
                            try {
                                scope.search();
                            }
                            catch (ex) {
                                return;
                            }
                            break;
                    }

                    //}
                });
            }


            //scope.$on("kendoRendered", function (event) {
            //   
            //});
        },
        template: "<div></div>"
    };
}]);
