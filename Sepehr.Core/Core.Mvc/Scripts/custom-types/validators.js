var ValidatorBase = (function () {
    function ValidatorBase() {
        this.message = "";
    }
    ValidatorBase.prototype.Validate = function (element) {
        throw ("implements must be necessary");
    };
    return ValidatorBase;
}());
var RequiredValidator = (function () {
    function RequiredValidator() {
        this.message = "ورود اطلاعات الزامی است";
        this.element.addClass("required-validation");
        return this;
    }
    RequiredValidator.prototype.Validate = function () {
        var text = this.element.val(), result = false;
        if (text && text.trim() != "") {
            result = true;
        }
        return result;
    };
    return RequiredValidator;
}());
var MaxLengthValidator = (function () {
    function MaxLengthValidator() {
        this.message = "طول رشته مجاز نیست";
    }
    MaxLengthValidator.prototype.Validate = function () {
        var text = this.element.val(), result = false;
        if (text.length < this.len) {
            result = true;
        }
        return result;
    };
    return MaxLengthValidator;
}());
var ValidationAttribute = (function () {
    function ValidationAttribute() {
        this.type = "";
        this.params = [];
        this.message = "";
    }
    return ValidationAttribute;
}());
