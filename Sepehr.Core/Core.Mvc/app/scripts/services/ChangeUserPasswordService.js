var changeUserPasswordServiceModule = new SepehrModule.MainModule('changeUserPasswordServiceModule', []);
changeUserPasswordServiceModule.addService('changeUserPasswordService', ['$http', '$q', function ($http, $q) {
        return {
            updateUserPassword: function (filterObject) {
                $http.put("/api/ChangeUserPasswordApi/PutEntity", JSON.stringify(filterObject)).error(function (err) {
                });
            }
        };
    }]);
