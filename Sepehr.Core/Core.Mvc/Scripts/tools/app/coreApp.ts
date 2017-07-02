/// <reference path="../sepehrappmodule.ts" />
/// <reference path="../../custom-types/coreviewelementinfo.ts" />

class coreModules {
    static coreModules() {
        moduleManagement.modulesFunc.push(coreModules.modules);
    }

    static modules(): Array<string> {
        return [
            'ngRoute',
            'kendo.directives',
            'grdSearchServiceModule',
            'kendoGridViewModule',
            'menuDirectiveModule',
            'viewElementServiceModule',
            //'menuContainerModule',
            'coreMainServiceModule',
            'dropDownListDirectiveModule',
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
            //'coreShortcutkeysService'
        ];
    }
}
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


coreAppModule.addController('clearTabContainer', ['$scope', 'viewElementService', ($scope, viewElementService) => {

    if (viewElementService.topMenu.tabStrip) {
        viewElementService.topMenu.closeAllOpenTabStrips();
    }

}]);


coreAppModule.addController('renderMenuController', ['$scope', 'getAuthorize', 'isAuthorizeService', ($scope, getAuthorize, isAuthorizeService) => {
    $scope.isAuthorize = isAuthorizeService.isAuthorize;
    $scope.isAuthorizeValue = isAuthorizeService;
    getAuthorize($scope).then((authorize) => {
        $scope.isAuthorizeValue = isAuthorizeService;
    });
}]);

