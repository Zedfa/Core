var coreLogOnModule = new SepehrModule.MainModule("coreLoginModule", ["coreLoginService", "coreAuthorizeService"]);
coreLogOnModule.addDirective("coreLogin", ["login", function (login) {
        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            controller: ["$scope", "$element", "$attrs", "$q", "getAuthorize", "isAuthorizeService", function ($scope, $element, $attrs, $q, getAuthorize, isAuthorizeService) {
                    $scope.logOnClick = function logOnClick() {
                        var loginData = $('#loginForm').kendoValidator().data("kendoValidator"), status = $(".status");
                        if (loginData.validate()) {
                            var info = {
                                UserName: $scope.userName,
                                Password: $scope.password,
                                RememberMe: $('#RememberMe').is(":checked"),
                                HiddenId: $scope.captchaEncrypted,
                                CaptchaCode: $scope.captchaEntered
                            };
                            login(info).then(function (data) {
                                getAuthorize($scope).then(function (result) {
                                    if (result) {
                                        $scope.isAuthorize = isAuthorizeService.isAuthorize;
                                        $scope.loginPopup.close();
                                    }
                                });
                            }).catch(function (err) {
                            });
                        }
                    };
                }],
            link: function ($scope, $element, attrs, ctrl) {
                $scope.loginPopup.center().open();
            },
            templateUrl: getAreaUrl("Account", "LogOn")
        };
    }]);
