


var menuDirective = new SepehrModule.MainModule("menuDirectiveModule", ['viewElementServiceModule', 'coreAppInfoServiceModule']);
menuDirective.addDirective('menu', ['viewElementService', '$location', function (viewElementService, $location) {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            currentTab: "=",
            customTemplate: "@",
        },
        templateUrl: '/core/partialviews/index?partialViewFileName=Menu',
        controller: 'menuController',

        link: ($scope, $element) => {

        },


    }
}]);
menuDirective.addRunInlineAnnotatedFunction(["$route", "$rootScope", "$location", "managePagesSevice", "getViewElementInfoByUniqueNamesFirstPart", "clearTabContainerSevice", 'viewElementService', 'isAuthorizeService',
    ($route, $rootScope, $location, managePagesSevice, getViewElementInfoByUniqueNamesFirstPart, clearTabContainerSevice, viewElementService, isAuthorizeService) => {
        var currentScope = null;
        var un = $rootScope.$on('$locationChangeSuccess', function (event, requestedPath, currentpath) {
            if (isAuthorizeService.isAuthorize) {
                var splitedUrl = $location.$$path.split('/');
                var queryString = "";
                if (splitedUrl[2]) {
                    if (splitedUrl.length > 3) {
                        for (var j = 3; j < splitedUrl.length; j++) {

                            queryString += "/" + splitedUrl[j];
                        }
                    }
                    managePagesSevice.managePages(splitedUrl[2], queryString).then(() => {
                        getViewElementInfoByUniqueNamesFirstPart($location.$$path);
                    });

                }

                else {
                    clearTabContainerSevice.closeAllOpenTabs();

                }
            }
        });

    }]);
