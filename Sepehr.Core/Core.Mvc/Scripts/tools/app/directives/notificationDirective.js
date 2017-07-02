var notificationDirective = new SepehrModule.MainModule('notificationDirective', ['coreNotificationService']);
notificationDirective.addDirective('notification', ['notificationInfo', function (notificationInfo) {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                notificationInfo: "="
            },
            controller: ["$scope", "$element", function ($scope, $element) {
                }],
            link: function ($scope, $element, attrs, ctrl) {
                notificationInfo.element = $scope.notificationInfo.element;
                $scope.onShow = function (args) {
                    args.element.parent().css({
                        zIndex: 22222
                    });
                };
            },
            templateUrl: '/core/partialviews/index?partialViewFileName=Notification'
        };
    }]);
