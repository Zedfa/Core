var DCAddress = "sepehrsystems.com";
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

            $scope.setDomain = (e) => {
                if ($scope.userName) {

                    if ($scope.userName.indexOf("@") == 0) {
                        $scope.domain = DCAddress;
                        return;
                    }
                }
                $scope.domain = "";
            }
            $scope.logOnClick = function logOnClick(): void {


                var loginData = $('#loginForm').kendoValidator().data("kendoValidator"), status = $(".status");
                if (loginData.validate()) {
                    var info = {
                        UserName: $scope.domain ? $scope.userName.replace("@", "") + "@" + DCAddress : $scope.userName,
                        Password: $scope.password,
                        RememberMe: $('#RememberMe').is(":checked"),
                        HiddenId: $scope.captchaEncrypted,
                        CaptchaCode: $scope.captchaEntered,
                        Domain: $scope.domain
                    };
                    login(info).then((data) => {
                        getAuthorize($scope).then((result) => {
                            if (result) {
                                      
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



