var changeUserPasswordServiceModule = new SepehrModule.MainModule('changeUserPasswordServiceModule', []);
changeUserPasswordServiceModule.addService('changeUserPasswordService', ['$http', '$q', ($http, $q) => {
    return {
        updateUserPassword: (filterObject: ChangeUserPassword): any => {
            $http.put("/api/ChangeUserPasswordApi/PutEntity", JSON.stringify(filterObject)).error((err) => {
               
            });
        }
    }
}]);
