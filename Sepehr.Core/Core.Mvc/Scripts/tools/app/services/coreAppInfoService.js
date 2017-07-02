var coreAppInfoServiceModule = new SepehrModule.MainModule("coreAppInfoServiceModule", []);
coreAppInfoServiceModule.addService('currentTabService', [function () {
        return {
            currentTab: 0
        };
    }]);
coreAppInfoServiceModule.addService("coreAppInfoService", ["currentTabService", function (currentTabService) {
        return {
            getElementInCurrentTabById: function (elementId) {
                var currentTab = currentTabService.currentTab;
                $('#' + currentTab + '').find(elementId);
            }
        };
    }]);
