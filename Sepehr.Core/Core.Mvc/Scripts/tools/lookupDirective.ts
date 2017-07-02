var lookupDirectiveModule = angular.module("lookupDirectiveModule", []);
//@(Html.LookUpCr<RoleViewModel>("RoleName", "انتخاب نقش", RoleViewModel.LookupInfo as GridInfo, "Id", "Name", "RoleId"))

lookupDirectiveModule.directive("custLookup", ["$compile", function ($compile) {

    return {
        restrict: "E",
        replace: true,
        scope: {
            typ: "@",
            msg: "@",
            lookupId: "@",
            valueName: "@",
            displayName: "@",
            lkpDisplayName: "@",
            bindingObj: "=",
            viewModelName: "@",
            treeConfig: "@",
            gridConfig: "=",
            buttonClicked: "&",
            name: "@",
            watermark: "@",
            isRequired: "@",
            valCustPattern: "@",
            valCustMsg: "@",
            securityTag: "@",
            winWidth: "@",
            winHeight: "@"
        },

        controller: function ($scope, $element) {
            var LKPTYPE = "", GRID = "Grid", TREE = "Tree";
            var LKPPREFIX = "lkp_";
            $scope.lkpId = LKPPREFIX + $scope.lookupId;

            LKPTYPE = !$scope.typ ? GRID : TREE;
            if (LKPTYPE === GRID) {
                $scope.isLookup = true;
                $scope.bindingObj = $scope.$parent.selectedItem;
                $scope.change = function (scope, fldName) {
                    //debugger;
                    //if (fldName==="valueName")
                    //    scope.bindingObj[scope[fldName]] = scope.localObj.val;
                    //else if (fldName==="displayName")
                    //    scope.bindingObj[scope[fldName]] = scope.localObj.display;
                }
                $scope.localObj = { val: '', display: '' };
                $scope.localObj.val = $scope.bindingObj[$scope.valueName];
                $scope.localObj.display = $scope.bindingObj[$scope.displayName];
                $scope.openLookupWindow = function (scope) {
                    var win = scope[$scope.lkpId + "TemplateWindow"];
                    win.center().open();
                }
                $scope.dblClick = function (args) {
                    var trItem = $("tr[data-uid='" + args.trUId + "']");
                    var sender = args.scope[$scope.lkpId];
                    var selected = sender.dataItem(trItem);
                    //$scope.bindingName.wholeRow = selected;
                    $scope.localObj.display = selected[$scope.lkpDisplayName];
                    $scope.localObj.val = selected.Id;//selected[$scope.valueName];
                    var win = $scope[$scope.lkpId + "TemplateWindow"];
                    win.close();
                }

                $scope.$watch("localObj", function (item) {
                    if (item.val != 0) {
                        $scope.bindingObj[$scope["valueName"]] = item.val;
                        $scope.bindingObj[$scope["displayName"]] = item.display;
                    }
                }, true);
            }
        },

        link: function (scope, $element, $document) {

        },

        templateUrl: "/Template/GetLookupTemplate"

    }

}]);

