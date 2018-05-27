var coreModules = (function () {
    function coreModules() {
    }
    coreModules.coreModules = function () {
        moduleManagement.modulesFunc.push(coreModules.modules);
    };
    coreModules.modules = function () {
        return [
            'ngRoute',
            'kendo.directives',
            'gridSearchServiceModule',
            'gridViewModule',
            'menuDirectiveModule',
            'viewElementServiceModule',
            'coreMainServiceModule',
            'dropDownListDirectiveModule',
            'autocompleteDirectiveModule',
            'coreAuthorizeService',
            'coreLoginModule',
            'coreCaptchaDirective',
            'coreLoginService',
            'priceFormatDirectiveModule',
            'coreNotificationService',
            'httpInterceptorModule',
            'notificationDirective',
            'coreValidationService',
            'validationDirective',
            'dateInfoServiceModule',
            'sepehrViewDirectiveModule',
            'managePagesSeviceModule',
            'userRoleControllerModule',
            'changeUserPasswordControllerModule',
            'confirmPassCheckDirectiveModule',
            'coreAppInfoServiceModule',
            'lookupDirectiveModule'
        ];
    };
    return coreModules;
}());
coreModules.coreModules();
var coreAppModule = new SepehrModule.MainModule("coreApp", moduleManagement.loadModules());
coreAppModule.addConfig(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
        $routeProvider.when('/menu/:name', {
            templateUrl: function (urlattr) {
                return "/core/partialviews/index?partialViewFileName=" + urlattr.name;
            },
            controller: 'coreMakeProperTabController'
        })
            .when('/menu/', {
            template: '',
            controller: 'clearTabContainer'
        })
            .otherwise({
            redirectTo: '/menu'
        });
    }]);
coreAppModule.addController('coreMakeProperTabController', ['$scope', '$location', 'viewElementService', 'getViewElementInfoByUniqueNamesFirstPart', '$q', function ($scope, $location, viewElementService, getViewElementInfoByUniqueNamesFirstPart, $q) {
        var viewElementInfo = new CoreViewElementInfo();
        viewElementInfo = getViewElementInfoByUniqueNamesFirstPart($location.$$path);
    }]);
coreAppModule.addController('clearTabContainer', ['$scope', 'viewElementService', function ($scope, viewElementService) {
        if (viewElementService.topMenu.tabStrip) {
            viewElementService.topMenu.closeAllOpenTabStrips();
        }
    }]);
coreAppModule.addController('renderMenuController', ['$scope', 'getAuthorize', 'isAuthorizeService', function ($scope, getAuthorize, isAuthorizeService) {
        $scope.isAuthorize = isAuthorizeService.isAuthorize;
        $scope.isAuthorizeValue = isAuthorizeService;
        getAuthorize($scope).then(function (authorize) {
            $scope.isAuthorizeValue = isAuthorizeService;
        });
    }]);
