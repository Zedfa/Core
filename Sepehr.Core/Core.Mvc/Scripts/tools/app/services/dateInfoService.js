var dateInfoServiceModule = new SepehrModule.MainModule("dateInfoServiceModule", []);
dateInfoServiceModule.addService("dateInfoService", ["$http", "$q", function ($http, $q) {
        return {
            GetTodaysShamsiDate: function () {
                var defer = $q.defer();
                $http.get("/api/DateInfoApi/GetTodaysShamsiDate")
                    .success(function (date) {
                    defer.resolve(date);
                })
                    .error(function () {
                    defer.reject("Failed to get menuItems");
                });
                return defer.promise;
            },
            GetTodaysMiladiDate: function () {
                var defer = $q.defer();
                $http.get("/api/DateInfoApi/GetTodaysMiladiDate")
                    .success(function (date) {
                    defer.resolve(date);
                })
                    .error(function () {
                    defer.reject("Failed to get menuItems");
                });
                return defer.promise;
            }
        };
    }]);
