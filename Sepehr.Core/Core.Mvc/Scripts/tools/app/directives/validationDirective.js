var validationDirective = new SepehrModule.MainModule("validationDirective", []);
validationDirective.addDirective("validations", ["$compile", function ($compile) {
        return {
            restrict: "A",
            replace: true,
            link: function (scope, element, attrs) {
                if (scope.$eval(attrs.validations)) {
                    var rules = scope.$eval(attrs.validations), widgetValidator = element.kendoValidator().data("kendoValidator"), validateFuncArray = [], thisArgs = { model: scope, element: element, message: "" }, ruleName = '';
                    $.each(rules, function (index, item) {
                        var validationClass, validatorFunc = window[item.type];
                        thisArgs.message = item.message;
                        if (item.params) {
                            var variablesList = new Array();
                            $.each(item.params, function (i, paramList) {
                                try {
                                    variablesList.push(scope.$eval(paramList));
                                }
                                catch (ex) {
                                    console.log("object " + paramList.split("\'")[1] + "doesn't exist in the scope");
                                }
                            });
                        }
                        validatorFunc.prototype.message = thisArgs.message,
                            validatorFunc.prototype.model = thisArgs.model,
                            validatorFunc.prototype.element = thisArgs.element;
                        validationClass = new validatorFunc();
                        validateFuncArray.push({ name: item.type, instance: validationClass, variables: item.params });
                        ruleName += item.type;
                    });
                    widgetValidator.options.rules[ruleName] = function (el) {
                        var result = true;
                        $.each(validateFuncArray, function (index, item) {
                            var args = new Array();
                            var thisArgs = { model: scope, element: el, message: item.instance.message };
                            $.each(item.variables, function (i, variable) {
                                try {
                                    args.push(scope.$eval(variable));
                                }
                                catch (ex) {
                                    console.log("object " + variable.split("\'")[1] + "doesn't exist in the scope");
                                }
                            });
                            result = result && item.instance.Validate.apply(thisArgs, args);
                            if (!result) {
                                widgetValidator.options.messages[ruleName] = thisArgs.message;
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
