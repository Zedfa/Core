var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var ValidatorBase = (function () {
    function ValidatorBase() {
        this.message = "ورود اطلاعات الزامی است";
    }
    ValidatorBase.prototype.Validate = function (element) {
        throw ("implements must be necessary");
    };
    return ValidatorBase;
}());
var RequiredValidator = (function (_super) {
    __extends(RequiredValidator, _super);
    function RequiredValidator(element, message) {
        var _this = _super.call(this) || this;
        _this.element = element.addClass("required-validation");
        return _this;
    }
    RequiredValidator.prototype.Validate = function (element) {
        var text = element.val(), result = false;
        if (text && text.trim() != "") {
            result = true;
        }
        return result;
    };
    return RequiredValidator;
}(ValidatorBase));
var MaxLengthValidator = (function (_super) {
    __extends(MaxLengthValidator, _super);
    function MaxLengthValidator(len, message) {
        var _this = _super.call(this) || this;
        _this.message = message;
        _this.len = len;
        return _this;
    }
    MaxLengthValidator.prototype.Validate = function (element) {
        var text = element.val(), result = false;
        if (text.length < this.len) {
            result = true;
        }
        return result;
    };
    return MaxLengthValidator;
}(ValidatorBase));
var ValidationAttribute = (function () {
    function ValidationAttribute() {
        this.type = "";
        this.params = [];
        this.message = "";
    }
    return ValidationAttribute;
}());
