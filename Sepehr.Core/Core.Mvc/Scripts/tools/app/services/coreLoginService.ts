/// <reference path="../../sepehrappmodule.ts" />
/// <reference path="../../generaltools.ts" />
var coreAuthorizeService = new SepehrModule.MainModule('coreLoginService', []);

coreAuthorizeService.addService('login', ['$http', '$q', ($http, $q) => {
    var loginFunc = (info) => {


        var deffered = $q.defer();
        $http.post("api/AccountApi/PostEntity",JSON.stringify( info)).success((result) => {
            deffered.resolve(result);
        }).error((err) => {
            deffered.reject(err);
        });
        return deffered.promise;
    };
    return loginFunc;
}]);

//coreAuthorizeService.addService('getMenuPageService', ['$http', '$q', ($http, $q) => {

//    var deffered = $q.defer();
//    $http.get(getAreaUrl("Home", "MainMenu")).success((pageContent) => {
//        deffered.resolve(pageContent);
//    }).error((err) => {
//        deffered.reject(null);
//    });
//    return deffered.promise;

//}]);
//coreAuthorizeService.addService('renderMenuService', ['$q', '$compile', 'getMenuPageService', ($q, $compile, getMenuPageService) => {

//    var update = function update(scope) {
//        var deffered = $q.defer();

//        getMenuPageService.then((content) => {
//            

//            $("#topMainMenu").html(content);
//            var result = $compile(angular.element($("#topMainMenu")))(scope);
//            deffered.resolve(result);

//        }).catch((ex) => {
//            
//            deffered.reject(ex);
//        });

//        return deffered.promise;
//    };
//    return { update: update };
//}]);


