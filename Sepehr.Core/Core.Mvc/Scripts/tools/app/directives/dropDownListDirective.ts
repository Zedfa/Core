var ddlDirectiveModule = new SepehrModule.MainModule("dropDownListDirectiveModule", []);
ddlDirectiveModule.addDirective("dropDownList", ["$compile", "$http", function ($compile, $http) {
    return {
        restrict: "E",
        replace: true,
        scope: {
            id: "@",
            displayName: "@",
            valueName: "@",
            //model: "=",
            propertyId: "=",
            propertyName: "=",
            selectedItem: "=?",
            url: "@",
            width: "@",
            customChange: "&",
            onDataBound: "&",
            dbCategoryName: "@",
            customDataSource: "=?source"
            // onSelect:"&"
        },
        controller: ["$scope", "$element", "$attrs", function ($scope, $element, $attrs) {
            if ($attrs.dbCategoryName && $attrs.dbCategoryName != "null") {
                $scope.displayName = "Key",
                    $scope.valueName = "Value",
                    $scope.url = "/api/ConstantsAPi/GetConstantByNameOfCategory?category=" + $scope.dbCategoryName;
            }

            if ($scope.url) {
                $scope.customDataSource = {
                    transport: {
                        read: {
                            dataType: "json",
                            url: $scope.url,
                        }
                    },

                };
            }


            $scope.dataBound = function (e) {


                var text = $scope.propertyName, //$scope.model[$scope.propertyName],
                    val = $scope.propertyId//$scope.model[$scope.propertyId];
                if (text) {
                    e.sender.search(text);
                }
                else {
                    var foundedItem;

                    var len = e.sender.dataSource.data().length;
                    for (var i = 0; i < len; i++) {
                        var record = e.sender.dataSource.data()[i];
                        if (record[$scope.valueName] == val) {
                            foundedItem = i;
                            return;
                        }
                    }

                    e.sender.select(foundedItem);

                    setModel(e.sender);

                }

                if ($scope.$eval($attrs.onDataBound)) {
                    $scope.onDataBound({ args: e, scope: $scope });
                }
            };

            $scope.change = function (e) {
                setModel(e.sender);

                if (typeof $scope.customChange == 'function') {
                    $scope.customChange({ args: e });
                }

            };
            $scope.onSelectItem = function (e) {
                var selectedItem = e.sender.dataItems()[e.item.index()];
                if (e.item.index() == -1 && e.sender.dataItems() > 0) {
                    e.select(0);
                }


                $scope.selectedItem = selectedItem;
                $scope.propertyName = selectedItem[$scope.displayName];
                $scope.propertyId = selectedItem[$scope.valueName];

                if (typeof $scope.customChange == 'function') {
                    $scope.customChange({ args: e });
                }


            };




            var setModel = function (dropdown) {

                if (dropdown.selectedIndex == -1 && dropdown.dataSource.data().length > 0) {
                    dropdown.select(0);
                }

                var selectedIndex = dropdown.selectedIndex,
                    selectedItem = dropdown.dataItem(selectedIndex);
                if (selectedItem) {
                    $scope.selectedItem = selectedItem;
                    //$scope.model[$scope.propertyName] = selectedItem[$scope.displayName];
                    $scope.propertyName = selectedItem[$scope.displayName];

                    //$scope.model[$scope.propertyId] = selectedItem[$scope.valueName];
                    $scope.propertyId = selectedItem[$scope.valueName];
                }
            };


        }],
        link: function (scope, elem, attrs) {

            elem.find("input").attr("width", scope.width ? scope.width : 'auto');
            if (scope.url) {
                scope.dropdown.reloadByUrl = (url: string) => {
                    scope.customDataSource.transport.read.url = url;
                    scope.customDataSource.transport.dataSource.read();

                };

            }
            //else {

            //    scope.$watch("dropdown", (n, o) => {

            //        if (n) {
            //            scope.isWidgetCreated = true;
            //        }
            //    });
            //    scope.$watch("customDataSource", (n, o) => {

            //        if (n) {
            //            scope.isDataSourceChanged = true;
            //        }
            //    }, true);
            //    scope.$watch("isWidgetCreated +isDataSourceChanged", (n, o) => {
            //        if (scope.isWidgetCreated && scope.isDataSourceChanged) {
            //            scope.dropdown.setDataSource(scope.customDataSource);
            //            scope.dropdown.refresh();
            //        }
            //    });

            //}
        },

        template: "<span ><input id='{{id}}' kendo-drop-down-list='dropdown' k-data-text-field='displayName' k-data-value-field='valueName' k-data-source='customDataSource'"
        + " k-on-change='change(kendoEvent)' k-on-select='onSelectItem(kendoEvent)' k-on-data-bound='dataBound(kendoEvent)' /> </span>"


    };
}]);
