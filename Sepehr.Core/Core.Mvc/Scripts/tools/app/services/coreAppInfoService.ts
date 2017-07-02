var coreAppInfoServiceModule = new SepehrModule.MainModule("coreAppInfoServiceModule", []);


coreAppInfoServiceModule.addService('currentTabService', [() => {

    return {
        currentTab: 0
    };

}]);
coreAppInfoServiceModule.addService("coreAppInfoService", ["currentTabService", function (currentTabService) {


    return {

        getElementInCurrentTabById: (elementId) => {
           
            var currentTab = currentTabService.currentTab;
            $('#' + currentTab + '').find(elementId);
        }
    }
}]); 