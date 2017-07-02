var pricesFormatDirectiveModule = new SepehrModule.MainModule("priceFormatDirectiveModule", []);
pricesFormatDirectiveModule.addDirective('priceFormat', [function () {
        return {
            restrict: 'A',
            scope: {
                priceValue: "=",
            },
            link: function (scope, element, attrs) {
                scope.$watch('priceValue', function (value) {
                    if (value) {
                        element.val(kendo.toString(Number(value), "n0"));
                    }
                    else {
                        element.val("");
                    }
                });
                element.bind('keydown', function (e) {
                    if (!(e.keyCode >= 48 && e.keyCode <= 57)
                        &&
                            !(e.keyCode >= 96 && e.keyCode <= 105)
                        &&
                            e.keyCode != 8
                        &&
                            e.keyCode != 9
                        &&
                            e.keyCode != 37
                        &&
                            e.keyCode != 39
                        &&
                            e.keyCode != 27
                        &&
                            e.keyCode != 46) {
                        event.preventDefault();
                    }
                });
                element.bind('keyup', function (blurEvent) {
                    if (element.data('old-value') != element.val()) {
                        scope.priceValue = element.val().replace(/,/g, '');
                        if (scope.priceValue && kendo.toString(Number(scope.priceValue), "n0").toString() != "NaN") {
                            element.val(kendo.toString(Number(scope.priceValue), "n0"));
                        }
                        else {
                            element.val('');
                        }
                        scope.$apply(function () {
                            scope.priceValue = element.val().replace(/,/g, '');
                        });
                    }
                });
            },
        };
    }]);
