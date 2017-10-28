var gridSearchServiceModule = new SepehrModule.MainModule("gridSearchServiceModule", ["coreAppInfoServiceModule"]);
gridSearchServiceModule.addService("gridSearchService", ["$compile", "coreAppInfoService", function ($compile, coreAppInfoService) {
        window.gridSearchInstances = window.gridSearchInstances ? window.gridSearchInstances : new Array();
        function findObject(id) {
            var foundInstance = null;
            $.each(window.gridSearchInstances, function (index, info) {
                if (info.id == id && coreAppInfoService.getElementInCurrentTabById("[kendo-grid=" + id + "]")) {
                    coreAppInfoService.getElementInCurrentTabById("[kendo-grid=" + id + "]:not(:first)").remove();
                    foundInstance = info.instanc;
                    return false;
                }
            });
            return foundInstance;
        }
        ;
        function setActiveGridSearch(gridInfo) {
            var instancefound = false;
            $.each(window.gridSearchInstances, function (index, info) {
                if (info.id == gridInfo.id) {
                    info.active = true,
                        info.instanc = gridInfo.instanc,
                        instancefound = true;
                }
                else {
                    info.active = false;
                }
            });
            if (!instancefound) {
                window.gridSearchInstances.push(gridInfo);
            }
        }
        function getActiveGridSearch() {
            var result = null;
            $.each(window.gridSearchInstances, function (index, info) {
                if (info.active) {
                    result = info.instanc;
                    return false;
                }
            });
            return result;
        }
        function setGlobalOnInitMethod(method) {
        }
        ;
        return {
            loadGridSearch: function (domElement, grdScope, gridId, ngOkCallback, ngCancelOrCloseCallback) {
                var gridSearchInstance = null, searchGridId = "grdSearch" + gridId;
                gridSearchInstance = ngSearchHelper.initializeNewGridSearch(domElement, grdScope, $compile, gridId, ngOkCallback, ngCancelOrCloseCallback, null, null, null);
                setActiveGridSearch({ id: searchGridId, instanc: gridSearchInstance, active: true });
                gridSearchInstance.initializeSearchObj();
                gridSearchInstance.openSearchWindow();
            },
            getGridSearchInstanceById: function (gridId) {
                var existedInstance = findObject(gridId);
                return existedInstance;
            },
            getActiveGridSearchInstance: function () {
                return getActiveGridSearch();
            },
            setActiveGridSearchInstance: function (instance) {
                setActiveGridSearch({ id: instance.cGId, instanc: instance, active: true });
            }
        };
    }]);
