var changeUserPasswordControllerModule = new SepehrModule.MainModule('changeUserPasswordControllerModule', ['changeUserPasswordServiceModule', 'coreValidationService']);
changeUserPasswordControllerModule.addController("changeUserPasswordController", ['$scope', '$element', 'changeUserPasswordService', 'validate',
    function ($scope, $element, changeUserPasswordService, validate) {
        $scope.changeUserPassword = new ChangeUserPassword();
        var customValidations = new Array();
        customValidations.push({ type: "RequiredValidator", message: "", params: [] });
        $scope.confirmValidations = customValidations;
        $scope.submitNewPassword = function () {
            if (validate($element))
                changeUserPasswordService.updateUserPassword($scope.changeUserPassword);
        };
    }]);
