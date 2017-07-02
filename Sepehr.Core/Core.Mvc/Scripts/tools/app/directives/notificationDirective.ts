var notificationDirective = new SepehrModule.MainModule('notificationDirective', ['coreNotificationService']);
notificationDirective.addDirective('notification', ['notificationInfo', (notificationInfo) => {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            //title:"@",
            //message: "@",
            //mode:"@"//succes , error , info , warning
            notificationInfo:"="
        },
        controller: ["$scope", "$element", ($scope, $element) => {
            
           
        }],
        link: ($scope, $element, attrs, ctrl) => {
        
            //switch ($scope.mode) {
            //    case "succes":
            //        $scope.info = new SuccessNotification($scope.title, $scope.message);
            //        $scope.notify.show();
            //        break;
            //    case "error":
            //        $scope.info = new ErrorNotification($scope.title, $scope.message);
            //        $scope.notify.show();
            //        break;
            //    case "warning":
            //        $scope.info = new WarningNotification($scope.title, $scope.message);
            //        $scope.notify.show();
            //        break;
            //    case "info":
            //        $scope.info = new InfoNotification($scope.title, $scope.message);
            //        $scope.notify.show();
            //        break;
            //    default:
            //        $scope.info = new Notification();
            //        $scope.notify.hide();
            //        break;

            //}
            //$scope.notificationInfo = notificationInfo;
            notificationInfo.element = $scope.notificationInfo.element;
            $scope.onShow = (args) => {
                
                args.element.parent().css({
                    zIndex: 22222
                })
            }
        },          
        templateUrl: '/core/partialviews/index?partialViewFileName=Notification'
    }
}]);