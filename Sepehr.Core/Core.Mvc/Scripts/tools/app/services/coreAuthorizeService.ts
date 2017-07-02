/// <reference path="../../sepehrappmodule.ts" />
/// <reference path="../../generaltools.ts" />
var coreAuthorizeService = new SepehrModule.MainModule('coreAuthorizeService', ['coreLoginService', 'viewElementServiceModule']);

coreAuthorizeService.addService('isAuthorizeService', [() => {

    return {
        isAuthorize: false
    };

}]);

coreAuthorizeService.addService('getAuthorize', ['$http', '$q', 'isAuthorizeService', 'renderMenuService', 'viewElementService', 'AccessibleViewElementsService', ($http, $q, isAuthorizeService, renderMenuService, viewElementService, AccessibleViewElementsService) => {
    return (scope) => {

        var deffered = $q.defer();
        $http.get("api/AccountApi/GetUserHassAccess").success((result) => {

            isAuthorizeService.isAuthorize = result;
            if (result && AccessibleViewElementsService.AccessibleViewElements == null) {
                viewElementService.GetAccessibleViewElements().then((data) => {
                    AccessibleViewElementsService.AccessibleViewElements = data;
                    renderMenuService.update(scope);
                    deffered.resolve(result);
                });
            }
            else {

                renderMenuService.update(scope);
                deffered.resolve(result);
            }

        }).error((err) => {

            deffered.reject(err);
        });
        return deffered.promise;
    }
}]);



coreAuthorizeService.addService('getMenuPageService', ['$http', '$q', ($http, $q) => {
    return () => {
        var deffered = $q.defer();
        $http.get(getAreaUrl("Home", "MainMenu")).success((pageContent) => {

            deffered.resolve(pageContent);
        }).error((err) => {
            deffered.reject(null);
        });
        return deffered.promise;
    };
}]);
coreAuthorizeService.addService('renderMenuService', ['$q', '$compile', 'getMenuPageService', 'viewElementService', ($q, $compile, getMenuPageService, viewElementService) => {

    var update = function update(scope) {

        var deffered = $q.defer();

        getMenuPageService().then((content) => {
            $("#topMainMenu").html(content);
            var result = $compile(angular.element($("#topMainMenu")))(scope);
            deffered.resolve(result);

        }).catch((ex) => {

            deffered.reject(ex);
        });

        return deffered.promise;
    };
    return { update: update };
}]);

