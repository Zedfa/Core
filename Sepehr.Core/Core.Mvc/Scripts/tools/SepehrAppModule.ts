/// <reference path="../typings/angularjs/angular.d.ts" />

module SepehrModule {
    export var $http = angular.injector(["ng"]).get("$http"),
        $q = angular.injector(["ng"]).get("$q");


    export class MainModule {
        app: ng.IModule;

        constructor(name: string, modules: Array<string>) {
            this.app = angular.module(name, modules);
        }

        addController(name: string, controller: any[]): void {
            this.app.controller(name, controller);
        }

        addService(name: string, service: any[]): void {
            this.app.factory(name, service);
        }
        addProvider(name: string, provider: any[]): void {
            this.app.provider(name, provider);
        }

        addConfig(config: any[]): ng.IModule {
            return this.app.config(config);
        }

        addDirective(name: string, directiveCallback: any[]): void {
            this.app.directive(name, directiveCallback);
        }

        addRunInitializationFunction(initializationFunction: Function): void {
            this.app.run(initializationFunction);
        }
        addRunInlineAnnotatedFunction(inlineAnnotatedFunction: any[]): void {
            this.app.run(inlineAnnotatedFunction);
        }

        addDirectiveByFactory(name: string, factoryFunction: Function): void {
            this.app.directive(name, factoryFunction);
        }

        addFilter(name: string, filterFactoryFunction: Function): void {
            this.app.filter(name, filterFactoryFunction);
        }

        addFilterInlineAnnotated(name: string, annotatedFunction: any[]): void {
            this.app.filter(name, annotatedFunction);
        }

        addValue(key: string, value: any): void {
            this.app.value(key, value);
        }

    }
}

module Ambients {

    export class LocalDictionary {
        key: string = null;
        value: string = null;
    }

    export class Window {
        static locale: any = null;
        static setLocale(loc: any) {
            //this.locale = "window.locale=" + loc;
            //eval(this.locale);
            this.locale = loc;

        }
        static getLocale() {
            //return eval("window.locale");
            return this.locale;
        }
        static initializeLocale() {
            // return eval("window.locale=[]");
            this.locale = [];
        }
        static setLocaleObjects(objs: Array<LocalDictionary>) {
            var localeObj = {};
            for (var i = 0; i < objs.length; i++) {
                // eval("window.locale['" + objs[i].key + "']='" + objs[i].value + "'");
                localeObj[objs[i].key.toString()] = " " + objs[i].value.toString() + " ";
            }
            this.locale = localeObj;
        }
        static setALocaleValue(objName: string) {
            // eval("window.locale." + objName + "= window.locale['" + objName + "']" );
            this.locale[objName.toString()] = this.locale[objName.toString()];
        }
    }

    export class Globals {
        static languageCode: string = null;
        static getLanguageCode() {
            this.languageCode = window.location.pathname.split('/')[1];
            if (this.languageCode == "" || this.languageCode == null)
                this.languageCode = "fa"
        }
        static gFilterObj: any = [];
        static timer = 0;
    }

}

class CrudOperation {
    constructor() { }
    change(id): void {
    }

    save(): void {
    }

    deleteItem(): void {

    }
}

class Router {
   
    // Router.action("SystemUser", "ListAll", { lang: languageCode })
    static action(controllerName: string, actionName: string, params: Object): any {

    }
}

interface JQuery {
    limiter(num: number, element: any);
    vTicker(configObj: any);
}

declare function moment(options: any, opt2: string);


declare function Search(criteria: boolean);


(($) => {
    $.fn.extend({
        limiter: function (limit, elem) {
            $(this).on("keyup focus", function () {
                setCount(this, elem);
            });
            function setCount(src, elem) {
                var chars = src.value.length;
                if (chars > limit) {
                    src.value = src.value.substr(0, limit);
                    chars = limit;
                }
                elem.html(limit - chars);
            }
            setCount($(this)[0], elem);
        }
    });
})(jQuery);