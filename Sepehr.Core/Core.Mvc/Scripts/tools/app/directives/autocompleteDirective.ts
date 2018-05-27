var autocompleteDirectiveModule = new SepehrModule.MainModule("autocompleteDirectiveModule", []);
autocompleteDirectiveModule.addDirective("autocomplete", ["$compile", "$http", function ($compile, $http) {
    return {
        restrict: "E",
        replace: true,
        transclude: true,
        scope: {
            id: "@",
            name: "=",
            filterType: "@",
            searchProperty: "@",
            waterMark: "@?",
            displayName: "@",
            valueName: "@",
            propertyId: "=",
            propertyName: "=",
            selectedItem: "=?",
            url: "@",
            width: "@",
            customDataSource: "=?source",
            dataBound: "&",
            select: "&",
            change: "&",
            //     validators: "=?"
        },
        controller: ["$scope", "$element", "$attrs", "$q", function ($scope, $element, $attrs, $q) {

            $scope.templateField = "${data." + $scope.displayName + "}";

            var GetDataSource = () => {
                var defer = $q.defer();
                $http.get($scope.url)
                    .success((data) => {
                        defer.resolve(data);
                    })
                    .error(function () {
                        defer.reject("Failed to get datasource");
                    });
                return defer.promise;
            }

            if ($scope.url) {
                GetDataSource().then((dataList) => {
                    $scope.customDataSource = dataList;
                    if ($scope.propertyId) {
                        $scope.selectedItem = dataList.find((data) => {
                            if (data[$scope.valueName] == $scope.propertyId) {
                                $scope.propertyName = data[$scope.displayName];
                                $scope.autocomplete.trigger("change");
                                return data;
                            }
                        });

                    }
                });
            }
            $scope.onDataBound = (e) => {

                if ($attrs.dataBound) {
                    $scope.dataBound({ args: e, scope: $scope });
                }

            }
            $scope.onChange = (e) => {

                if (!$scope.propertyName) {
                    $scope.propertyId = "";
                }
                else {

                    $scope.selectedItem = $scope.customDataSource.find((data) => {
                        if (data[$scope.displayName] == $scope.propertyName && data[$scope.valueName] == $scope.propertyId) {
                            return data;
                        }
                    });
                    if (!$scope.selectedItem) {
                        $scope.propertyName = "",
                            $scope.propertyId = "";
                    }
                }

                if ($attrs.change) {
                    $scope.change({ args: e, scope: $scope });
                }

            }

            $scope.onSelectItem = function (e) {
                if (e != undefined) {
                    var model = e.sender.dataItem(e.item);
                    $scope.propertyName = model[$scope.displayName],
                        $scope.propertyId = model[$scope.valueName],
                        $scope.autocomplete.value(model[$scope.displayName]);
                    if ($attrs.select) {
                        $scope.select({ args: e, scope: $scope });
                    }
                    $scope.autocomplete.trigger("change");

                    //baraye inke meghdare searchproperty dar input nashine 
                    e.preventDefault();
                }

            };
        }],

        link: function (scope, elem, attrs) {
            var input = elem.find("input");


            scope.onClick = (args) => {

                scope.autocomplete.search("");
                if (scope.autocomplete.popup._closing)
                    scope.autocomplete.search(" ");
            };

            input.attr("width", scope.width ? scope.width : 'auto');

            scope.options = {
                placeholder: scope.waterMark,

            }
        },
        template:
        " <span > "
        + " <input "
        + " ng-model = propertyName "
        + " type= 'text'"
        + " id= '{{id}}'"
        + " kendo-auto-complete='autocomplete' "
        + " k-data-source='customDataSource' "
        + " k-data-text-field='searchProperty' "
        + " k-filter='filterType' "
        + " k-template='templateField' "
        + " k-select='onSelectItem' "
        + " k-data-bound='onDataBound' "
        + " k-options='options' "
        + " k-change='onChange' "
        + " ng-click=onClick($event) "
        + " validations='validators'"
        + "/> "
        + " </ span>"

    };
}]);