var coreNotificationService = new SepehrModule.MainModule('coreNotificationService', []);
coreNotificationService.addService('notificationInfo', [function () {
        var notificationInfo = new Notifications();
        return notificationInfo;
    }]);
coreNotificationService.addService('success', [function () {
        return function (info) {
            info.element.success(info.message);
        };
    }]);
coreNotificationService.addService('error', [function () {
        return function (info) {
            $.each(info.message, function (index, msg) {
                info.element.error(msg);
            });
        };
    }]);
coreNotificationService.addService('warning', [function () {
        return function (info) {
            info.element.warning(info.message);
        };
    }]);
coreNotificationService.addService('info', [function () {
        return function (info) {
            info.element.info(info.message);
        };
    }]);
