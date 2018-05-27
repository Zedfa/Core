/// <reference path="../../../scripts/tools/sepehrappmodule.ts" />
/// <reference path="../../../scripts/custom-types/changeuserpassword.ts" />
/// <reference path="../../../scripts/custom-types/validators.ts" />
var changeUserPasswordControllerModule = new SepehrModule.MainModule('changeUserPasswordControllerModule', ['changeUserPasswordServiceModule', 'coreValidationService']);
changeUserPasswordControllerModule.addController("changeUserPasswordController", ['$scope', '$element', 'changeUserPasswordService', 'validate',
    ($scope, $element, changeUserPasswordService, validate) => {

        $scope.changeUserPassword = new ChangeUserPassword();

        var customValidations: Array<ValidationAttribute> = new Array<ValidationAttribute>();

        customValidations.push({ type: "RequiredValidator", message: "", params: [] });

        $scope.confirmValidations = customValidations;

        $scope.submitNewPassword = () => {
            if (validate($element))
                changeUserPasswordService.updateUserPassword($scope.changeUserPassword);

        }


    }]);

