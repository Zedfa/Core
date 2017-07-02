var coreAuthorizeService = new SepehrModule.MainModule('coreLoginService', []);
coreAuthorizeService.addService('login', ['$http', '$q', function ($http, $q) {
        var loginFunc = function (info) {
            var deffered = $q.defer();
            $http.post("api/AccountApi/PostEntity", JSON.stringify(info)).success(function (result) {
                deffered.resolve(result);
            }).error(function (err) {
                deffered.reject(err);
            });
            return deffered.promise;
        };
        return loginFunc;
    }]);
