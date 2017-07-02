var userRoleControllerModule = new SepehrModule.MainModule("userRoleControllerModule", []);
userRoleControllerModule.addController('userRoleController', ['$scope', function ($scope) {
        $scope.onUserGridDataBound = function (args) {
            args.scope.userGrid.select(args.scope.userGrid.content.find("tr:last"));
        };
        $scope.onUserGridChange = function (args) {
            var userRoleGrid = $scope.userRoleGrid;
            var userGrid = args.userGrid;
            var selectedUserIndex = userGrid.select().index();
            var selectedUser = userGrid.dataItems()[selectedUserIndex];
            $scope.initialFilterArray = new Array();
            var filterItemParentId = new FilterParameter();
            filterItemParentId.value = selectedUser.Id;
            $scope.userId = selectedUser.Id;
            filterItemParentId.field = "UserId";
            filterItemParentId.operator = 'eq';
            $scope.initialFilterArray.push(filterItemParentId);
            var filterDataSource = new FilterDataSource();
            filterDataSource.FilterItems = $scope.initialFilterArray;
            filterDataSource.Grid = userRoleGrid;
            $scope.userRoleGrid.dataSource.transport.cache.clear();
            filterDataSource.ApplyFilter();
            $scope.filterBase = {
                logic: "and",
                filters: $scope.initialFilterArray
            };
        };
        $scope.onUserRoleGridChange = function (args) {
            var userRoleGrid = args.userRoleGrid;
            var selectedItemIndexIndex = userRoleGrid.select().index();
            var selectedUserRole = userRoleGrid.dataItems()[selectedItemIndexIndex];
            selectedUserRole.OldRoleId = selectedUserRole.RoleId;
        };
        $scope.onUserGridEdit = function (args) {
        };
        $scope.onUserRoleGridInit = function (args) {
            $scope.userRoleGrid = args["userRoleGrid"];
        };
        $scope.onUserRoleGridEdit = function (args) {
            args.model.UserId = $scope.userId;
        };
    }]);