menuDirective.addController('menuController', ['$rootScope', '$scope', '$http', '$q', '$location', 'viewElementService', 'findValTextService', 'AccessibleViewElementsService', 'currentTabService',
    function ($rootScope, $scope, $http, $q, $location, viewElementService, findValTextService, AccessibleViewElementsService, currentTabService) {
        $scope.currentTab = currentTabService.currentTab;

        $scope.enterKeyPressed = false;
        $scope.isLocationGoingToChange = false;

        $scope.menuContentLoaded = false;
        $scope.menuData = null;
        $scope.AccessibleViewElements = AccessibleViewElementsService.AccessibleViewElements;

        $scope.tabStrip;
        $scope.menuItemSelectedEventArgs = null;

        $scope.location = $location;
        viewElementService.topMenu.$initialScope = $scope;

        viewElementService.eventArgs.menuItemSelectedEventArgs = null;
        viewElementService.topMenu.locationService = $location;

        viewElementService.topMenu.$rootScope = $rootScope;
        $scope.onMenuItemSelect = (ev) => {

            if ($(ev.item).attr("aria-haspopup") == "true") {
                return;
            }
            var id = $(ev.item.firstChild).find("span").attr("id");
            if (id == "#/CoreCloseAll") {
                $scope.currentTab = "";
                currentTabService.currentTab = "";
                $location.path("/menu/");
                viewElementService.topMenu.removeAllTabs();

            }
            else if (id == "#/CoreAboutUs") {
                $scope.currentTab = "CoreAboutUs";
                currentTabService.currentTab = "CoreAboutUs";
                $("#aboutUsModal").data("kendoWindow").center().open();
            }
            else {
                var path = id.split('#')[1].split('/');

                $scope.currentTab = id.split('#')[0].hashCode();
                currentTabService.currentTab = id.split('#')[0].hashCode();

                var rawMenuPath = "/menu/" + id.split('#')[0];


                viewElementService.eventArgs.menuItemSelectedEventArgs = ev;



                if (rawMenuPath != "/menu") {
                    $location.path(rawMenuPath);
                }
            }

        };

        viewElementService.GetMenuItems().then(function (menuItems) {

            if ($location.path().indexOf("menu") != -1 && $location.path() != "/menu" && $location.path() != "/menu/") {

                if (!viewElementService.eventArgs.menuItemSelectedEventArgs) {

                    viewElementService.topMenu.isComeFromAddressBar = true;


                    if (AccessibleViewElementsService.AccessibleViewElements && AccessibleViewElementsService.AccessibleViewElements.length > 0) {

                        var menuItem = findValTextService.getValText(AccessibleViewElementsService.AccessibleViewElements, $location.path());

                    }

                }

            }
            else {
                viewElementService.topMenu.insertedTabs = [];
                localStorage.removeItem("tabItems");
            }

            $scope.MenuItems = menuItems;
            var menuItemsInLocalStorage = localStorage.getItem("tabItems");

            if (menuItemsInLocalStorage) {
                var userTabs = JSON.parse(menuItemsInLocalStorage);

                if (userTabs.length > 0) {

                    var unselectedUserTabs = $.grep(userTabs, function (tab: any) { return !tab.IsLastSelected; });
                    var selectedUserTab = $.grep(userTabs, function (tab: any) { return tab.IsLastSelected; });

                    unselectedUserTabs.forEach(function (tab) {
                        tab.Url = decodeURI(tab.Url);
                        viewElementService.topMenu.appendTabToTabStrip(tab.TabName, tab.Url, "");
                        tab.wasTabsContentLoaded = false;
                        viewElementService.topMenu.insertedTabs.push(tab);
                    });


                    var menuItem = findValTextService.getValText(AccessibleViewElementsService.AccessibleViewElements, $location.path());
                    if (menuItem.url && menuItem.text) {

                        viewElementService.topMenu.menuItemSelected(viewElementService.eventArgs.menuItemSelectedEventArgs, menuItem.url, menuItem.text, "");
                        selectedUserTab[0].wasTabsContentLoaded = true
                        selectedUserTab[0].Url = decodeURI(selectedUserTab[0].Url);


                    }

                }
            }
        });


        $scope.tabStripNavigator;


        $scope.tabStripSettings = {

            select: function (e) {

                var queryString = "";

                if ($(e.item).attr('class').indexOf('k-state-hover') != -1) {

                    viewElementService.topMenu.comeFromTabClick = false;
                }

                else if (viewElementService.topMenu.isComeFromAddressBar) {

                    viewElementService.topMenu.comeFromTabClick = true;
                    viewElementService.topMenu.isComeFromAddressBar = false;
                }


                var tabId = $(e.item).find("span.k-i-close").attr("id");
                var locationParts = $location.path().split('/');
                var uniqueNameFromUrl = locationParts[2];

                if (locationParts.length > 3) {
                    for (var j = 3; j < locationParts.length; j++) {

                        uniqueNameFromUrl += "/" + locationParts[j];
                    }
                }

                if (tabId == uniqueNameFromUrl) {

                    $scope.currentTab = uniqueNameFromUrl.hashCode();
                    currentTabService.currentTab = uniqueNameFromUrl.hashCode();

                    viewElementService.topMenu.onTabItemSelected(e, $location, uniqueNameFromUrl);
                }
                else {
                    $scope.currentTab = tabId.hashCode();
                    currentTabService.currentTab = tabId.hashCode();

                    viewElementService.topMenu.onTabItemSelected(e, $location, tabId);
                }

            },
            encoded: false
        }

        $scope.$on("kendoWidgetCreated", function (event, widget) {

            if (widget.options.name === "TabStrip") {
                if (widget.element.attr("id") == "tabStripNavigator") {
                    viewElementService.topMenu.tabStrip = widget;
                }
            }
        });

    }]);

menuDirective.addService('findValTextService', [function () {

    return {
        getValText: getValText
    }

    function getValText(accessibleViewElements, path) {

        var menuItem = { url: "", text: "" };
        var selectedPath = path.split('/')[2].toLowerCase();

        for (var i = 0; i < accessibleViewElements.length; i++) {

            var val = accessibleViewElements[i];
            var title = accessibleViewElements[i].Title
            var uniqueNamesFirstPart = accessibleViewElements[i].UniqueName.split('#')[0];



            if (uniqueNamesFirstPart.toLowerCase() == selectedPath) {
                menuItem.text = title;
                menuItem.url = accessibleViewElements[i].UniqueName;
                return menuItem;
            }
        }

        return menuItem;
    }
}]);
