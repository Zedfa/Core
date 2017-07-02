/// <reference path="gridsearch.ts" />
class FilterDataSource {

    public Grid: any
    public FilterItems: Array<FilterParameter>

    public ApplyFilter = (logic: string = "and"): any=> {
        

        var gridData = this.Grid;
        
        // var model =angular.element(gridData.element).scope()
        //var gridScope:any = angular.element(gridData.element).scope(),
        //    initialFilter = jQuery.extend(true,{}, gridScope.gridOptions.initialFilter);

        var dataSource = gridData.dataSource;
        var currentFilterObj = jQuery.extend(true, {}, dataSource.filter()),
            currentFilters = currentFilterObj ? jQuery.extend(true, [], currentFilterObj.filters)  : [];

       

        for (var j = 0; j < this.FilterItems.length; j++) {

            //Search filter in CurrentFilters And remove it if exist
            if (currentFilters && currentFilters.length > 0 && currentFilters.length == this.FilterItems.length) {

                for (var i = 0; i < currentFilters.length; i++) {

                    if (currentFilters[i].field == this.FilterItems[j].field) {

                        currentFilters.splice(i, 1);
                    }
                }
            }

            if (this.FilterItems[j].value != '' && this.FilterItems[j].value != null && this.FilterItems[j].value != '0' && this.FilterItems[j].value != "false") {
                currentFilters.push({
                    field: this.FilterItems[j].field,
                    operator: this.FilterItems[j].operator,
                    value: this.FilterItems[j].value,
                });
            }
        }
        var baseFilter = {
            logic: logic,
            filters: currentFilters
        };
        dataSource.options["filter"] = baseFilter;
      
        dataSource.filter(dataSource.options["filter"]);

        //gridScope.gridOptions.initialFilter = initialFilter;
    }

}

class FilterParameter {

    public value: any
    public field: string
    public operator: string
    public show: boolean
}