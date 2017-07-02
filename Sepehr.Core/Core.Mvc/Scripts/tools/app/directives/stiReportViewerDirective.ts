/// <reference path="../../sepehrappmodule.ts" />


var changePassDirective = new SepehrModule.MainModule('stimReportViewerDirective', []);
changePassDirective.addDirective('stimReportViewers', [() => {
    return {
        restrict: 'E',
        replace: true,
        scope:
        {
            reportType: "="
        },
        controller: ['$scope', '$http', '$element', '$attrs', '$compile', ($scope, $http, $element, $attrs, $compile) => {
            //$scope.reportType = $scope.report;
        }],
        link: ($scope, $element, attrs, ctrl) => {

        },

        templateUrl: ($scope, $element, attrs) => { return 'StimReportViewer?reportType=' + $element.reportType },
    }
}]);


 