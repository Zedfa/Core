var changePassDirective = new SepehrModule.MainModule('stimReportViewerDirective', []);
changePassDirective.addDirective('stimReportViewers', [function () {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                reportType: "="
            },
            controller: ['$scope', '$http', '$element', '$attrs', '$compile', function ($scope, $http, $element, $attrs, $compile) {
                }],
            link: function ($scope, $element, attrs, ctrl) {
            },
            templateUrl: function ($scope, $element, attrs) { return 'StimReportViewer?reportType=' + $element.reportType; },
        };
    }]);
