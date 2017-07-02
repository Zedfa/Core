
var coreLogOnModule = new SepehrModule.MainModule("coreLoginModule", ["coreLoginService", "coreAuthorizeService"]);

coreLogOnModule.addDirective("coreLogin", ["login", function (login) {

    return {
        restrict: 'E',
        replace: true,
        transclude: true,
        //scope: {
        //    isAuthorize:"="
        //},
        controller: ["$scope", "$element", "$attrs", "$q", "getAuthorize", "isAuthorizeService", ($scope, $element, $attrs, $q, getAuthorize, isAuthorizeService) => {
            
            // $scope.isAuthorize = isAuthorizeService.isAuthorize;

            $scope.logOnClick = function logOnClick(): void {


                var loginData = $('#loginForm').kendoValidator().data("kendoValidator"), status = $(".status");
                if (loginData.validate()) {
                    var info = {
                        UserName: $scope.userName,
                        Password: $scope.password,
                        RememberMe: $('#RememberMe').is(":checked"),
                        HiddenId: $scope.captchaEncrypted,
                        CaptchaCode: $scope.captchaEntered

                    };
                    login(info).then((data) => {



                        getAuthorize($scope).then((result) => {

                            if (result) {
                            
                                //isAuthorize = result;
                                $scope.isAuthorize = isAuthorizeService.isAuthorize;
                                $scope.loginPopup.close();
                            }
                        })
                    }).catch((err) => {

                    });

                }
            };


        }],
        link: ($scope, $element, attrs, ctrl) => {


            $scope.loginPopup.center().open();

        },
        templateUrl: getAreaUrl("Account", "LogOn")
    }
}]);



