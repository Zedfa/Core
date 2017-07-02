var coreAuthorizeService = new SepehrModule.MainModule('coreAuthorizeService', ['coreLoginService', 'viewElementServiceModule']);
coreAuthorizeService.addService('isAuthorizeService', [function () {
        return {
            isAuthorize: false
        };
    }]);
coreAuthorizeService.addService('getAuthorize', ['$http', '$q', 'isAuthorizeService', 'renderMenuService', 'viewElementService', 'AccessibleViewElementsService', function ($http, $q, isAuthorizeService, renderMenuService, viewElementService, AccessibleViewElementsService) {
        return function (scope) {
            var deffered = $q.defer();
            $http.get("api/AccountApi/GetUserHassAccess").success(function (result) {
                isAuthorizeService.isAuthorize = result;
                if (result && AccessibleViewElementsService.AccessibleViewElements == null) {
                    viewElementService.GetAccessibleViewElements().then(function (data) {
                        AccessibleViewElementsService.AccessibleViewElements = data;
                        renderMenuService.update(scope);
                        deffered.resolve(result);
                    });
                }
                else {
                    renderMenuService.update(scope);
                    deffered.resolve(result);
                }
            }).error(function (err) {
                deffered.reject(err);
            });
            return deffered.promise;
        };
    }]);
coreAuthorizeService.addService('getMenuPageService', ['$http', '$q', function ($http, $q) {
        return function () {
            var deffered = $q.defer();
            $http.get(getAreaUrl("Home", "MainMenu")).success(function (pageContent) {
                deffered.resolve(pageContent);
            }).error(function (err) {
                deffered.reject(null);
            });
            return deffered.promise;
        };
    }]);
coreAuthorizeService.addService('renderMenuService', ['$q', '$compile', 'getMenuPageService', 'viewElementService', function ($q, $compile, getMenuPageService, viewElementService) {
        var update = function update(scope) {
            var deffered = $q.defer();
            getMenuPageService().then(function (content) {
                $("#topMainMenu").html(content);
                var result = $compile(angular.element($("#topMainMenu")))(scope);
                deffered.resolve(result);
            }).catch(function (ex) {
                deffered.reject(ex);
            });
            return deffered.promise;
        };
        return { update: update };
    }]);
