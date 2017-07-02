

var sepehrViewDirectiveModule = new SepehrModule.MainModule('sepehrViewDirectiveModule', []);
sepehrViewDirectiveModule.addDirective('sepehrView', [() => {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            currentTab: "="
        },
        controller: ['$scope', '$http', '$element', '$attrs', '$compile', '$location', ($scope, $http, $element, $attrs, $compile, $location) => {

            if ($location.path().split('/')[2]) {
                var myDiv = document.createElement('div');
                myDiv.setAttribute("ng-include", "\'/core/partialviews/index?partialViewFileName=" + $location.path().split('/')[2] + "\'");
                myDiv.setAttribute("ng-show", "'" + $location.path().split('/')[2].hashCode() + "'== currentTab");
                myDiv.setAttribute("id", $location.path().split('/')[2].hashCode());

                $("#sepehrViewContainer").append($compile(myDiv)($scope));
            }

        }],

        link: ($scope, $element, attrs, ctrl) => {

        },

        templateUrl: '/core/partialviews/index?partialViewFileName=SepehrView'
    }
}]);


