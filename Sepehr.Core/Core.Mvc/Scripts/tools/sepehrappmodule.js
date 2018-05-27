var SepehrModule;
(function (SepehrModule) {
    SepehrModule.$http = angular.injector(["ng"]).get("$http"), SepehrModule.$q = angular.injector(["ng"]).get("$q");
    var MainModule = (function () {
        function MainModule(name, modules) {
            this.app = angular.module(name, modules);
        }
        MainModule.prototype.addController = function (name, controller) {
            this.app.controller(name, controller);
        };
        MainModule.prototype.addService = function (name, service) {
            this.app.factory(name, service);
        };
        MainModule.prototype.addProvider = function (name, provider) {
            this.app.provider(name, provider);
        };
        MainModule.prototype.addConfig = function (config) {
            return this.app.config(config);
        };
        MainModule.prototype.addDirective = function (name, directiveCallback) {
            this.app.directive(name, directiveCallback);
        };
        MainModule.prototype.addRunInitializationFunction = function (initializationFunction) {
            this.app.run(initializationFunction);
        };
        MainModule.prototype.addRunInlineAnnotatedFunction = function (inlineAnnotatedFunction) {
            this.app.run(inlineAnnotatedFunction);
        };
        MainModule.prototype.addDirectiveByFactory = function (name, factoryFunction) {
            this.app.directive(name, factoryFunction);
        };
        MainModule.prototype.addFilter = function (name, filterFactoryFunction) {
            this.app.filter(name, filterFactoryFunction);
        };
        MainModule.prototype.addFilterInlineAnnotated = function (name, annotatedFunction) {
            this.app.filter(name, annotatedFunction);
        };
        MainModule.prototype.addValue = function (key, value) {
            this.app.value(key, value);
        };
        return MainModule;
    }());
    SepehrModule.MainModule = MainModule;
})(SepehrModule || (SepehrModule = {}));
var Ambients;
(function (Ambients) {
    var LocalDictionary = (function () {
        function LocalDictionary() {
            this.key = null;
            this.value = null;
        }
        return LocalDictionary;
    }());
    Ambients.LocalDictionary = LocalDictionary;
    var Window = (function () {
        function Window() {
        }
        Window.setLocale = function (loc) {
            this.locale = loc;
        };
        Window.getLocale = function () {
            return this.locale;
        };
        Window.initializeLocale = function () {
            this.locale = [];
        };
        Window.setLocaleObjects = function (objs) {
            var localeObj = {};
            for (var i = 0; i < objs.length; i++) {
                localeObj[objs[i].key.toString()] = " " + objs[i].value.toString() + " ";
            }
            this.locale = localeObj;
        };
        Window.setALocaleValue = function (objName) {
            this.locale[objName.toString()] = this.locale[objName.toString()];
        };
        return Window;
    }());
    Window.locale = null;
    Ambients.Window = Window;
    var Globals = (function () {
        function Globals() {
        }
        Globals.getLanguageCode = function () {
            this.languageCode = window.location.pathname.split('/')[1];
            if (this.languageCode.length > 2) {
                this.languageCode = "fa";
            }
            if (this.languageCode == "" || this.languageCode == null)
                this.languageCode = "fa";
        };
        return Globals;
    }());
    Globals.languageCode = null;
    Globals.gFilterObj = [];
    Globals.timer = 0;
    Ambients.Globals = Globals;
})(Ambients || (Ambients = {}));
var CrudOperation = (function () {
    function CrudOperation() {
    }
    CrudOperation.prototype.change = function (id) {
    };
    CrudOperation.prototype.save = function () {
    };
    CrudOperation.prototype.deleteItem = function () {
    };
    return CrudOperation;
}());
var Router = (function () {
    function Router() {
    }
    Router.action = function (controllerName, actionName, params) {
    };
    return Router;
}());
(function ($) {
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
