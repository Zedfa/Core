
var dateInfoServiceModule = new SepehrModule.MainModule("dateInfoServiceModule", []);

dateInfoServiceModule.addService("dateInfoService", ["$http", "$q", function ($http, $q) {
    return {
        GetTodaysShamsiDate: () => {

            var defer = $q.defer();
            $http.get("/fa/api/DateInfoApi/GetTodaysShamsiDate")
                .success((date) => {

                    defer.resolve(date);
                })
                .error(function () {
                    defer.reject("Failed to get menuItems");
                });
            return defer.promise;

        },
        GetTodaysMiladiDate: () => {

            var defer = $q.defer();
            $http.get("/api/DateInfoApi/GetTodaysMiladiDate")
                .success((date) => {

                    defer.resolve(date);
                })
                .error(function () {
                    defer.reject("Failed to get menuItems");
                });
            return defer.promise;

        }
    }
}]);