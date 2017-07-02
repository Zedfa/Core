var coreMainServiceModule = new SepehrModule.MainModule('coreMainServiceModule', []);



coreMainServiceModule.addService('crossGrids', [() => {

    var sharedData = {};

    return {

        setGridData: function (gId, data) {
            sharedData[gId] = data;
        },

        getGridData: function (gId) {
            return sharedData[gId];
        }
    }
    


}]);



