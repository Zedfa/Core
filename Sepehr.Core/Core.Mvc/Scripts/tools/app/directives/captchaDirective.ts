

var captchaModule = new SepehrModule.MainModule("coreCaptchaDirective", []);

captchaModule.addDirective('coreCaptcha', [() => {

    return {
        restrict: 'E',

        replace: true,
        scope: {
            captchaEntered: '=',
            captchaEncrypted: '=captchaEncrypted',
            refreshImage: '=',

        },
        link: ($scope, $element, attrs, ctrl) => { },

        controller: ['$scope', '$http', ($scope, $http) => {
            $scope.selectText = () => {
                $('#securityCode').select();
            };
            $scope.locale = Ambients.Window.getLocale();
            $scope.refreshImage = () => {
                $scope.base64Data = "/Areas/Core/Content/images/captcha-loading.gif"; 

                $http.get("api/CaptchaApi/GetCaptchaImage").success((data) => {
                    
                    $scope.captchaEncrypted = data.EncryptedKey;
                    $scope.base64Data = "data:image/jpg;base64," + data.Base64imgage;
                    $scope.captchaEntered = '';
                }).error((xhr, ajaxOptions, thrownError) => {
                    console.log(xhr);
                    console.log(thrownError);
                    console.log(ajaxOptions);
                });
            }
            $scope.refreshImage();
            $scope.keypress = (ev) => {
                if (ev.which === 13) {
                    $scope.userLogin();
                }
            }
        }],

        templateUrl: "/core/partialviews/index?partialViewFileName=Captcha"

    }
}]);

















//var coreCaptchaDirective = new SepehrModule.MainModule("coreCaptchaDirective", []);
//coreCaptchaDirective.addDirective("coreCaptcha", [function () {

//    return {
//        restrict: 'E',
//        replace: true,
//        controller: ["$scope", "$element", "$attrs", ($scope, $element, $attrs) => {
           
            
            

//               // var img: JQuery = $("<img />");
//                //var captchaContainer: JQuery = $("#captchaContainer");
//                //var imgCaptchaLoading: JQuery = $("#imgCaptchaLoading");
//                //captchaContainer.hide()
//                //imgCaptchaLoading.show()
//                //img.attr('src', captchaUrl)
//                //    .load((data) => {
//                //    imgCaptchaLoading.hide();
//                //    captchaContainer.html(<any>img);
//                //    captchaContainer.show();
//                //});
           
//        }],
//        link: ($scope, $element, attrs, ctrl) => {
//            
//            $scope.imgCaptchaLoading = true;

//            $scope.captchaUrl = window.location.origin + '/Core/Captcha/GetCaptchaImage?guid=' + $("#hdnCaptchaGuid").val() + '&r=' + Math.random();
            
//            $scope.loadCaptcha = () => {

//                $scope.captchaUrl = window.location.origin + '/Core/Captcha/GetCaptchaImage?guid=' + $("#hdnCaptchaGuid").val() + '&r=' + Math.random();

//                $scope.imgCaptchaLoading = true;

//            };

//            $element.find("#captchaImag").load((data) => {
//                
//                $scope.imgCaptchaLoading = false;

//            });
            

//        },
//        templateUrl: getAreaUrl("Captcha", "Index")
//    }
//}]);


