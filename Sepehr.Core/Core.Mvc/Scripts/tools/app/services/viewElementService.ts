/// <reference path="../../topmainmenu.ts" />
/// <reference path="../../sepehrappmodule.ts" />

var viewElementServiceModule = new SepehrModule.MainModule("viewElementServiceModule", []);

viewElementServiceModule.addService("AccessibleViewElementsService", [function () {


    return { AccessibleViewElements: null }

}]);

viewElementServiceModule.addService("viewElementService", ["$http", "$q", function ($http, $q) {


    return {
        topMenu: ns_MainMenu,
        GetMenuItems: () => {
            var defer = $q.defer();
            $http.get("/api/ViewElementApi/GetMenuItems")
                .success((data) => {
                    defer.resolve(data);
                })
                .error(function () {
                    defer.reject("Failed to get menuItems");
                });
            return defer.promise;
        },
        eventArgs: { menuItemSelectedEventArgs: null },
        GetAccessibleViewElements: () => {

            var defer = $q.defer();
            $http.get("/api/ViewElementApi/GetAccessibleViewElements")
                .success((data) => {

                    defer.resolve(data);
                })
                .error(function () {
                    defer.reject("Failed to get menuItems");
                });
            return defer.promise;
        },
        AccessibleViewElements: { items: null },

    }
}]);
viewElementServiceModule.addService("getViewElementInfoByUniqueNamesFirstPart", ['viewElementService', 'AccessibleViewElementsService', function (viewElementService, AccessibleViewElementsService) {

    var getViewElementByuniqueNamesFirstPart = (uniqueNamesFirstPart) => {
        var viewElementInfo = new CoreViewElementInfo();
        if (AccessibleViewElementsService.AccessibleViewElements) {

            var splitedUrl = uniqueNamesFirstPart.split('/');
            for (var i = 0; i < AccessibleViewElementsService.AccessibleViewElements.length; i++) {
                if (AccessibleViewElementsService.AccessibleViewElements[i].UniqueName.split('#')[0] == splitedUrl[2]) {
                    viewElementInfo.Title = AccessibleViewElementsService.AccessibleViewElements[i].Title;
                    viewElementInfo.UniqueName = AccessibleViewElementsService.AccessibleViewElements[i].UniqueName;
                    break;
                }


            }
        }

        if (!viewElementInfo.Title) {
            var errorViewElement = $.grep(AccessibleViewElementsService.AccessibleViewElements, function (item, index) {
                return (item as CoreViewElementInfo).UniqueName.split('#')[0] == "Error";
            });
            viewElementInfo.Title = (errorViewElement[0] as CoreViewElementInfo).Title;
            viewElementInfo.UniqueName = (errorViewElement[0] as CoreViewElementInfo).UniqueName;
        }
        if (viewElementInfo) {
            var queryString = "";
            if (viewElementService.topMenu.tabStrip) {
                if (splitedUrl.length > 3) {
                    for (var j = 3; j < splitedUrl.length; j++) {

                        queryString += "/" + splitedUrl[j];
                    }
                }
                viewElementService.topMenu.menuItemSelected(null, viewElementInfo.UniqueName, viewElementInfo.Title, queryString);

            }
        }
    }
    return getViewElementByuniqueNamesFirstPart;
}]);