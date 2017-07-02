'use strict';
var kendoGridAngularAdapterModule = new SepehrModule.MainModule("kendoGridViewModule", []);
kendoGridAngularAdapterModule.addDirective('gridView', ["$compile", "$http", "$q", "$rootScope", function ($compile, $http, $q, $rootScope) {
    function getTemplate(url) {
        var deferred = $q.defer();
        $http.get(url).success(function (data) {
            deferred.resolve(data);
        }).error(function (result) {
            debugger;
        });
        return deferred.promise;
    }
    function getAdTemplate(tempAddress, viewModelTypeFullName) {
        debugger;
        var deferred = $q.defer();
        $http({
            url: getAreaUrl("Template", "Get"),
            method: "GET",
            params: { templateUrl: tempAddress, viewModelFullName: viewModelTypeFullName }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (result) {
            debugger;
        });
        return deferred.promise;
    }
    return {
        restrict: "EA",
        scope: {
            gridName: "@",
            viewModelType: "@",
            //clientDependent: "=",
            tempUrl: "@",
            onEdit: "&",
            onDataBound: "&",
            onDataBinding: "&",
            onCreate: "&",
            onPreSave: "&",
            onInit: "&",
            onDoubleClick: "&",
            htmlAttributes: "@",
            onPreDeleteCallback: "&",
            onPreUpdate: "&",
            initialFilter: "=",
            cssStyles: "@",
            onCreateStyle: "@",
            onUpdateStyle: "@",
            onDeleteStyle: "@",
            onRefreshStyle: "@",
            onSearchStyle: "@",
            height: "@",
            width: "@",
            aeWidth: "@",
            aeHeight: "@",
            isLookup:"@",
            lkpPropName:"@"
        },

        replace: true,

        controller: function ($scope, $element) {
        },

        link: function (scope, elem, attrs) {
            scope.isLookup = !scope.isLookup ? null : scope.isLookup;
            scope.lkpPropName = !scope.lkpPropName ? null : scope.lkpPropName;
            var grdJsnUrl = "Template/GetGridTotalConfigurationOptions?gName=" + scope.gridName + "&viewModel=" + scope.viewModelType;
            if (scope.onDeleteCallback){
                grdJsnUrl + "&onDelete=" + scope.onDeleteCallback;
            }
            if (scope.lkpPropName){
                grdJsnUrl + "&lkpPropName=" + scope.lkpPropName
            }
            if (scope.isLookup){
                grdJsnUrl + "&isLookup=" + scope.isLookup;
            }
            
            var grdAddEditTempl = "";
            var dom = elem;
            
            var viewModelTypeFullName = scope.viewModelType;
            var tempAddress = !scope.tempUrl ? "~/Areas/Core/Views/Shared/EditorTemplates/" + viewModelTypeFullName.split('.')[3] + "Template.cshtml" : scope.tempUrl;
            var domAttrs = attrs;


            var gridAddEditTempl = getAdTemplate(tempAddress, viewModelTypeFullName);
            var gridObj = getTemplate(grdJsnUrl);

            var addEditGlossary = { add: { title: "جدید", update: "افزودن", cancel: "انصراف" }, edit: { title: "ویرایش", update: "اِعمال", cancel: "انصراف" } };
            var valRules = [];

            function makeValidationRules(clmns: any): void {
                for (var i = 0; i < clmns.length; i++) {
                    buildColumnValidationRules(clmns[i]);
                }
            }

            function buildColumnValidationRules(column) {
                var colValRules = column["validationRules"];
                var fldRules = {};
                fldRules[column.field] = { "required": {} };
                
                if (colValRules) {
                    //for (var j = 0; j < colValRules.length; j++) {
                    if (colValRules.required) {
                        if (colValRules.required.message)
                            fldRules[column.field]["required"] = { message: colValRules.required.message }
                        valRules.push(fldRules);
                    } 
                    //}
                    
                }
            }

            $q.all([gridObj,gridAddEditTempl]).then(function (res) {
                var grid = null;
                var addEdit = false;
                var remove = false;
                var winAE = null;
                var winDelete = null;
                var modelsAddedSofar = [];
                var model = null;
                var viewModel = null;
                scope.toolbarTemplate = makeToolbarActions().html();
                scope.win2visible = false;
                scope.ajaxSuccess = false;
                scope.selectedItem = null;
                var selectedInitialItem = null;
                
  
                var initCreate = res[0]["dataSource"]["transport"].create;
                scope.adeTempVal = null;
                // res[0]["dataSource"]["type"] = "json";
                var initUpdt = res[0]["dataSource"]["transport"].update;

                res[0]["dataBound"] = function (e) {
                    if (domAttrs.onDoubleClick) {
                        angular.forEach(e.sender.table.find("tr"), function (item) {
                            var itm = $(item).attr("ng-dblclick", "onDoubleClick({args:{scope:this, trUId:'" + $(item).attr('data-uid') + "'}})");
                            itm.find("td > span").removeAttr("ng-bind");
                            $compile(itm)(scope);
                        });
                    }
                    if (domAttrs.onDataBound) {
                        scope.onDataBound({ args: scope });
                    }
                }

                res[0]["dataBinding"] = function (e) {
                    if (scope.onDataBinding) {
                        scope.onDataBinding({ args: scope });
                    }
                }

                if (scope.initialFilter) {
                    res[0]["dataSource"]["filter"] = scope.initialFilter;
                }

                // res[1] = res[1].replace(/data-val/gm, "x").replace(/data-val-required/gm, "ox");
                //var addEdtTempl = kendo.template(res[1]);

                if (addEdit) {
                    var aeWidth = !scope.aeWidth ? 410 : scope.aeWidth;
                    var aeHeight = !scope.aeHeight ? 100 : scope.aeHeight;
                    winAE = "<div kendo-window='winAe'  " +  // k-content=\"{ url: '" + tempAdd + "' } \"
                        "k-width='" + aeWidth + "'  k-height='" + aeHeight + "' k-visible='false' " +
                    'k-on-open="win2visible=true" k-on-close="closeAdwin()" > </div>';

                    scope.closeAdwin = function () {
                        if (scope.ajaxSuccess === false && scope.opt === 'add') {
                            scope[scope.gridName].dataSource.remove(model);
                        }
                        else if (selectedInitialItem && scope.opt === 'edit') {
                            var selItem = scope[scope.gridName].select().first().is("td") ? scope[scope.gridName].select().first().parent("tr") : scope[scope.gridName].select().first();
                            var selected = scope[scope.gridName].dataItem(selItem);
                            var changeDsArray = scope[scope.gridName]._data;
                            for (var i = 0; i < changeDsArray.length; i++) {
                                if (changeDsArray[i].Id == selectedInitialItem.Id) {
                                    selected = jQuery.extend(true, changeDsArray[i], selectedInitialItem);
                                    break;
                                }
                            }
                        }
                    }

                    scope.cancelItem = function () {
                        scope.winAe.close();
                    }

                    scope.editItem = function (opt) {
                        
                        var validator = $('div.k-edit-form-container').kendoValidator().data('kendoValidator')
                        if (validator.validate() === true) {
                            if (domAttrs.onPreSave) {
                                if (scope.onPreSave({ args: scope })) {
                                    doUpdate(opt);
                                }
                                else {
                                    console.log("preSave function has returned false or not correctly being implemented.")
                                }
                            }
                            else {
                                doUpdate(opt);
                            }
                        }
                    }

                }

                function makeToolbarActions() {
                    var initTlbr = res[0]["toolbar"];
                    delete res[0]["toolbar"];
                    var toolbarTemplate = $("<div class='toolbar'> </div>");//$("<script type='text/x-kendo-template'> <div class='toolbar'> </div></script>");
                    for (var i = 0; i < initTlbr.length; i++) {
                        switch (initTlbr[i].name) {
                            case "create":
                                var newBtn = '<button kendo-button ng-click="newItemWin()"> <span class="k-icon k-i-plus" ></span>جدید</button>';
                                toolbarTemplate
                                    .append(newBtn);
                                assignNewCallback();
                                break;
                            case "edit":
                                var editBtn = '<button kendo-button ng-click="editItemWin()"> <span class="k-icon k-i-pencil" ></span>ویرایش</button>';
                                toolbarTemplate
                                    .append(editBtn);
                                assignEditCallback();
                                break;
                            case "destroy":
                                var deleteBtn = '<button kendo-button ng-click="openDeleteWindow()"> <span class="k-icon k-delete" ></span>حذف</button>';
                                toolbarTemplate
                                //.find("div.toolbar")
                                    .append(deleteBtn);
                                assignDestroyCallbacks();

                                break;
                            case "search":
                                var searchBtn = '<button kendo-button ng-click="search()"> <span class="k-icon k-i-search" ></span>جستجو</button>';
                                toolbarTemplate
                                //.find("div.toolbar")
                                    .append(searchBtn);
                                assignSearchCallback();
                                break;
                            case "refresh":
                                var refBtn = '<button kendo-button ng-click="refresh()"> <span class="k-icon k-i-refresh" ></span></button>';
                                toolbarTemplate
                                //.find("div.toolbar")
                                    .append(refBtn);
                                assignRefreshCallback();
                                break;
                        }
                    }
                    return toolbarTemplate;
                }

                function assignNewCallback() {
                    scope.newItemWin = function () {
                        setAdTemplate(res[1], 'add');
                        scope.winAe.center().open();
                    };
                    addEdit = true;
                }

                function assignEditCallback() {
                    scope.editItemWin = function () {
                        setAdTemplate(res[1], 'edit');
                        scope.winAe.center().open();
                    }
                    addEdit = true;
                }

                function assignSearchCallback() {
                    scope.search = function () {

                    }
                }

                function assignRefreshCallback() {
                    scope.refresh = function () {
                       
                        if (scope[scope.gridName].dataSource.transport.cache._store) scope[scope.gridName].dataSource.transport.cache._store = {};
                        scope[scope.gridName].dataSource.read();
                        //if (scope.initialFilter) {
                        //      ns_Search.setGridInitialFilterRule(scope.gridName, scope.initialFilter);
                        //      ns_Grid.GridOperations.doWithInitialOrClearFilter(scope.gridName, true);
                        //}
                        //else {
                        //    if (ns_Grid.GridOperations.ifRefreshCanApplyAccordingToFilter(scope.gridName)) {
                        //        ns_Grid.GridOperations.doWithInitialOrClearFilter(scope.gridName, false);
                        //    }
                        //    else {
                        //        ns_Grid.GridOperations.doWithInitialOrClearFilter(scope.gridName, true);
                        //    }
                        //}
                    }
                }

                function assignDestroyCallbacks() {

                    scope.closeDeleteWin = function () {
                    }

                    scope.openDeleteWindow = function () {
                        if (scope[scope.gridName].select()) {
                            setDeleteRecordWindow("<p style='color:red;' >آیا از حذف رکورد مربوطه مطمئن هستید؟</p>", 'حذف', 'انصراف');
                            scope.winDelete.center().open();
                        }
                    }

                    winDelete = "<div kendo-window='winDelete'  " +
                    "k-width='" + 410 + "'  k-height='100' k-visible='false' " +
                    'k-on-open="win2visible=true" k-on-close="closeDeleteWin()" > </div>';

                    scope.deleteItem = function () {
                        var selected = scope[scope.gridName].select();
                        //scope.selRec = selected;
                        if (selected) {
                            if (domAttrs.onPreDeleteCallback) {
                               
                                selected = scope[scope.gridName].select().first().is("td") ? scope[scope.gridName].select().first().parent("tr") : scope[scope.gridName].select().first();
                                var selectedItem = scope[scope.gridName].dataItem(selected);
                                if (scope.onPreDeleteCallback({ args: selectedItem })) {
                                    $http({
                                        url: res[0]["dataSource"]["transport"].destroy.url,
                                        method: 'DELETE',
                                        data: selectedItem,
                                        dataType: "json",
                                        headers: { "Content-Type": "application/json;charset=utf-8" }
                                    })
                                        .success(function (data) {
                                        if (data === "success") {
                                            scope[scope.gridName].dataSource.remove(selectedItem);
                                            scope.winDelete.close();
                                        }
                                        else {
                                            scope.winDelete.close();
                                            //TODO:(If general exception catching not work)Show amessage that deleting process has encountered an
                                            //internal server problem and was not successful.
                                        }
                                    })
                                        .error(function (res) {
                                        //TODO: ajax call has encountered a problem and could not send deletion request to the server.
                                        scope.winDelete.close();
                                    });
                                }
                                //});
                            }
                        }
                    }
                    scope.cancelFromDeleting = function () {
                        scope.winDelete.close();
                    }
                    remove = true;
                }

                function setNewlyInsertedModelIds(dat) {
                    if (dat.Data.length == 1) {
                        //simple one item insertion
                        model.Id = dat.Data[0].Id;
                    }
                    else if (dat.Data.length > 1) {
                        //bulky insertion
                        for (var i = 0; i < dat.Data.Length; i++) {
                            var mdlUid = modelsAddedSofar[i];
                            var newItem = scope[scope.gridName].dataItem(mdlUid);
                            newItem.Id = dat.Data[i].Id;
                        }
                    }
                }

                function doUpdate(opt) {
                    if (opt === 'add') {
                        $http.post(initCreate.url, model)
                            .success(function (dat) {
                            setNewlyInsertedModelIds(dat);
                            // TODO : message that is created successfully.
                            scope.ajaxSuccess = true;
                            console.log(model + " is created successfully.");
                            scope.winAe.close();
                        })
                            .error(function (res) {
                            //TODO:remove the newly inserted value from ds.
                            scope[scope.gridName].dataSource.remove(model);
                            scope.ajaxSuccess = false;
                            scope.winAe.close();
                        });
                    }
                    else if (opt === 'edit') {
                        $http.put(initUpdt.url, model)
                            .success(function (dat) {
                            // TODO : message that is created successfully.
                            console.log(model + " is updated successfully.");
                            scope.winAe.close();
                        })
                            .error(function (res) {
                            model = null;
                            //TODO:remove the newly inserted value from ds.
                            //scope[scope.gridName].dataSource.remove(model);

                            scope.winAe.close();
                        });
                    }
                }

                function setDeleteRecordWindow(content, deleteText, cancelText) {
                    scope.winDelete.content("<div class='k-edit-form-container' >" + content + "</div>");
                    var buttonActions = $('<div class="k-edit-buttons k-state-default" >' +
                        '<button class="k-button k-button-icontext k-primary k-grid-delete" ng-click="deleteItem()" >' + deleteText + '</button> ' +
                        '<button class="k-button k-button-icontext k-grid-cancel" ng-click="cancelFromDeleting()" >' + cancelText + '</button> ' +
                        '</div>');
                    scope.winDelete.element
                        .find("div.k-edit-form-container")
                        .append(buttonActions);
                    $compile(scope.winDelete.element.find("div.k-edit-buttons"))(scope);
                }
                
                function setAdTemplate(content, adOrEdit) {
                    model = null;
                    scope.ajaxSuccess = false;
                    var addEditObj = adOrEdit === 'add' ? addEditGlossary.add : addEditGlossary.edit
                    scope.winAe.title(addEditObj.title);
                    scope.opt = adOrEdit
                    scope.winAe.content("<div class='k-edit-form-container' >"
                       + content
                        + "</div>");
                    var buttonActions = $('<div class="k-edit-buttons k-state-default" >' +
                        '<button class="k-button k-button-icontext k-primary k-grid-update" ng-click="editItem(opt)" >' + addEditObj.update + '</button> ' +
                        '<button class="k-button k-button-icontext k-grid-cancel" ng-click="cancelItem()" >' + addEditObj.cancel + '</button> ' +
                        '</div>');

                    scope.winAe.element
                        .find("div.k-edit-form-container")
                        .append(buttonActions);


                    var container = $("div.k-edit-form-container");
                    kendo.init(container);
                    var lastRule = null;
                    container.kendoValidator({
                        rules: {
                            "require": function (input) {
                                if (inputHasRequiredValCriteria(input.attr("name"))) {
                                    debugger;
                                    var text = input.val();
                                    if (text != "" && text.trim() === "") {
                                        return false;
                                    }
                                    else if (text === "" && text.trim() === "") {
                                        return false;
                                    }
                                    else {
                                        return true;
                                    }
                                }
                                else {
                                    return true;
                                }
                            }
                        },
                        messages: {
                            "require": function (input) {
                                debugger;
                                if (lastRule[input.attr("name")]) {
                                    return lastRule[input.attr("name")]["required"]["message"];
                                }
                            }
                        }
                    });

                    function inputHasRequiredValCriteria(inpName) {
                        for (var i = 0; i < valRules.length; i++) {
                            if (valRules[i][inpName]) {
                                if (valRules[i][inpName]["required"]) {
                                    lastRule = valRules[i];
                                    return true;
                                }
                            }
                        }
                        return false;
                    }

                    $compile(scope.winAe.element
                        .find("div.k-edit-buttons"))(scope);
                    if (adOrEdit === 'add') {
                        var mdl = kendo.data.Model.define(scope[scope.gridName].dataSource.options.schema.model);
                        model = new mdl();
                        var newlyInserted = scope[scope.gridName].dataSource.insert(0, model)
                        modelsAddedSofar.push(newlyInserted.uid);
                        viewModel = kendo.observable({
                            data: newlyInserted
                        });
                        scope.selectedItem = newlyInserted;
                    }
                    else if (adOrEdit === 'edit') {
                        var selected = scope[scope.gridName].select();
                        if (selected) {
                            selected = scope[scope.gridName].select().first().is("td") ? scope[scope.gridName].select().first().parent("tr") : scope[scope.gridName].select().first();
                            var selectedItem = scope[scope.gridName].dataItem(selected);
                            model = selectedItem;
                            selectedInitialItem = jQuery.extend(true, {}, scope[scope.gridName].dataItem(selected));
                            viewModel = kendo.observable({
                                data: selectedItem
                            });
                            scope.selectedItem = selectedItem;
                            
                        }
                    }
                    kendo.bind($("div.k-edit-form-container"), viewModel);
                }

                makeValidationRules(res[0].columns);
                //var tempAdd = "Core\/Template\/Get?templateUrl=" + tempAddress + "&viewModelFullName=" + viewModelTypeFullName.replace(/\./gm, "_"); + "";
                //var tempAdd = "Core\/Template\/Get?viewModelFullName=" + viewModelTypeFullName.replace(/\./gm, "_");// + "";
                scope.gridOptions = res[0];
                var kendoGridElem = "<div kendo-grid='" + scope.gridName + "' options='gridOptions'  k-toolbar='[{ template: toolbarTemplate }]' ></div>";
                dom.html(kendoGridElem);

                if (addEdit) {
                    dom.append(winAE);
                }
                if (remove) {
                    dom.append(winDelete);
                }
                
                $compile(dom)(scope);

                scope.$on("kendoWidgetCreated", function (options, widget) {
                    if (widget.options.name === "Grid") {
                        if (scope.onInit) {
                            scope.onInit({ args: widget });
                        }
                    }
                });
            });
        },
        template: "<div></div>"
    };
}]);