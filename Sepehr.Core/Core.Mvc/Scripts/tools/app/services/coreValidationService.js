var coreValidationService = new SepehrModule.MainModule('coreValidationService', []);
coreValidationService.addService("validate", [function () {
        return function (container) {
            var result = true, validationObjects = container.find("[validations]");
            $.each(validationObjects, function (index, element) {
                var isLookup = false;
                if ($(element).parent("[lookup-id]").length > 0) {
                    isLookup = true,
                        $(element).removeAttr("readonly");
                }
                var validatorWidget = $(element).data("kendoValidator");
                if (validatorWidget) {
                    result = result && validatorWidget.validate();
                }
                if (isLookup) {
                    $(element).attr("readonly", "readonly");
                }
            });
            return result;
        };
    }]);
