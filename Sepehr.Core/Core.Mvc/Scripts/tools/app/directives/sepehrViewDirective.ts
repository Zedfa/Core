

var sepehrViewDirectiveModule = new SepehrModule.MainModule('sepehrViewDirectiveModule', []);
sepehrViewDirectiveModule.addDirective('sepehrView', [() => {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            currentTab: "="
        },
        controller: ['$scope', '$http', '$element', '$attrs', '$compile', '$location', '$window', ($scope, $http, $element, $attrs, $compile, $location, $window) => {

            if ($location.path().split('/')[2]) {
                var myDiv = document.createElement('div');
                myDiv.setAttribute("ng-include", "\'/core/partialviews/index?partialViewFileName=" + $location.path().split('/')[2] + "\'");
                myDiv.setAttribute("ng-show", "'" + $location.path().split('/')[2].hashCode() + "'== currentTab");
                myDiv.setAttribute("id", $location.path().split('/')[2].hashCode());

                $("#sepehrViewContainer").append($compile(myDiv)($scope));
            }
             var windowapp = angular.element($window);
             windowapp.bind('resize', function () {
                $scope.getDocHeight = function () {
                    return Math.max(
                        $(document).height(),
                        $(window).height(),
                        /* For opera: */
                        document.documentElement.clientHeight,
                    );
                };
                var bodyHeight = document.documentElement.clientHeight;
                var footerHeight = 39;
                var gridContentHeight = bodyHeight - (footerHeight + 110 + 37 + 31 + 15);
                $('body').find("div.k-grid-content").css("height", gridContentHeight);
            });
        }],

        link: ($scope, $element, attrs, ctrl, $window) => {
           




        },

        templateUrl: '/core/partialviews/index?partialViewFileName=SepehrView'
    }
}]);


