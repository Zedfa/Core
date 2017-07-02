var FilterDataSource = (function () {
    function FilterDataSource() {
        var _this = this;
        this.ApplyFilter = function (logic) {
            if (logic === void 0) { logic = "and"; }
            var gridData = _this.Grid;
            var dataSource = gridData.dataSource;
            var currentFilterObj = jQuery.extend(true, {}, dataSource.filter()), currentFilters = currentFilterObj ? jQuery.extend(true, [], currentFilterObj.filters) : [];
            for (var j = 0; j < _this.FilterItems.length; j++) {
                if (currentFilters && currentFilters.length > 0 && currentFilters.length == _this.FilterItems.length) {
                    for (var i = 0; i < currentFilters.length; i++) {
                        if (currentFilters[i].field == _this.FilterItems[j].field) {
                            currentFilters.splice(i, 1);
                        }
                    }
                }
                if (_this.FilterItems[j].value != '' && _this.FilterItems[j].value != null && _this.FilterItems[j].value != '0' && _this.FilterItems[j].value != "false") {
                    currentFilters.push({
                        field: _this.FilterItems[j].field,
                        operator: _this.FilterItems[j].operator,
                        value: _this.FilterItems[j].value,
                    });
                }
            }
            var baseFilter = {
                logic: logic,
                filters: currentFilters
            };
            dataSource.options["filter"] = baseFilter;
            dataSource.filter(dataSource.options["filter"]);
        };
    }
    return FilterDataSource;
}());
var FilterParameter = (function () {
    function FilterParameter() {
    }
    return FilterParameter;
}());
