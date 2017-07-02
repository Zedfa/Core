
var managePagesSeviceModule = new SepehrModule.MainModule("managePagesSeviceModule", []);


managePagesSeviceModule.addService("managePagesSevice", ['$q', 'CurrentPagesSevice', 'AccessibleViewElementsService', 'viewElementService', "managePagesSeviceByAccessibleViewElementsService", 
    function ($q, CurrentPagesSevice, AccessibleViewElementsService, viewElementService, managePagesSeviceByAccessibleViewElementsService) {
        return {

            managePages: (selectedPageUniqueName, queryString) => {
               
                var deffered = $q.defer();
                if (AccessibleViewElementsService.AccessibleViewElements == null) {
                    viewElementService.GetAccessibleViewElements().then((data) => {
                        AccessibleViewElementsService.AccessibleViewElements = data;
                        deffered.resolve();
                        managePagesSeviceByAccessibleViewElementsService.appendPage(selectedPageUniqueName, queryString, data, deffered);

                    });
                }
                else {
                    deffered.resolve();

                    managePagesSeviceByAccessibleViewElementsService.appendPage(selectedPageUniqueName, queryString, AccessibleViewElementsService.AccessibleViewElements, deffered);
                }
                return deffered.promise;

            }

        }
    }]);


managePagesSeviceModule.addService("managePagesSeviceByAccessibleViewElementsService", ["$compile", "CurrentPagesSevice", function ($compile, CurrentPagesSevice) {
    return {

        appendPage: (selectedPageUniqueName, queryString, AccessibleViewElements) => {
            var isVMExist = false;
            $.each(AccessibleViewElements, (index, val) => {
                if (val.UniqueName.split('#')[0] == selectedPageUniqueName) {
                    isVMExist = true;
                }
            });

            if (!isVMExist) {
                selectedPageUniqueName = "Error";
                queryString = "";
            }

            var isPageExist = false;
            var currentPages = CurrentPagesSevice.getCurrentPages();

            if (currentPages.length > 0) {
                currentPages.forEach((page) => {
                    if (page.Url == selectedPageUniqueName + queryString) {
                        if (page.wasTabsContentLoaded)
                            isPageExist = true;
                        else
                            page.wasTabsContentLoaded = true;

                    }
                });
            }

            if (!isPageExist && selectedPageUniqueName) {


                var myDiv = document.createElement('div');
                var currentTabHashCode = (selectedPageUniqueName + queryString).hashCode();

                myDiv.setAttribute("ng-include", "\'/core/partialviews/index?partialViewFileName=" + selectedPageUniqueName + "\'");
                myDiv.setAttribute("ng-show", "'" + currentTabHashCode + "'== currentTab");



                myDiv.setAttribute("id", currentTabHashCode);

                if ($("#sepehrViewContainer").length != 0) {

                    var parentScope = angular.element("#tabstripNgView").scope();
                    var newScope = parentScope.$new(false);

                    $("#sepehrViewContainer").append($compile(myDiv)(parentScope));
                }
            }

           
        }
    }
}]);
managePagesSeviceModule.addService("CurrentPagesSevice", ["viewElementService", function (viewElementService) {
    return {

        getCurrentPages: () => {
            var tabstripItemsUniqueName = [];
            return viewElementService.topMenu.insertedTabs;
        }
    }

}]);


managePagesSeviceModule.addService("clearTabContainerSevice", ['viewElementService', function (viewElementService) {
    return {
        closeAllOpenTabs: () => {
            if (viewElementService.topMenu.tabStrip) {
                viewElementService.topMenu.closeAllOpenTabStrips();
            }
        }
    }

}]); 