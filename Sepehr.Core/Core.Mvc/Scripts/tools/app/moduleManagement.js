var moduleManagement = (function () {
    function moduleManagement() {
    }
    Object.defineProperty(moduleManagement, "modulesFunc", {
        get: function () {
            if (!moduleManagement._modules) {
                moduleManagement._modules = new Array();
            }
            return moduleManagement._modules;
        },
        enumerable: true,
        configurable: true
    });
    moduleManagement.loadModules = function () {
        var newModules = new Array();
        moduleManagement._modules.forEach(function (func) {
            func().forEach(function (moduleName) {
                newModules.push(moduleName);
            });
        });
        return newModules;
    };
    return moduleManagement;
}());
