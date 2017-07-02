var captchaModule = new SepehrModule.MainModule("coreCaptchaDirective", []);
captchaModule.addDirective('coreCaptcha', [function () {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                captchaEntered: '=',
                captchaEncrypted: '=captchaEncrypted',
                refreshImage: '=',
            },
            link: function ($scope, $element, attrs, ctrl) { },
            controller: ['$scope', '$http', function ($scope, $http) {
                    $scope.selectText = function () {
                        $('#securityCode').select();
                    };
                    $scope.locale = Ambients.Window.getLocale();
                    $scope.refreshImage = function () {
                        $scope.base64Data = "/Areas/Core/Content/images/captcha-loading.gif";
                        $http.get("api/CaptchaApi/GetCaptchaImage").success(function (data) {
                            $scope.captchaEncrypted = data.EncryptedKey;
                            $scope.base64Data = "data:image/jpg;base64," + data.Base64imgage;
                            $scope.captchaEntered = '';
                        }).error(function (xhr, ajaxOptions, thrownError) {
                            console.log(xhr);
                            console.log(thrownError);
                            console.log(ajaxOptions);
                        });
                    };
                    $scope.refreshImage();
                    $scope.keypress = function (ev) {
                        if (ev.which === 13) {
                            $scope.userLogin();
                        }
                    };
                }],
            templateUrl: "/core/partialviews/index?partialViewFileName=Captcha"
        };
    }]);
