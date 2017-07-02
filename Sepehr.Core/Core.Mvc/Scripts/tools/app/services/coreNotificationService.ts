/// <reference path="../../sepehrappmodule.ts" />
/// <reference path="../../generaltools.ts" />
/// <reference path="../../../custom-types/notifications.ts" />
var coreNotificationService = new SepehrModule.MainModule('coreNotificationService', []);

coreNotificationService.addService('notificationInfo', [() => {
    var notificationInfo = new Notifications();
    return notificationInfo;
    //return {
    //    title : "",
    //    message : ""
    //};
}]);

coreNotificationService.addService('success', [ () => {
    return (info:SuccessNotification) => {
        info.element.success(info.message);
    }
}]);

coreNotificationService.addService('error', [() => {
    return (info: ErrorNotification) => {
        $.each(info.message,(index, msg) => {
            info.element.error(msg);
        });
    };
}]);
coreNotificationService.addService('warning', [ () => {
    return (info: WarningNotification) => {
        info.element.warning(info.message);
    }
}]);
coreNotificationService.addService('info', [() => {
    return (info: InfoNotification) => {
        info.element.info(info.message);
    }
}]);
//coreNotificationService.addService('showMessage', ['notificationInfo', ( notificationInfo) => {
//    return () => {
//        notificationInfo.element.show();
//    }
//}]);
//coreNotificationService.addService('hideMessage', ['notificationInfo', (notificationInfo) => {
//    return () => {
//        notificationInfo.element.hide();
//    }
//}]);


