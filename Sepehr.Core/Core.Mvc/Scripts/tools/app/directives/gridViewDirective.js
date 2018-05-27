var gridViewDirectiveModule = new SepehrModule.MainModule("gridViewModule", ["gridSearchServiceModule", "coreValidationService"]);
gridViewDirectiveModule.addDirective('gridView', ["$compile", "$http", "$q", "gridSearchService", "validate", function ($compiler, $http, $q, gridSearchService, validate) {
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
                modalWidth: "@",
                modalHeight: "@",
                isLookup: "@",
                wiPropName: "@",
                requestParameters: "=",
                scrollPosition: "@",
                onRemoveFilters: "&",
                onValidate: "&"
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
                var viewModelTypeFullName = scope.viewModelType;
                var gridObj = getTemplate(grdJsnUrl);
                var addEditGlossary = { add: { title: "جدید", update: "تایید", cancel: "انصراف" }, edit: { title: "ویرایش", update: "تایید", cancel: "انصراف" } };
                gridObj.then(function (config) {
                    var grid = null;
                    var addEdit = false;
                    var remove = false;
                    var winAE = null;
                    var winDelete = null;
                    var model = null;
                    scope.toolbarTemplate = makeToolbarActions().html();
                    scope.ajaxSuccess = false;
                    scope.selectedItem = null;
                    var selectedInitialItem = null;
                    var tempAddress = config["adTempAdd"];
                    var domAttrs = attrs;
                    var dtoType = config["dtoType"];
                    dtoType = (dtoType === "undefined" || !dtoType) ? null : dtoType;
                    var gridAddEditTempl;
                    var resizable = true;
                    var Reorderable = false;
                    var initCreate = config["dataSource"]["transport"].create;
                    scope.view = {};
                    config.cache = false;
                    if (attrs.requestParameters) {
                        config.dataSource.transport.read.data = function () {
                            var data = {};
                            for (var param in scope.requestParameters) {
                                data[param] = scope.requestParameters[param];
                            }
                            return data;
                        };
                    }
                    config["editable"] = null;
                    config["dataSource"]["transport"].cache = true;
                    config["dataSource"]["transport"].read.cache = false;
                    var initUpdt = config["dataSource"]["transport"].update;
                    config["dataBound"] = function (e) {
                        if (domAttrs.onDoubleClick) {
                            angular.forEach(e.sender.table.find("tr"), function (item) {
                                var itm = $(item).attr("ng-dblclick", "onDoubleClick({args:{scope:this, trUId:'" + $(item).attr('data-uid') + "'}})");
                                itm.find("td > span").removeAttr("ng-bind");
                                $compiler(itm)(scope);
                            });
                        }
                        var parentCheckHiddenInterval = setInterval(function () {
                            if (scope[scope.gridName].element.parents(":hidden").length == 0) {
                                scope[scope.gridName].virtualScrollable.refresh();
                                clearInterval(parentCheckHiddenInterval);
                            }
                        }, 100);
                        scope.callOnDataBound();
                    };
                    scope.callOnDataBound = function () {
                        if (domAttrs.onDataBound) {
                            scope.onDataBound({ args: { scope: scope } });
                        }
                    };
                    config["dataBinding"] = function (e) {
                        scope.onDataBinding({ args: scope });
                    };
                    config["edit"] = function (e) {
                        if (scope.onEdit) {
                            scope.onEdit({ args: scope });
                        }
                    };
                    config["change"] = function (e) {
                        if (scope.onChange) {
                            scope.onChange({ args: scope });
                        }
                    };
                    if (scope.initialFilter) {
                        config["dataSource"]["filter"] = scope.initialFilter;
                    }
                    scope.openWin = function (arg) {
                    };
                    scope.activateWin = function (arg) {
                    };
                    if (addEdit) {
                        var modalWidth = !scope.modalWidth ? 'auto' : scope.modalWidth;
                        var modalHeight = !scope.modalHeight ? 'auto' : scope.modalHeight;
                        winAE = "<div kendo-window='winAe' k-modal='true' " + "k-width='" + modalWidth + "'  k-height='" + modalHeight + "' k-visible='false' " + 'k-on-open="win2visible=true" k-on-close="closeAdwin()" > </div>';
                        scope.closeAdwin = function () {
                            if (scope.ajaxSuccess == false && scope.action == 'add') {
                                scope[scope.gridName].dataSource.remove(model);
                            }
                            else if (selectedInitialItem && scope.action === 'edit' && scope.ajaxSuccess === false) {
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
                            scope.action = "",
                                scope.selectedItem = undefined;
                            scope.winAe.close();
                        };
                        scope.editItem = function (action) {
                            if (domAttrs.onPreSave) {
                                scope.onPreSave({ args: { grid: scope[scope.gridName], isNew: action === 'add' ? true : false, model: model } });
                            }
                            var isValidate = validate(scope.winAe.element);
                            scope.onValidate({ args: { isValidate: isValidate, grid: scope[scope.gridName], isNew: action === 'add' ? true : false, model: model } });
                            if (isValidate === true) {
                                callServer(action, scope.winAe.element);
                            }
                        };
                    }
                    function makeToolbarActions() {
                        var initTlbr = config["toolbar"];
                        delete config["toolbar"];
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
                                    scope.exportToExcel = function () {
                                        scope[scope.gridName].saveAsExcel();
                                    };
                                    break;
                                case "pdf":
                                    var pdfBtn = '<button kendo-button ng-click="exportToPDF()"> <span class="k-icon export-pdf" ></span>' + initTlbr[i].text + '</button>';
                                    toolbarTemplate.append(pdfBtn);
                                    scope.exportToPDF = function () {
                                        scope[scope.gridName].saveAsPDF();
                                    };
                                    break;
                                case "customAction":
                                    var custElem = '<button kendo-button ng-click="handleCustomAction(\'' + initTlbr[i].clickHandler + '\')"> <span></span>' + initTlbr[i].text + '</button>';
                                    toolbarTemplate.append(custElem);
                                    scope.handleCustomAction = function (funcName) {
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
                            gridSearchService.loadGridSearch(dom, scope, scope.gridName);
                        };
                    }
                    function assignRemoveFilterBtnCallback() {
                        scope.removeFilters = function () {
                            var defaultFilters = scope.initialFilter ? scope.initialFilter : [];
                            if (scope[scope.gridName].dataSource.options.filter) {
                                scope[scope.gridName].dataSource.options.filter.filters = defaultFilters;
                            }
                            scope[scope.gridName].dataSource.filter(defaultFilters);
                            scope[scope.gridName].dataSource.filter(scope.initialFilter ? scope.initialFilter : []);
                            removeSearchTextInFooter(scope[scope.gridName].wrapper);
                            if (domAttrs.onRemoveFilters) {
                                scope.onRemoveFilters();
                            }
                        };
                    }
                    ;
                    function removeSearchTextInFooter(container) {
                        var pager = container.find("div[data-role=pager]");
                        pager.find("." + ngSearchObj.searchTextContainer.attr("class")).remove();
                    }
                    ;
                    function assignRefreshCallback() {
                        scope.refresh = function () {
                            if (scope.onRefresh) {
                                scope.onRefresh({ args: { scope: scope } });
                            }
                            scope[scope.gridName]._requestInProgress = undefined;
                            scope[scope.gridName].dataSource.transport.cache.clear();
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
                            var selectedItems = [];
                            if (selected && selected.length > 0) {
                                $.each(selected, function (index, item) {
                                    var selectedRow = item;
                                    if (config.selectable == "multiple cell" || config.selectable == "cell") {
                                        selectedRow = $(item).parent("tr")[0];
                                    }
                                    selectedItems.push(scope[scope.gridName].dataItem(selectedRow));
                                });
                                if (domAttrs.onPreDeleteCallback) {
                                    scope.onPreDeleteCallback({ args: { scope: scope, item: config.dataSource.batch ? selectedItems : selectedItems[0] } });
                                }
                                $http({
                                    url: config["dataSource"]["transport"].destroy.url,
                                    method: 'DELETE',
                                    data: config.dataSource.batch ? selectedItems : selectedItems[0],
                                    dataType: "json",
                                    headers: { "Content-Type": "application/json;charset=utf-8" }
                                }).success(function (response, status) {
                                    if (status == 200 || response === "success" || response.Data && response.Data.length >= 1) {
                                        $.each(selectedItems, function (selectedItemsIndex, item) {
                                            scope[scope.gridName].dataSource.remove(item);
                                            var cacheInfo = getCurrentCacheInfo("destroy").info;
                                            $.each(cacheInfo.Data, function (index, value) {
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
                                }).error(function (err) {
                                    $.each(selectedItems, function (selectedItemsIndex, item) {
                                        scope[scope.gridName].dataSource.cancelChanges(item);
                                    });
                                });
                            }
                        };
                        scope.cancelFromDeleting = function () {
                            scope.winDelete.close();
                        };
                        remove = true;
                    }
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
                    function postEntity(successCallBack, errorCallback, finallyCallback) {
                        $http.post(initCreate.url, model).success(function (dat) {
                            if (dat.Data && dat.Data.length >= 1) {
                                mapNewToCurrent(dat.Data[0]);
                                scope.callOnDataBound();
                                scope.ajaxSuccess = true;
                            }
                            else {
                                scope[scope.gridName].dataSource.remove(model);
                                scope.ajaxSuccess = false;
                            }
                            if (successCallBack) {
                                successCallBack(dat);
                            }
                            scope.winAe.close();
                        }).error(function (err) {
                            scope[scope.gridName].dataSource.remove(model);
                            scope.ajaxSuccess = false;
                            if (errorCallback) {
                                errorCallback(err);
                            }
                        }).finally(function (result) {
                            finallyCallback(result);
                        });
                    }
                    function putEntity(successCallBack, errorCallback, finallyCallback) {
                        $http.put(initUpdt.url, model).success(function (dat) {
                            if (dat.Data && dat.Data.length >= 1) {
                                var cacheInfo = getCurrentCacheInfo("update").info;
                                $.each(cacheInfo.Data, function (index, value) {
                                    if (value[getModelIdName()] == dat.Data[0][getModelIdName()]) {
                                        for (var field in value) {
                                            value[field] = dat.Data[0][field];
                                        }
                                        return false;
                                    }
                                });
                                scope[scope.gridName].dataSource.success(cacheInfo);
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
                    function getCurrentCacheInfo(crudType) {
                        if (crudType === void 0) { crudType = "read"; }
                        var pageSize = scope[scope.gridName].dataSource.pageSize(), parameters = {
                            sort: scope[scope.gridName].dataSource.sort(),
                            page: scope[scope.gridName].dataSource.page(),
                            pageSize: scope[scope.gridName].dataSource.pageSize(),
                            group: scope[scope.gridName].dataSource.group(),
                            filter: scope[scope.gridName].dataSource.filter()
                        };
                        var cacheKey = scope[scope.gridName].dataSource.transport.parameterMap(parameters, crudType);
                        var cacheData = new CacheInfo(), cacheKeyFoundData = scope[scope.gridName].dataSource.transport.cache.find(cacheKey);
                        if (cacheKeyFoundData) {
                            cacheData = cacheKeyFoundData;
                        }
                        return {
                            info: cacheData,
                            key: cacheKey
                        };
                    }
                    function progressBar(container, show) {
                        kendo.ui.progress(container, show);
                    }
                    function callServer(action, container) {
                        progressBar(container, true);
                        if (action === 'add') {
                            var finallyCallbackFunc = function () { progressBar(container, false); }, succesCallbackFunc = function (result) {
                                var entityList = result.Data, cacheInfo = getCurrentCacheInfo(), pageSize = scope[scope.gridName].dataSource.pageSize();
                                if (scope.scrollPosition == scrollPosittion.bottom) {
                                    $q.when().then(function () {
                                        scope[scope.gridName].options.editable = { mode: "incell", createAt: scrollPosittion.bottom };
                                    }).then(function () {
                                        if (config.dataSource.batch == false) {
                                            if (cacheInfo.info.Data.length < pageSize) {
                                                addResultToCache(cacheInfo, entityList[0], scrollPosittion.bottom);
                                            }
                                            else {
                                                addResultToCache(cacheInfo, entityList[0], scrollPosittion.bottom, true);
                                            }
                                        }
                                        else {
                                            bulkInsert(pageSize, cacheInfo, entityList, false);
                                        }
                                        setScrollPosition();
                                    });
                                }
                                else if (scope.scrollPosition == scrollPosittion.top) {
                                    scope[scope.gridName].options.editable = { mode: "incell", createAt: scrollPosittion.top };
                                    if (!config.dataSource.batch) {
                                        addResultToCache(cacheInfo, entityList[0], scrollPosittion.top);
                                    }
                                    else {
                                        bulkInsert(pageSize, cacheInfo, entityList);
                                    }
                                }
                            };
                            postEntity(succesCallbackFunc, null, finallyCallbackFunc);
                        }
                        else if (action === 'edit') {
                            var selectedRow = scope[scope.gridName].select(), selectedItem = scope[scope.gridName].dataItem(selectedRow), finallyCallbackFunc = function () {
                                progressBar(container, false);
                                var uidRow = scope[scope.gridName].dataSource.get(selectedItem.id).uid, row = scope[scope.gridName].table.find("tr[data-uid='" + uidRow + "']");
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
                        if (scope.scrollPosition == scrollPosittion.top && config.dataSource.batch == false) {
                            newlyInserted = scope[scope.gridName].dataSource.insert(0, model);
                        }
                        else {
                            newlyInserted = model;
                        }
                        scope.selectedItem = newlyInserted;
                        scope.view = scope.selectedItem;
                    }
                    function setAdTemplate(adOrEdit) {
                        model = null;
                        scope.ajaxSuccess = false;
                        var addEditObj = adOrEdit === 'add' ? addEditGlossary.add : addEditGlossary.edit;
                        scope.winAe.title(addEditObj.title);
                        scope.action = adOrEdit;
                        if (adOrEdit == 'add') {
                            model = new scope[scope.gridName].dataSource.reader.model();
                            addRecordTospecificLocation(model);
                        }
                        else if (adOrEdit == 'edit') {
                            var selected = scope[scope.gridName].select().first().is("td") ? scope[scope.gridName].select().first().parent("tr") : scope[scope.gridName].select().first();
                            var selectedItem = scope[scope.gridName].dataItem(selected);
                            model = selectedItem;
                            selectedInitialItem = clone(scope[scope.gridName].dataItem(selected));
                            scope.selectedItem = selectedItem;
                            scope.view = scope.selectedItem;
                        }
                        scope.winAe.content("<div class='k-edit-form-container' >" + gridAddEditTempl + "</div>");
                        var buttonActions = $('<div class="k-edit-buttons k-state-default" >' + '<button class="k-button k-button-icontext k-primary mainbtn k-grid-update" ng-click="editItem(action)" autoFocus >' + addEditObj.update + '</button> ' + '<button class="k-button k-button-icontext k-grid-cancel" ng-click="cancelItem()" >' + addEditObj.cancel + '</button> ' + '</div>');
                        var container = scope.winAe.element.find("div.k-edit-form-container");
                        container.append(buttonActions),
                            $compiler(scope.winAe.element.find("div.k-edit-buttons"))(scope);
                        $.each(scope[scope.gridName].columns, function (i, col) {
                            var validationAttrs = new Array();
                            for (var rule in col.validationRules) {
                                var params = [];
                                for (var input in col.validationRules[rule]["params"]) {
                                    var field = col.validationRules[rule]["params"][input];
                                    if (scope.view.hasOwnProperty(field))
                                        params.push("view['" + field + "']");
                                    else
                                        params.push(col.validationRules[rule]["params"][input]);
                                }
                                validationAttrs.push({ type: rule, message: col.validationRules[rule].message, params: params });
                            }
                            if (validationAttrs.length > 0) {
                                var relatedElement = scope.winAe.element.find("#" + col.field);
                                if (relatedElement.length > 0 && !relatedElement.attr("price-format") && relatedElement.find("input[kendo-auto-complete]").length == 0) {
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
                        function getDirectiveInfo(id, validationsValue) {
                            var childScope = scope.$new(true, scope), newElement, relatedElement, hasTemplate = false, isKendo = false;
                            childScope.validators = validationsValue,
                                childScope.view = scope.view;
                            if (container.find("[input-id=" + id + "] input.hasDatepicker").length > 0) {
                                relatedElement = container.find("[input-id=" + id + "] input.hasDatepicker"),
                                    newElement = relatedElement.attr("validators", "validators");
                                setScopeForDirective(childScope, relatedElement);
                            }
                            else if (container.find("[lookup-id=" + id + "]").length > 0) {
                                relatedElement = container.find("[lookup-id=" + id + "]"),
                                    newElement = relatedElement.clone(true, true).attr("validators", "validators");
                            }
                            else if (container.find("[price-format][id=" + id + "]").length > 0) {
                                relatedElement = container.find("[price-format][id=" + id + "]"),
                                    newElement = relatedElement.attr("validations", "validators");
                            }
                            else if (container.find("span#" + id + " input[kendo-auto-complete]").length > 0) {
                                newElement = container.find("span#" + id + " input[kendo-auto-complete]");
                                childScope = angular.element(newElement).scope();
                                childScope["validators"] = validationsValue;
                                isKendo = true;
                            }
                            return {
                                element: newElement,
                                scope: childScope,
                                isKendo: isKendo,
                                currentElement: relatedElement
                            };
                        }
                        function setValidationForDirective(container, col, validationsValue) {
                            var directiveInfo = getDirectiveInfo(col.field, validationsValue), compiledDirective = $compiler(directiveInfo.element)(directiveInfo.scope);
                            if (!directiveInfo.isKendo && compiledDirective.data()) {
                                directiveInfo.currentElement.replaceWith(compiledDirective);
                            }
                        }
                        if (attrs.onEdit) {
                            scope.onEdit({ args: { contentDom: scope.winAe.element.find("div.k-edit-form-container"), item: scope.selectedItem, sc: scope, isNew: adOrEdit === 'add' ? true : false } });
                        }
                    }
                    scope.gridOptions = config;
                    var kendoGridElem = "<div kendo-grid='" + scope.gridName + "' options='gridOptions' k-resizable='false'  k-toolbar='[{ template: toolbarTemplate }]' ></div>";
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
                        scope.onLoad({ args: { options: config } });
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
                            setShortcutKeyOnGrid(widget.element.data("kendoGrid"));
                            var bodyHeight = document.documentElement.clientHeight, footerHeight = 39;
                            var gridContentHeight = bodyHeight - (footerHeight + 110 + 37 + 31 + 15);
                            current.dom.find("div.k-grid-content").css("height", gridContentHeight);
                            if (current.atts.width && current.atts.width != '') {
                                current.dom.find("div.k-grid-content").css("width", current.scp.width);
                            }
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
                function addResultToCache(cacheInfo, entity, position, nextPage) {
                    if (nextPage === void 0) { nextPage = false; }
                    if (!nextPage) {
                        if (position == "bottom") {
                            cacheInfo.info.Data.push(entity);
                        }
                        else {
                            cacheInfo.info.Data.unshift(entity);
                        }
                        cacheInfo.info.Total += 1;
                        scope[scope.gridName].dataSource.success(cacheInfo.info);
                    }
                    else {
                        cacheInfo.key.page = cacheInfo.key.page + 1;
                        scope[scope.gridName].dataSource.transport.cache.add(cacheInfo.key, entity);
                        scope[scope.gridName].dataSource.read();
                    }
                }
                function bulkInsert(pageSize, cacheInfo, entityList, top) {
                    if (top === void 0) { top = true; }
                    var remainingRecords = pageSize - (cacheInfo.info ? cacheInfo.info.Data.length : 0), lastInsertedIndex = 0, pageNo = entityList.length >= pageSize ? Math.ceil(entityList.length / pageSize) : 0, mod = entityList.length > pageSize ? entityList.length % pageSize : 0;
                    if (entityList.length < remainingRecords) {
                        for (var i = 0; i < entityList.length; i++) {
                            if (top) {
                                scope[scope.gridName].dataSource.insert(0, entityList[i]);
                            }
                            else {
                                scope[scope.gridName].dataSource.add(entityList[i]);
                            }
                            addResultToCache(cacheInfo, entityList[i], top ? scrollPosittion.top : scrollPosittion.bottom);
                            lastInsertedIndex += 1;
                        }
                    }
                    else {
                        for (var i = 0; i < remainingRecords; i++) {
                            if (top) {
                                scope[scope.gridName].dataSource.insert(0, entityList[i]);
                            }
                            else {
                                scope[scope.gridName].dataSource.add(entityList[i]);
                                addResultToCache(cacheInfo, entityList[i], scrollPosittion.bottom);
                            }
                            lastInsertedIndex += 1;
                        }
                        if (mod == 0) {
                            mod = entityList.length - lastInsertedIndex;
                        }
                    }
                    for (var i = 0; i < pageNo; i++) {
                        cacheInfo.key.page = cacheInfo.key.page + 1;
                        for (var j = 0; j < pageSize; i++) {
                            if (top) {
                                scope[scope.gridName].dataSource.insert(0, entityList[lastInsertedIndex]);
                            }
                            else {
                                scope[scope.gridName].dataSource.add(entityList[lastInsertedIndex]);
                                scope[scope.gridName].dataSource.transport.cache.add(cacheInfo.key, entityList[lastInsertedIndex]);
                                scope[scope.gridName].dataSource.read();
                            }
                            lastInsertedIndex += 1;
                        }
                    }
                    if (mod > 0) {
                        cacheInfo.key.page = cacheInfo.key.page + 1;
                        for (var i = 0; i < mod; i++) {
                            if (top) {
                                scope[scope.gridName].dataSource.insert(0, entityList[lastInsertedIndex]);
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
                    var totalHeight = 0;
                    if (scope.scrollPosition == "bottom") {
                        $.each(scope[scope.gridName].virtualScrollable.verticalScrollbar.children(), function (index, scroller) {
                            totalHeight = $(scroller).height() + totalHeight;
                        });
                    }
                    scope[scope.gridName].virtualScrollable.verticalScrollbar.animate({ scrollTop: totalHeight });
                }
                function setShortcutKeyOnGrid(grid) {
                    grid.table.on("keydown", function (e) {
                        switch (e.keyCode) {
                            case 38:
                            case 40:
                                setTimeout(function () {
                                    grid.select().removeClass("k-state-selected");
                                    grid.select(grid.table.find("td.k-state-focused").closest("tr"));
                                });
                                break;
                            case 45:
                                try {
                                    scope.newItemWin();
                                }
                                catch (ex) {
                                    return;
                                }
                                break;
                            case 46:
                                try {
                                    scope.openDeleteWindow();
                                }
                                catch (ex) {
                                    return;
                                }
                                break;
                            case 113:
                                try {
                                    scope.editItemWin();
                                }
                                catch (ex) {
                                    return;
                                }
                                break;
                            case 115:
                                try {
                                    scope.search();
                                }
                                catch (ex) {
                                    return;
                                }
                                break;
                        }
                    });
                }
            },
            template: "<div></div>"
        };
    }]);
