var lookupDirectiveModule = new SepehrModule.MainModule("lookupDirectiveModule", []);
lookupDirectiveModule.addDirective("custLookup", ["$http", "$parse", "$compile", function ($http, $parse, $compile) {
        return {
            restrict: "E",
            replace: true,
            transclude: true,
            scope: {
                typ: "@",
                msg: "@",
                title: "@",
                lookupId: "@",
                valueName: "=",
                displayName: "=",
                lkpValueName: "@",
                lkpDisplayName: "@",
                lkpPropName: "@",
                viewModelName: "@",
                dtoType: "@",
                treeConfig: "@",
                gridConfig: "=",
                name: "@",
                watermark: "@",
                winWidth: "@",
                winHeight: "@",
                lkpPreDblClick: "&",
                lkpDblClick: "&",
                lkpInit: "&",
                lkpDataBound: "&",
                lkpDataBinding: "&",
                lkpCreate: "&",
                lkpPreSave: "&",
                lkpPostSave: "&",
                lkpCodeName: "&",
                selectedItem: "=",
                shortcutCodeName: "@",
                shortcutCode: "=",
                validators: "=",
                scrollPosition: "@"
            },
            controller: ["$scope", "$element", "$attrs", function ($scope, $element, $attrs) {
                    var LKPTYPE = "", GRID = "Grid", TREE = "Tree";
                    var initialFilterRule = {};
                    LKPTYPE = !$scope.typ ? GRID : getLookupType();
                    $scope.scrollPosition = $scope.scrollPosition ? $scope.scrollPosition : "top";
                    function getLookupType() {
                        if ($scope.typ.toLowerCase() === GRID.toLowerCase()) {
                            return GRID;
                        }
                        else if ($scope.typ.toLowerCase() === TREE.toLowerCase()) {
                            return TREE;
                        }
                        return GRID;
                    }
                    if (LKPTYPE === GRID) {
                        $scope.isLookup = true;
                        $scope.change = function (scope, fldName) {
                        };
                    }
                    $scope.openLookupWindow = function (scope) {
                        if ($attrs.lkpInit) {
                            $scope.lkpInit({ args: $scope.gridScope });
                        }
                        else {
                            $scope.readData($scope.gridScope, null, null);
                        }
                        var win = scope[$scope.lookupId + "TemplateWindow"];
                        win.center().open();
                    };
                    $scope.lclInit = function (arg) {
                        getData(arg);
                        $scope.gridScope = arg;
                    };
                    $scope.lclDataBound = function (arg) {
                        if ($attrs.lkpDataBound) {
                            $scope.lkpDataBound({ args: arg });
                        }
                    };
                    function getData(arg) {
                        var currentFilter = arg[$scope.lookupId].dataSource.options["filter"];
                        initialFilterRule = jQuery.extend(true, {}, currentFilter);
                        var finalFilterRule = null;
                        if (currentFilter && angular.isArray(currentFilter.Filters)) {
                            if (currentFilter.Filters.length == 1) {
                                currentFilter.Filters.push($scope.getFilterRule());
                                finalFilterRule = {
                                    logic: "and",
                                    filters: currentFilter
                                };
                            }
                            else if (currentFilter.Filters.length == 2) {
                                var tempFilter = {
                                    logic: "and",
                                    filters: [$scope.getFilterRule(), currentFilter]
                                };
                                finalFilterRule = tempFilter;
                            }
                        }
                        if ($scope.valueName) {
                            finalFilterRule = [$scope.getFilterRule()];
                            $scope.readData(arg, finalFilterRule, true);
                        }
                    }
                    $scope.readData = function readGridData(arg, filtr, edit, url) {
                        var grd = arg[$scope.lookupId];
                        if (filtr && typeof filtr == 'object') {
                            if (edit) {
                                $http({
                                    url: url ? url : grd.dataSource.transport.options.read.url,
                                    method: 'GET',
                                    params: {
                                        filter: serializeFilter(filtr, false),
                                    },
                                    dataType: "JSON"
                                }).success(function (data) {
                                    if (data && data.Data && data.Data.length == 1) {
                                        $scope.selectedItem = data.Data[0];
                                        $scope.shortcutCode = $scope.selectedItem[$scope.shortcutCodeName];
                                        $scope.displayName = $scope.selectedItem[$scope.lkpDisplayName];
                                        $scope.valueName = $scope.selectedItem[$scope.lkpValueName];
                                    }
                                    else {
                                        $scope.selectedItem = undefined;
                                        $scope.shortcutCode = "";
                                        $scope.displayName = "کد اشتباه است";
                                        $scope.valueName = undefined;
                                    }
                                    $scope.lastShortcutCodeSearched = $scope.shortcutCode;
                                });
                            }
                            else {
                                grd.dataSource.filter(filtr);
                            }
                        }
                        else {
                            grd.dataSource.read();
                        }
                    };
                    $scope.getFilterRule = function () {
                        return {
                            field: $scope.lkpValueName,
                            operator: 'eq',
                            value: $scope.valueName
                        };
                    };
                    function serializeFilter(filter, encode) {
                        if (filter && angular.isArray(filter) && filter.length == 1) {
                            filter = filter[0];
                        }
                        if (filter.filters) {
                            return $.map(filter.filters, function (f) {
                                var hasChildren = f.filters && f.filters.length > 1, result = serializeFilter(f, encode);
                                if (result && hasChildren) {
                                    result = "(" + result + ")";
                                }
                                return result;
                            }).join("~" + filter.logic + "~");
                        }
                        if (filter.field) {
                            return filter.field + "~" + filter.operator + "~" + encodeFilterValue(filter.value, encode);
                        }
                        else {
                            return undefined;
                        }
                    }
                    var escapeQuoteRegExp = /'/ig;
                    function encodeFilterValue(value, encode) {
                        if (typeof value === "string") {
                            if (value.indexOf('Date(') > -1) {
                                value = new Date(parseInt(value.replace(/^\/Date\((.*?)\)\/$/, '$1'), 10));
                            }
                            else {
                                value = value.replace(escapeQuoteRegExp, "''");
                                if (encode) {
                                    value = encodeURIComponent(value);
                                }
                                return "'" + value + "'";
                            }
                        }
                        if (value && value.getTime) {
                            return "datetime'" + kendo.format("{0:yyyy-MM-ddTHH-mm-ss}", value) + "'";
                        }
                        return value;
                    }
                }],
            link: function (scope, elem, attrs, ctrl) {
                scope.dblClick = function (args) {
                    var trItem = $("tr[data-uid='" + args.trUId + "']");
                    var sender = args.scope[scope.lookupId];
                    var selected = sender.dataItem(trItem);
                    scope.selectedItem = selected;
                    scope.displayName = selected[scope.lkpDisplayName];
                    scope.valueName = selected[scope.lkpValueName];
                    scope.shortcutCode = scope.selectedItem[scope.shortcutCodeName];
                    var win = scope[scope.lookupId + "TemplateWindow"];
                    if (attrs.lkpDblClick) {
                        scope.lkpDblClick({ args: { scope: scope, grid: sender } });
                    }
                    win.close();
                };
                $(elem).find("button").focus();
                scope.onShortcutCodekeypress = function (args, event) {
                    if (event.keyCode == 13) {
                        scope.filterByField(scope.shortcutCodeName, scope.shortcutCode);
                    }
                };
                scope.onShortcutCodeBlur = function (args) {
                    scope.filterByField(scope.shortcutCodeName, scope.shortcutCode);
                };
                var listener = scope.$watch("valueName", function (current, old) {
                    if (current && current != old)
                        scope.filterByField(scope.lkpValueName, current);
                });
                elem.on('$destroy', function () { listener(); });
                scope.filterByField = function (fieldName, val) {
                    if (!val) {
                        scope.selectedItem = undefined;
                        scope.shortcutCode = "";
                        scope.displayName = "";
                        scope.valueName = undefined;
                        scope.lastShortcutCodeSearched = "";
                    }
                    else if (val != scope.lastShortcutCodeSearched) {
                        var filterObj = {
                            field: fieldName,
                            operator: 'eq',
                            value: val
                        };
                        scope.readData(scope.gridScope.gridOptions, filterObj, true, scope.gridScope.gridOptions.dataSource.transport.read.url);
                    }
                };
            },
            templateUrl: "/Core/Template/GetLookupTemplate"
        };
    }]);
