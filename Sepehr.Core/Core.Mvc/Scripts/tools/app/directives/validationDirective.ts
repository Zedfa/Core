/// <reference path="../../../custom-types/validators.ts" />

var validationDirective = new SepehrModule.MainModule("validationDirective", [])
validationDirective.addDirective("validations", ["$compile", ($compile) => {
    return {
        restrict: "A",
        replace: true,
        //scope: {
        //     validations: "="
        //},
        link: function (scope, element: JQuery, attrs) {


            //if (scope.validations) {
            if (scope.$eval(attrs.validations)) {
                var rules = scope.$eval(attrs.validations),
                    widgetValidator = element.kendoValidator().data("kendoValidator"),
                    validateFuncArray = [],
                    ruleName = '';


                $.each(rules, (index: number, item: ValidationAttribute) => {

                    var validationClass: IValidator,
                        validatorFunc: Function = window[item.type];

                    if (item.params) {
                        var variablesList: Array<any> = new Array<any>();
                        variablesList.push(null),
                            variablesList.push(element),
                            variablesList.push(item.message);

                        $.each(item.params, (i, paramList) => {
                          
                            variablesList.push(scope.$eval(paramList) ? scope.$eval(paramList) : paramList);

                            // for (var input in paramList) { variablesList.push(paramList[input]); }
                        });
                        validationClass = new (Function.prototype.bind.apply(validatorFunc, variablesList));
                    }
                    else {
                        validationClass = new (Function.prototype.bind.apply(validatorFunc, [null, element, item.message]));
                    }

                    validateFuncArray.push({ name: item.type, instance: validationClass, variables: item.params });
                    ruleName += item.type;
                });


                widgetValidator.options.rules[ruleName] = (el) => {
                    var result = true;
                    $.each(validateFuncArray, (index, item) => {
                        var args: Array<any> = new Array<any>();

                        $.each(item.variables, (i, variable) => {
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
    }


}]);
