var httpInterceptorModule = new SepehrModule.MainModule("httpInterceptorModule", ["coreNotificationService"]);
httpInterceptorModule.addConfig(["$httpProvider", function ($httpProvider) {
        function getExceptionText(expInfo) {
            var text = new Array();
            if (typeof expInfo == "string") {
                text.push(expInfo);
            }
            else if (expInfo.ModelState) {
                for (var info in expInfo.ModelState) {
                    if (info != "isHandled") {
                        text.push(expInfo.ModelState[info][0]);
                    }
                }
            }
            else {
                for (var info in expInfo) {
                    if (expInfo.Message) {
                        text.push(expInfo.Message);
                    }
                    if (expInfo.ExceptionMessage) {
                        text.push(expInfo.ExceptionMessage);
                    }
                    if (expInfo.Message) {
                        text.push(expInfo.Message);
                    }
                    if (info == "InnerException") {
                        return getExceptionText(expInfo.InnerException);
                    }
                }
            }
            return text;
        }
        $httpProvider.interceptors.push(["$q", "notificationInfo", "error", "warning", "success", "info", function ($q, notificationInfo, error, warning, success, info) {
                return {
                    "request": function (config) {
                        return config;
                    },
                    "response": function (response) {
                        if (response.headers("error")) {
                            notificationInfo.message = getExceptionText(decodeURI(escape(response.headers("error"))));
                            error(notificationInfo);
                        }
                        if (response.headers("warning")) {
                            notificationInfo.message = getExceptionText(decodeURI(escape(response.headers("warning"))));
                            warning(notificationInfo);
                        }
                        if (response.headers("success")) {
                            notificationInfo.message = getExceptionText(decodeURI(escape(response.headers("success"))));
                            success(notificationInfo);
                        }
                        if (response.headers("info")) {
                            notificationInfo.message = getExceptionText(decodeURI(escape(response.headers("info"))));
                            info(notificationInfo);
                        }
                        return response;
                    },
                    "responseError": function (rejection) {
                        if (rejection.data) {
                            notificationInfo.message = getExceptionText(rejection.data);
                        }
                        else if (rejection.statusText) {
                            notificationInfo.message = rejection.statusText;
                        }
                        else {
                            notificationInfo.message = rejection.message;
                        }
                        error(notificationInfo);
                        return $q.reject(rejection);
                    },
                    "requestError": function (rejection) {
                        return $q.reject(rejection);
                    },
                };
            }]);
    }]);
