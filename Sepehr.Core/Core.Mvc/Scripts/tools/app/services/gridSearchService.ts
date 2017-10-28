/// <reference path="../../nggridsearch.ts" />
var gridSearchServiceModule = new SepehrModule.MainModule("gridSearchServiceModule", ["coreAppInfoServiceModule"]);



gridSearchServiceModule.addService("gridSearchService", ["$compile", "coreAppInfoService", function ($compile, coreAppInfoService) {

    window.gridSearchInstances = window.gridSearchInstances ? window.gridSearchInstances : new Array<gridInstanceInfo>();

    function findObject(id: string): ngSearchObj {

        var foundInstance: ngSearchObj = null;
        $.each(window.gridSearchInstances, (index: number, info: gridInstanceInfo) => {

            if (info.id == id && coreAppInfoService.getElementInCurrentTabById("[kendo-grid=" + id + "]")) {

                coreAppInfoService.getElementInCurrentTabById("[kendo-grid=" + id + "]:not(:first)").remove();

                foundInstance = info.instanc;

                return false;
            }
        });
        return foundInstance;
    };
    function setActiveGridSearch(gridInfo: gridInstanceInfo) {
        var instancefound = false;

        $.each(window.gridSearchInstances, (index: number, info: gridInstanceInfo) => {

            // info.active = info.id == gridInfo.id;
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
        $.each(window.gridSearchInstances, (index: number, info: gridInstanceInfo) => {

            if (info.active) {
                result = info.instanc;
                return false;
            }
        });
        return result;
    }
    function setGlobalOnInitMethod(method: Function) {

    };
    return {

        loadGridSearch: (domElement, grdScope, gridId, ngOkCallback, ngCancelOrCloseCallback): void => {



            var gridSearchInstance: ngSearchObj = null,
                searchGridId = "grdSearch" + gridId;
           
            gridSearchInstance = ngSearchHelper.initializeNewGridSearch(domElement, grdScope, $compile, gridId, ngOkCallback, ngCancelOrCloseCallback, null, null, null);
          
            setActiveGridSearch({ id: searchGridId, instanc: gridSearchInstance, active: true });

            gridSearchInstance.initializeSearchObj();
            gridSearchInstance.openSearchWindow();



        },

        getGridSearchInstanceById: (gridId): ngSearchObj => {
            var existedInstance = findObject(gridId);
            return existedInstance;
        },
        getActiveGridSearchInstance: (): ngSearchObj => {
            return getActiveGridSearch();
        },
        setActiveGridSearchInstance: (instance: ngSearchObj): void => {
            setActiveGridSearch({ id: instance.cGId, instanc: instance, active: true });
        }
       
    }
}]);
