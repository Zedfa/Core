var validationDirective = new SepehrModule.MainModule("validationDirective", []);
validationDirective.addDirective("validations", ["$compile", function ($compile) {
        return {
            restrict: "A",
            replace: true,
            link: function (scope, element, attrs) {
                if (scope.$eval(attrs.validations)) {
                    var rules = scope.$eval(attrs.validations), widgetValidator = element.kendoValidator().data("kendoValidator"), validateFuncArray = [], ruleName = '';
                    $.each(rules, function (index, item) {
                        var validationClass, validatorFunc = window[item.type];
                        if (item.params) {
                            var variablesList = new Array();
                            variablesList.push(null),
                                variablesList.push(element),
                                variablesList.push(item.message);
                            $.each(item.params, function (i, paramList) {
                                variablesList.push(scope.$eval(paramList) ? scope.$eval(paramList) : paramList);
                            });
                            validationClass = new (Function.prototype.bind.apply(validatorFunc, variablesList));
                        }
                        else {
                            validationClass = new (Function.prototype.bind.apply(validatorFunc, [null, element, item.message]));
                        }
                        validateFuncArray.push({ name: item.type, instance: validationClass, variables: item.params });
                        ruleName += item.type;
                    });
                    widgetValidator.options.rules[ruleName] = function (el) {
                        var result = true;
                        $.each(validateFuncArray, function (index, item) {
                            var args = new Array();
                            $.each(item.variables, function (i, variable) {
                                args.push(scope.$eval(variable) ? scope.$eval(variable) : variable);
                            });
                            result = result && item.instance.Validate(el, args);
                            if (!result) {
                                widgetValidator.options.messages[ruleName] = item.instance.message;
                                return result;
                            }
                        });
                        attrs.$set("isValid", result.toString());
                        return result;
                    };
                }
            }
        };
    }]);
