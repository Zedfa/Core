var changePassDirective = new SepehrModule.MainModule('menuContainerModule', []);
changePassDirective.addDirective('menuContainer', [function () {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/core/partialviews/index?partialViewFileName=MainLayoutTemplates/TopMainMenu',
        };
    }]);
