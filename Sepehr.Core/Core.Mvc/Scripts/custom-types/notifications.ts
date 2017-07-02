/// <reference path="../typings/kendo/kendo.all.d.ts" />
class NotificationMode {

    public error: string = "error";
    public warning: string = "warning";
    public succes: string = "succes";
    public info: string = "info";


}
interface INotification {
    title: string;
    message: Array<string>;
    mode: NotificationMode;
    icon: string;
    element:any;

}
class Notifications implements INotification {
    title: string;
    message: Array< string>;
    mode: NotificationMode;
    icon: string;
    element:any ;
    constructor(title?: string, message?: Array<string> ) {
        this.title = title;
        this.message = message;
        this.element = undefined;
        
    }

}
class SuccessNotification extends Notifications {

    constructor(title: string = "موفقیت", message?: Array<string>) {
        super(title, message);
        this.icon = "Areas/Core/Content/images/success-icon.png";
    }
}
class ErrorNotification extends Notifications {

    constructor(title: string = "خطا", message?: Array<string>) {
        super(title, message);
        this.icon = "Areas/Core/Content/images/error-icon.png";
    }
}
class WarningNotification extends Notifications {

    constructor(title: string = "هشدار", message?: Array<string>) {
        super(title, message);
        this.icon = "Areas/Core/Content/images/warning-icon.png";

    }
}
class InfoNotification extends Notifications {

    constructor(title: string = "اطلاع", message?: Array<string>) {
        super(title, message);
        this.icon = "Areas/Core/Content/images/info-icon.png";

    }
}

