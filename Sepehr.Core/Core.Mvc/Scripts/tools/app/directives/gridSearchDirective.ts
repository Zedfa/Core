var gridSearchDirectiveModule = angular.module("gridSearchDirectiveModule", ["grdSearchServiceModule"]);
gridSearchDirectiveModule.directive("gridSearch", ["grdSearchService", function (grdSearchService) {
    return {
        restrict: "E",
        replace: true,
        scope: {
            seTitle: "@",
            grdSeId: "@",
            okClicked: "&",
            cancelClicked: "&",
            winWidth: "@",
            winHeight: "@",
        },
        controller: function ($scope, $element) {
           
        },

        link: function (scope, elem, attrs) {
            var template = grdSearchService.getWinGrdSearchTemplate();

            scope.btnOkClicked = function (arg) {
                if (attrs.okClicked) { 
                    scope.okClicked({ args: arg });
                }
            };
            scope.btnCancelClicked = function (arg) {
                if (attrs.cancelClicked) {
                    scope.cancelClicked({ args: arg });
                }
            };
           
        },
        //template: grdSearchService.
    };
}]); 