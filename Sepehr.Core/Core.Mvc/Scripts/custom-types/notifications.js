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
var NotificationMode = (function () {
    function NotificationMode() {
        this.error = "error";
        this.warning = "warning";
        this.succes = "succes";
        this.info = "info";
    }
    return NotificationMode;
}());
var Notifications = (function () {
    function Notifications(title, message) {
        this.title = title;
        this.message = message;
        this.element = undefined;
    }
    return Notifications;
}());
var SuccessNotification = (function (_super) {
    __extends(SuccessNotification, _super);
    function SuccessNotification(title, message) {
        if (title === void 0) { title = "موفقیت"; }
        var _this = _super.call(this, title, message) || this;
        _this.icon = "Areas/Core/Content/images/success-icon.png";
        return _this;
    }
    return SuccessNotification;
}(Notifications));
var ErrorNotification = (function (_super) {
    __extends(ErrorNotification, _super);
    function ErrorNotification(title, message) {
        if (title === void 0) { title = "خطا"; }
        var _this = _super.call(this, title, message) || this;
        _this.icon = "Areas/Core/Content/images/error-icon.png";
        return _this;
    }
    return ErrorNotification;
}(Notifications));
var WarningNotification = (function (_super) {
    __extends(WarningNotification, _super);
    function WarningNotification(title, message) {
        if (title === void 0) { title = "هشدار"; }
        var _this = _super.call(this, title, message) || this;
        _this.icon = "Areas/Core/Content/images/warning-icon.png";
        return _this;
    }
    return WarningNotification;
}(Notifications));
var InfoNotification = (function (_super) {
    __extends(InfoNotification, _super);
    function InfoNotification(title, message) {
        if (title === void 0) { title = "اطلاع"; }
        var _this = _super.call(this, title, message) || this;
        _this.icon = "Areas/Core/Content/images/info-icon.png";
        return _this;
    }
    return InfoNotification;
}(Notifications));
