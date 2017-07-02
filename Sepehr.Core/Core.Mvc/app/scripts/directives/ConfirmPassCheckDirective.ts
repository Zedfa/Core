
var confirmPassCheckDirective = new SepehrModule.MainModule("confirmPassCheckDirectiveModule", ["coreValidationService"]);

confirmPassCheckDirective.addDirective("confirmPassCheck", [() => {

    return {
        restrict: 'E',
        replace: true,
        transclude: true,
        scope: {
            id: "@",
            password: "=",
            class: "@",
            validators: "=",
            confirm: "="
        },

        link: ($scope, $element, attrs, ctrl) => {

            $element.removeClass($scope.class);
            $scope.validator.options.rules.compareValue = (element) => {

                return $scope.confirm == $scope.password;
            };

            $scope.validator.options.messages.compareValue = "کلمه عبور مطابقت ندارد!";

        },
        template: '<span ><input kendo-validator="validator"  type="password" id="{{id}}" ng-change="compareValue()" ng-model="confirm"  validations="validators" ng-class="class" /> </span>'
    }


}]);