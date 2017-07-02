
var changePassDirective = new SepehrModule.MainModule('menuContainerModule', []);
changePassDirective.addDirective('menuContainer', [() => {
    return {
        restrict: 'E',
        replace: true,
        //templateUrl: F:\TFSSource\Core\Sepehr.Core\Core.Mvc\Views\Shared\MainLayoutTemplates\TopMainMenu.cshtml
        templateUrl: '/core/partialviews/index?partialViewFileName=MainLayoutTemplates/TopMainMenu',
    }
}]);


  