var ddlDirectiveModule = new SepehrModule.MainModule("dropDownListDirectiveModule", []);
ddlDirectiveModule.addDirective("dropDownList", ["$compile", "$http", function ($compile, $http) {
        return {
            restrict: "E",
            replace: true,
            scope: {
                id: "@",
                displayName: "@",
                valueName: "@",
                propertyId: "=",
                propertyName: "=",
                selectedItem: "=",
                url: "@",
                width: "@",
                customChange: "&",
                onDataBound: "&",
                dbCategoryName: "@"
            },
            controller: ["$scope", "$element", "$attrs", function ($scope, $element, $attrs) {
                    if ($attrs.dbCategoryName && $attrs.dbCategoryName != "null") {
                        $scope.displayName = "Key",
                            $scope.valueName = "Value",
                            $scope.url = "/api/ConstantsAPi/GetConstantByNameOfCategory?category=" + $scope.dbCategoryName;
                    }
                    $scope.customDataSource = {
                        transport: {
                            read: {
                                dataType: "json",
                                url: $scope.url,
                            }
                        },
                    };
                    $scope.dataBound = function (e) {
                        var text = $scope.propertyName, val = $scope.propertyId;
                        if (text) {
                            e.sender.search(text);
                        }
                        else {
                            var foundedItem;
                            $.each(e.sender.dataSource.data(), function (index, record) {
                                if (record[$scope.valueName] == val) {
                                    foundedItem = index;
                                    return;
                                }
                            });
                            e.sender.select(foundedItem);
                            setModel(e.sender);
                        }
                        if ($scope.onDataBound) {
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
                        var selectedIndex = dropdown.selectedIndex, selectedItem = dropdown.dataItem(selectedIndex);
                        $scope.selectedItem = selectedItem;
                        $scope.propertyName = selectedItem[$scope.displayName];
                        $scope.propertyId = selectedItem[$scope.valueName];
                    };
                }],
            link: function (scope, elem, attrs) {
                elem.find("input").attr("width", scope.width ? scope.width : 'auto');
                scope.dropdown.reloadByUrl = function (url) {
                    scope.customDataSource.transport.read.url = url;
                    scope.customDataSource.transport.dataSource.read();
                };
            },
            template: "<span ><input id='{{id}}' kendo-drop-down-list='dropdown' k-data-text-field='displayName' k-data-value-field='valueName' k-data-source='customDataSource'"
                + " k-on-change='change(kendoEvent)' k-on-select='onSelectItem(kendoEvent)' k-on-data-bound='dataBound(kendoEvent)' /> </span>"
        };
    }]);
