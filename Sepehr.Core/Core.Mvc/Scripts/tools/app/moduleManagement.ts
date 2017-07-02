class moduleManagement {
    public static _modules: Array<iModules>;
    static get modulesFunc(): Array<iModules> {
        if (!moduleManagement._modules) {
            moduleManagement._modules = new Array<iModules>();
        }
        return moduleManagement._modules;
    }

    static loadModules(): Array<string> {
        var newModules = new Array<string>();
        moduleManagement._modules.forEach(func => {
            func().forEach(moduleName => {
                newModules.push(moduleName);
            })
        });
        return newModules;
    }
}

interface iModules {
    (): Array<string>
    //modules: Array<Function>;
}