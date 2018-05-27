/// <reference path="../../../custom-types/validators.ts" />

var validationDirective = new SepehrModule.MainModule("validationDirective", [])
validationDirective.addDirective("validations", ["$compile", ($compile) => {
    return {
        restrict: "A",
        replace: true,
        link: function (scope, element: JQuery, attrs) {


            //if (scope.validations) {
            if (scope.$eval(attrs.validations)) {
                var rules = scope.$eval(attrs.validations),
                    widgetValidator = element.kendoValidator().data("kendoValidator"),
                    validateFuncArray = [],
                    thisArgs = { model: scope, element: element, message: "" },
                    ruleName = '';


                $.each(rules, (index: number, item: ValidationAttribute) => {

                    var validationClass: IValidator,
                        validatorFunc = window[item.type];

                    thisArgs.message = item.message;

                    if (item.params) {
                        var variablesList: Array<any> = new Array<any>();
                        //variablesList.push(scope),
                        //    variablesList.push(element),
                        //    variablesList.push(item.message);
                        //variablesList.push(initialArgs);

                        $.each(item.params, (i, paramList) => {
                            try {
                                variablesList.push(scope.$eval(paramList) );
                            }
                            catch (ex) {
                                console.log("object " + paramList.split("\'")[1] + "doesn't exist in the scope");

                            }
                                                    
                        });
                      //  validationClass = new (Function.prototype.bind.apply(validatorFunc, variablesList));
                    }
                    //else {

                    validatorFunc.prototype.message = thisArgs.message,
                        validatorFunc.prototype.model = thisArgs.model,
                        validatorFunc.prototype.element = thisArgs.element;

                    validationClass = new validatorFunc();
                    //validatorFunc.prototype.constructor.apply(thisArgs, variablesList),
                    //    validationClass = validatorFunc.prototype;

                     //Object.create(Function.prototype.bind.apply(validatorFunc, variablesList));

                        //validationClass.element = thisArgs.element,
                        //validationClass.model = thisArgs.model,
                        //validationClass.message = thisArgs.message;

                                      
                    //}
                   
                    validateFuncArray.push({ name: item.type, instance: validationClass, variables: item.params });
                    ruleName += item.type;
                });


                widgetValidator.options.rules[ruleName] = (el) => {
                    var result = true;
                    $.each(validateFuncArray, (index, item) => {
                        var args: Array<any> = new Array<any>();
                     
                        var thisArgs = { model: scope, element: el, message: item.instance.message };

                        $.each(item.variables, (i, variable) => {
                            try {
                                args.push(scope.$eval(variable));
                            }
                            catch (ex) {
                                console.log("object " + variable.split("\'")[1] + "doesn't exist in the scope");

                            }
                        });
                      
                        result = result && item.instance.Validate.apply(thisArgs , args);
                        
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
    }


}]);
