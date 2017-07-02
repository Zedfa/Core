/// <reference path="../typings/jqueryui/jqueryui.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />


class ngSearchObj {
    searchWindowAttributes;
    windowContainer;
    winContentcontainer;
    gridSelector;
    gridData;
    afterFirstContentUrlAssigned = false;
    andOrDropDownAlreadyAssigned = false;
    localContent;
    win;
    searchableColumnsList = [];
    selectedColumnCriteria = {};
    fieldOptcont = null;
    columnCont = null;
    schemaTypes = null;
    booleanValContainer = {};
    multiValueFields = {};
    instance;
    activeRowIndex;

    seWinWidth = "900";
    seWinHeight = "250";
    scope;
    compile;
    outerDom;
    cGId = "";
    okCallback;
    cancelCallback;
    title;
    winSearchId;


    constructor(dom, scope, $compile, immAncesstorGridId, ngOkCallback, ngCancelOrCloseCallback, title, seWinWidth, seWinHeight) {
        this.scope = scope;
        this.compile = $compile;
        this.outerDom = dom;
        this.cGId = immAncesstorGridId;
        this.okCallback = ngOkCallback;
        this.cancelCallback = ngCancelOrCloseCallback;
        this.title = !title ? "جستجو" : title;
        this.seWinWidth = !seWinWidth ? this.seWinWidth : seWinWidth;
        this.seWinHeight = !seWinHeight ? this.seWinHeight : seWinHeight;

    }

    initializeSearchObj() {

        this.buildWindowContentTemplate(this.cGId);
    }

    buildWindowContentAttributes(gridId) {
        this.searchWindowAttributes = { 'contentSearchId': 'searchGrid_' + gridId, 'winContainerId': 'win_se_' + gridId };
    }

    buildWindowContentTemplate(gridId) {

        var that = this;
        that.searchableColumnsList = [];
        that.gridData = that.scope[that.cGId].dataSource;
        that.schemaTypes = that.scope[that.cGId].options.dataSource.schema.searchCustomTypes;
        $.each(that.scope[that.cGId].columns, function (key, val) {
            if (val.searchable && that.ifFieldIsNotExcluding(val)) {
                that.searchableColumnsList.push(val);
            }
        });

        this.winSearchId = "search" + gridId;


        this.scope.seOptions = this.buildSearchGrid();


        this.scope.commitSearch = function (sc) {
            // var that = ngSearchHelper.getCurrentActiveSearch(),
            var data = that.scope["grdSearch" + that.cGId].dataSource.data(),
                pager = that.outerDom.find("div[data-role=pager]"); //$("[grid-name=" + that.cGId + "] div[data-role=pager]");

            try {


                that.buildCurrentSearchList();
                pager.children("[class='search-text']").remove();

                if (data.length > 0) {
                    var container: JQuery = $("<div>", { "class": "search-text" }).text("جستجوی فعلی : ");

                    $.each(data, (index, record) => {

                        var logic = index == 0 ? "" : record.andor.andorName,
                            displayText = record.fld.columnId.type == "boolean" ? record.val.value : record.val.text,
                            conditionText = "<span class='search-text-item'> <strong > " + logic + " </strong>"
                                + record.fld.columnName + " "
                                + record.opt.operatorName + " "
                                + "'" + displayText + "'" + "</span>";
                        container.append(conditionText);
                    });

                    if (pager) {
                        pager.append(container);
                    }


                }
            }
            catch (ex) {
                console.log(ex);
            }
        }

        this.scope.cancelSearch = function (sc) {

            that.closeSearchWin();
        }

        this.scope.searchVisible = false;

        this.scope.addSearchRuleRow = function () {

            that.addCriteriaRecord();
        }

        var searchAddRuleBtn = '<button k-button ng-click="addSearchRuleRow()" > <span class="k-icon k-i-plus" ></span></button>';

        this.scope.searchToolbarTemplate = searchAddRuleBtn;

        this.scope.searchOpen = function (arg) {
            //var that = this;
            //ngSearchHelper.setActiveGridSearch("grdSearch" + that.cGId);
        }

        this.scope.searchClose = function (arg) {
            that.scope["search" + that.cGId].refresh();
        }

        var wnGrdTemplate =
            $('<div kendo-window="search' + gridId + '" k-modal="true"  k-width="' + this.seWinWidth + '"k-heigth="' + this.seWinHeight + '" title="' + this.title + '" k-visible="searchVisible" on-open="searchOpen($event)"   on-close="searchClose($event)"  >' +

                '<div kendo-grid="grdSearch' + this.cGId + '" k-width="' + this.seWinWidth + '" k-height="' + this.seWinHeight + '" options="seOptions" k-toolbar="[{ template: searchToolbarTemplate }]" > ' +

                '</div>' +


                '<div class="k-edit-form-container">' +

                '</div>' +
                '</div>');

        var buttonActions = $('<div class="k-edit-buttons k-state-default" >' +
            '<button class="k-button k-button-icontext k-primary  mainbtn k-grid-update" ng-click="commitSearch(this)" >' +
            'تایید</button> ' + '<button class="k-button k-button-icontext k-grid-cancel" ng-click="cancelSearch(this)" >' +
            'انصراف</button> ' + '</div>');
        wnGrdTemplate.find("div.k-edit-form-container").append(buttonActions);


        this.outerDom.append(wnGrdTemplate);

        this.compile(wnGrdTemplate)(this.scope);


    }

    getCurrentSearchGrid() {
        return this.scope["grdSearch" + this.cGId];
    }
    openSearchWindow() {
        this.scope.searchVisible = true;
        this.scope["search" + this.cGId].title('جستجو');
        this.scope["search" + this.cGId].center().open();
    }
    getSchemaInfoByField(field): any {
        var that = this,
            schemaInfo = null,
            loc = "";
        $.each(that.schemaTypes, (schemaField, info) => {
            //if (schemaField == field &&
            //    ((info.custType.toLowerCase() == "lookup") || //info.lookupInfo.bindingName == field) ||
            //        (info.custType.toLowerCase() == "dropdown"))) {//info.dropdownInfo.propertyNameForBinding == field)) {
            if ((info.custType.toLowerCase() == "lookup" && info.lookupInfo.bindingName == field) ||
                (info.custType.toLowerCase() == "dropdown" && info.dropdownInfo.propertyNameForBinding == field)) {

                schemaInfo = info,
                    loc = schemaField;
                return false;
            }


        });
        return {
            info: schemaInfo,
            position: loc
        };
    }
    findColumnAccordingName(field) {
        var columns = this.scope[this.scope.gridName].columns,
            result = {};
        $.each(columns, (i, col) => {
            if (col.field == field) {
                result = col;
                return false;
            }
        });
        return result;
    }
    getValueModelByType(type, val) {

        var valueInfo = {
            value: "",
            text: ""
        };

        switch (type.toLocaleLowerCase()) {
            case "date":
            case "persiandate":
                valueInfo.value = val.split(',')[0],
                    valueInfo.text = val.split(',')[0];
                break;
            case "currency":
                var realValue = typeof (val) == "number" ? val : val.replace(/,/g, '');
                valueInfo.text = val,
                    valueInfo.value = realValue;
                break;
            case "dropdown":
            case "lookup":
                valueInfo.value = typeof (val) == "string" ? val.split(",")[0] : val;
                break;
            default:
                valueInfo.text = val,
                    valueInfo.value = val;
                break;
        }
        return valueInfo;
    }

    convertFiltersToData(data, filterCollection, logic, initfilter) {

        var self = this,
            rowLogic = data.length >= 1 ? logic : "";

        if (filterCollection.filters) {
            $.each(filterCollection.filters, (index, filter) => {
                if (filter.filters) {
                    self.convertFiltersToData(data, filter, rowLogic, initfilter);
                }
                else if (!isEqual(initfilter, filter) || (Object.hasOwnProperty(filter.show) && filter.show)) //(!isEqual(initfilter, filter))
                    data.push(self.createModelAccordingFilter(filter, filterCollection.logic));
            });
        }

        else if (!isEqual(initfilter, filterCollection) || (Object.hasOwnProperty(filterCollection.show) && filterCollection.show)) { //(!isEqual(initfilter, filterCollection)) {
            data.push(self.createModelAccordingFilter(filterCollection, rowLogic));
        }



    }

    createModelAccordingFilter(filter, logic) {

        var that = this,
            fieldInfo = null, //{  columnId: { type: "number", LId: "userId", TId: "userId" },  columnName: "شماره کاربر"},
            andOrInfo = that.getLogicModel(logic),
            columns = this.scope[this.scope.gridName].columns,
            operatorInfo = null,
            valueInfo;

        var navigateInfo = that.getSchemaInfoByField(filter.field),
            navigationColumn = navigateInfo.info,
            colName = navigateInfo.position;

        if (navigationColumn) {

            switch (navigationColumn.custType.toLowerCase()) {
                case "lookup":

                    fieldInfo = { columnId: { type: "lookup", LId: colName, TId: colName }, columnName: navigationColumn.lookupInfo.title };

                    break;
                case "dropdown":
                    fieldInfo = { columnId: { type: "dropdown", LId: colName, TId: colName }, columnName: navigationColumn.dropdownInfo.DisplayName };

                    break;
            };

            valueInfo = that.getValueModelByType(navigationColumn.custType, filter.value);
        }
        else {
            $.each(columns, (i, col) => {


                fieldInfo = that.getColumnObj(col);

                if (col.field == filter.field) {


                    valueInfo = that.getValueModelByType(fieldInfo.columnId.type, filter.value);

                    return false;
                }
            });
        }

        $.each(that.getOperatorsByType(fieldInfo.columnId.type), (i, opt) => {
            if (opt.operatorId == filter.operator) {
                operatorInfo = opt;
                return false;
            }
        });

        return {
            fld: fieldInfo,
            opt: operatorInfo,
            andor: andOrInfo,
            val: valueInfo
        }
    }
    buildSearchGrid() {

        var that = this,
            columnObj = that.getColumnObj(that.searchableColumnsList[0]),
            dataArray = [],
            mainGridFilters = that.scope[that.scope.gridName].dataSource.filter(),
            currentData = []; //that.scope["grdSearch" + that.cGId] ? that.scope["grdSearch" + that.cGId].dataSource.data() : [];


        //original grid has filter
        if (mainGridFilters) {

            $.each(mainGridFilters.filters, (i, filter) => {
                if (that.scope.initialFilter && that.scope.initialFilter.filters.length > 0) {
                    $.each(this.scope.initialFilter.filters, (j, initFilter) => {

                        that.convertFiltersToData(currentData, filter, mainGridFilters.logic, initFilter);

                    });
                }
                else {
                    // var logic = i == 0 ? "" : mainGridFilters.logic;
                    that.convertFiltersToData(currentData, filter, mainGridFilters.logic, that.scope.initialFilter);
                }
            });

        }



        var dataSource = {
            data: currentData.length > 0 ? currentData : [{
                fld: columnObj,
                opt: that.getFirstItemOperatorBasedOnColType(columnObj.columnId.type),
                andor: { andorId: "", andorName: "" },
                val: {
                    text: "",
                    value: ""
                }
            }]
        };



        var seConfig = {
            columns: [
                { field: "andor", title: "ترکیب شرط ها", editor: that.andorDropDownEditor, template: "#=andor.andorName#", width: 85 },
                { field: "fld", title: "ستون", editor: that.columnDropDownEditor, template: "#=fld.columnName#", width: 130 },
                { field: "opt", title: "عملگر", editor: that.operatorDropDownEditor, template: "#=opt.operatorName#", width: 150 },
                { field: "val", title: "مقدار", editor: that.valComplexInputsEditor, template: that.createValueTemplate, width: 250 },
                { command: { text: "حذف ", click: that.removeCurrentRow }, width: 110 }],

            editable: { confirmation: false },
            autoBind: true,
            dataBound: function (e) {
                e.sender.tbody.find("tr > td:nth-child(2)").trigger('click');

                e.sender.tbody.find("tr > td:last-child > a.k-button").css('width', '100');
                e.sender.content.css("height", Number(that.seWinHeight) - 70);
            },
            dataSource: dataSource.data,
            edit: function (e) {
            },
            change: function (e) {
            },
            save: function (e) {
            },
            remove: function (e) {

            }
        }
        return seConfig;
    }

    selectDeselectCheckBox(chkItem, trueText, falseText) {
        if ($(this).prop('checked')) {
            $(this).text(trueText);
        }
        else {
            $(this).text(falseText);
        }
    }

    getLogicModel(logic: string): Object {
        var result = null,
            self = this,
            logicCollection = self.getWholeLogicModel();
        if (logic) {
            $.each(logicCollection, (index, item) => {
                if (item.andorId == logic) {
                    result = jQuery.extend(false, {}, item);
                    return false;
                }
            });

        }
        else {
            result = { andorId: "", andorName: "" };
        }
        return result;

    }

    getWholeLogicModel(): Array<any> {
        return [{ andorId: "and", andorName: "و" }, { andorId: "or", andorName: "یا" }];
    }

    addCriteriaRecord() {

        var that = this,
            dataSource = this.scope["grdSearch" + that.cGId].dataSource,
            columnObj = that.getColumnObj(that.searchableColumnsList[0]),
            logic = dataSource.data().length > 0 ? that.getWholeLogicModel()[0] : { andorId: "", andorName: "" },
            newItem = kendo.observable({
                fld: columnObj,
                opt: that.getFirstItemOperatorBasedOnColType(columnObj.columnId.type),
                andor: logic,
                val: {
                    text: "",
                    value: ""
                }
            });
        dataSource.add(newItem);
    }

    getDataFields() {
        var that = this;
        var columns = that.searchableColumnsList;
        var fieldData = [];
        for (var i = 0; i < columns.length; i++) {
            var colObj = that.getColumnObj(columns[i]);
            if (colObj) {
                fieldData.push(colObj);
            }
        }
        return fieldData;
    }

    getColumnObj(column) {

        var that = this;
        var fields = that.scope[that.cGId].options.dataSource.schema.model.fields;
        var retcol;

        $.each(fields, function (fieldName, fieldType) {

            if (column.field.toLowerCase() === fieldName.toLowerCase()) {

                retcol = {
                    columnId: { type: that.getFieldSpecificType(column, fieldType, that.schemaTypes), LId: that.getTrueFieldName(fieldName), TId: fieldName },
                    columnName: column.title,
                    // isCurrency: column.isCurrency
                };
                return false;
            }
        });

        return retcol;
    }

    getTrueFieldName(key) {
        var that = this;
        if (that.schemaTypes[key]) { if (that.schemaTypes[key].mdlPropName) return that.schemaTypes[key].mdlPropName; }
        return key;
    }

    getFieldSpecificType(column, val, sTypes) {

        if (column.isCurrency) {
            return "currency";
        }

        var colType = val.type;

        for (var field in sTypes) {

            if (field.toLocaleLowerCase() == column.field.toLocaleLowerCase()) {
                colType = sTypes[field].custType;

            }

        }

        return colType;
    }

    removeCurrentRow(event) {

        var that = ngSearchHelper.getActiveGridSearch(),//ngSearchHelper.getGridSearchInstanceById($(event.delegateTarget).attr("kendo-grid")),
            searchGrid = that.scope[$(event.delegateTarget).attr("kendo-grid")],
            currentRowUid = $(event.target).closest("tr").attr("data-uid");
        searchGrid.dataSource.remove(searchGrid.dataSource.getByUid(currentRowUid));
        event.preventDefault();
    }
    columnDropDownEditor(cont, options) {

        var that = ngSearchHelper.getActiveGridSearch(); //ngSearchHelper.getGridSearchInstanceById(cont.closest("[kendo-grid]").attr("kendo-grid")); 
        that.columnCont = cont;
        var input = $('<input id="column" data-text-field="columnName" data-value-field="columnId.LId" data-bind="value:' + options.field + '"/>');
        input.appendTo(cont);
        input.kendoDropDownList({
            autoBind: true,
            dataSource: { data: that.getDataFields(), events: { requestEnd: (e) => { } } },
            //animation: {
            //    close: {
            //        effects: "fadeOut zoom:out",
            //        duration: 300
            //    },
            //    open: {
            //        effects: "fadeIn zoom:in",
            //        duration: 300
            //    }
            //},
            dataBound: (e) => {

                //var parentRowId = that.columnCont.parent("tr").attr("data-uid");
                //if (!that.selectedColumnCriteria[parentRowId]) {
                //    var val = e.sender.dataSource.options.data[0];
                //    that.selectedColumnCriteria[parentRowId] = val;
                //   that.columnCont.parent("tr").find("td:nth-child(2)").focus();
                //}

            },
            select: (e) => {

                $.each(e.sender.dataSource.options.data, (key, val) => {
                    if (key == $(e.item).index()) {
                        var parentRowId = cont.parent("tr").attr("data-uid");
                        that.selectedColumnCriteria[parentRowId] = val;
                        that.columnCont.parent("tr").find("td:nth-child(3)").focus();
                        that.columnCont.parent("tr").find("td:nth-child(3)").text();
                        return;
                    }
                });
            },
            change: (e) => {

                var model = that.scope["grdSearch" + that.cGId].dataSource.getByUid(that.columnCont.parent("tr").attr("data-uid"));

                that.columnCont.parent("tr").find("td:nth-child(3)").text(that.getFirstItemOperatorBasedOnColType(model.fld.columnId.type).operatorName);
                // that.columnCont.parent("tr").find("td:nth-child(4)").text("");

                model.val.value = "",
                    model.val.text = "",
                    model.opt.operatorId = that.getFirstItemOperatorBasedOnColType(model.fld.columnId.type).operatorId,
                    model.opt.operatorName = that.getFirstItemOperatorBasedOnColType(model.fld.columnId.type).operatorName;
                that.columnCont.parent("tr").find("td:nth-child(4)").click();

            }
        });
    }

    getFirstItemOperatorBasedOnColType(colType) {
        var that = this;//ngSearchHelper.getCurrentActiveSearch();
        var specificFieldOperators = null;
        switch (colType.toLowerCase()) {
            case 'number':
            case 'digit':
            case 'currency':
                specificFieldOperators = that.getIntRelatedOperators()[0];
                break;
            case 'bool':
            case 'boolean':
                specificFieldOperators = that.getBooleanRelatedOperators()[0];
                break;
            case 'string':
            case 'navigation':
                specificFieldOperators = that.getStringRelatedOperators()[0];
                break;
            case 'lookup':
            case 'dropdown':
                specificFieldOperators = that.getLookupRelatedOperators()[0];
                break;
            case 'persiandate':
            case 'date':
            case 'datetime':
                specificFieldOperators = that.getDateRelatedOperators()[0];
                break;
            case 'time':
                specificFieldOperators = that.getTimeRelatedOperators()[0];
                break;
            default:
                specificFieldOperators = that.getTheWholeRules()[0];
        }
        return specificFieldOperators;

    }

    getOperatorsByType(fldType) {
        var that = this;//ngSearchHelper.getCurrentActiveSearch();
        var specificFieldOperators = null;
        switch (fldType.toLowerCase()) {
            case 'number':
            case 'digit':
            case 'currency':
                specificFieldOperators = that.getIntRelatedOperators();
                break;
            case 'enum':
                break;
            case 'boolean':
            case 'bool':
                specificFieldOperators = that.getBooleanRelatedOperators();
                break;
            case 'string':
            case 'navigation':
                specificFieldOperators = that.getStringRelatedOperators();
                break;
            case 'lookup':
            case 'dropdown':
                specificFieldOperators = that.getLookupRelatedOperators();
                break;
            case 'date':
            case 'persiandate':
            case 'datetime':
                specificFieldOperators = that.getDateTimeRelatedOperators();
                break;
            case 'time':
                specificFieldOperators = that.getTimeRelatedOperators();
                break;
            default:
                specificFieldOperators = that.getTheWholeRules();
        }
        return specificFieldOperators;
    }

    getBooleanFirstItem(colItem) {
        var col: any = _.find(this.schemaTypes, (col_item: any) => {
            return this.schemaTypes[colItem.field] && col_item.custType.toLowerCase() == 'boolean';
        });
        var boolObj;
        if (col) {
            boolObj = { boolVal: col.falseEqui, boolText: col.falseEqui };
        }
        return boolObj;
    }

    operatorDropDownEditor(cont, options) {
        var that = ngSearchHelper.getActiveGridSearch(), //ngSearchHelper.getGridSearchInstanceById(cont.parents("[kendo-grid]:first").attr("kendo-grid"));
            fieldOptcont = cont,
            parentRowId = cont.parent("tr").attr("data-uid"),
            fld = options.model.fld;


        $('<input required data-text-field="operatorName" data-value-field="operatorId" data-bind="value:' + options.field + '"/>')
            .appendTo(cont)
            .kendoDropDownList({
                autoBind: false,
                dataSource: that.getOperatorsByType(options.model.fld.columnId.type),// that.getOperatorsByType(that.scope[that.cGId].options.dataSource.schema.model.fields[options.model.fld.columnId.LId].type), 
                dataBound: (e) => {
                },
                select: (e) => {
                }
            });


    }



    getIntRelatedOperators() {
        return [{ operatorName: "برابر با", operatorId: "eq" },
        { operatorId: "neq", operatorName: "رقمی غیر از" },
        { operatorName: "بزرگتر از", operatorId: "gt" },
        { operatorName: "کوچکتر از ", operatorId: "lt" }];
    }

    getBooleanRelatedOperators() {
        return [{ operatorName: "برابر با", operatorId: "eq" },
        { operatorId: "neq", operatorName: "نا برابر با" }];
    }

    getStringRelatedOperators() {
        return [
            { operatorName: "شامل عبارت", operatorId: "contains" },
            { operatorName: "برابر با", operatorId: "eq" },
            { operatorName: "شامل نشود عبارت", operatorId: "doesnotcontain" },
            { operatorName: "(به ترتیب الفبا)بعد از عبارت", operatorId: "gte" },
            { operatorName: "(به ترتیب الفبا)قبل از عبارت", operatorId: "lte" },
        ];
    }

    getLookupRelatedOperators() {
        return [{ operatorName: "برابر با", operatorId: "eq" }];
    }

    getDateRelatedOperators() {
        return [{ operatorName: "برابر با", operatorId: "eq" },
        { operatorId: "neq", operatorName: "غیر از تاریخ" },
        { operatorName: "از تاریخ", operatorId: "gt" },
        { operatorName: "تا تاریخ", operatorId: "lt" }];
    }

    getDateTimeRelatedOperators() {
        return [{ operatorName: "برابر با", operatorId: "eq" },
        { operatorName: "نا برابر با", operatorId: "neq" },
        { operatorName: "از تاریخ", operatorId: "gt" },
        { operatorName: "تا تاریخ", operatorId: "lt" }]

    }

    getTimeRelatedOperators() {
        return [{ operatorName: "برابر با", operatorId: "eq" },
        { operatorId: "neq", operatorName: "غیر از زمان" },
        { operatorName: "بعد از زمان", operatorId: "gt" },
        { operatorName: "قبل از زمان", operatorId: "lt" }];
    }



    getTheWholeRules() {
        return [{ operatorName: "برابر با", operatorId: "eq" },
        { operatorName: "رقمی غیر از", operatorId: "neq", },
        { operatorName: "شامل رقم(رقم ها)", operatorId: "contains" },
        { operatorName: "بزرگتر از", operatorId: "gt" },
        { operatorName: "کوچکتر از ", operatorId: "lt" },
        { operatorName: "عبارتی غیر از", operatorId: "neq" },
        { operatorName: "شامل عبارت", operatorId: "contains" },
        { operatorName: "شامل نشود عبارت", operatorId: "doesnotcontain" },
        { operatorName: "(به ترتیب الفبا)بعد از عبارت", operatorId: "gte" },
        { operatorName: "(به ترتیب الفبا)قبل از عبارت", operatorId: "lte" },
        //{ operatorName: "برابر با تاریخ ", operatorId: "eq" },
        { operatorName: "غیر از تاریخ", operatorId: "neq", },
        { operatorName: "از تاریخ", operatorId: "gte" },
        { operatorName: "تا تاریخ", operatorId: "lte" },
        //{ operatorName: "برابر با زمان", operatorId: "eq" },
        { operatorName: "اعمال", operatorId: "eq" },
        { operatorName: "عدم اعمال", operatorId: "neq" },
        { operatorName: "غیر از زمان", operatorId: "neq" },
        { operatorName: "بعد از زمان", operatorId: "gte" },
        { operatorName: "قبل از زمان", operatorId: "lte" }

        ]
    }

    andorDropDownEditor(container, options) {

        if (!container.parent().is("tr:first-child")) {
            var that = ngSearchHelper.getActiveGridSearch();
            $('<input required data-text-field="andorName" data-value-field="andorId" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    autoBind: false,
                    dataSource: that.getWholeLogicModel()
                });
        }
    }

    valComplexInputsEditor(container, options) {

        var that = ngSearchHelper.getActiveGridSearch();//ngSearchHelper.getGridSearchInstanceById(container.closest("[kendo-grid]").attr("kendo-grid"));
        that.makeInputElement(container, options);
    }
    createValueTemplate(dataItem) {

        var result: any,
            that = ngSearchHelper.getActiveGridSearch();

        dataItem.val.value = that.getValueModelByType(dataItem.fld.columnId.type, dataItem.val.value).value,
            dataItem.val.text = that.getValueModelByType(dataItem.fld.columnId.type, dataItem.val.value).text;

        switch (dataItem.fld.columnId.type.toLowerCase()) {

            case "lookup":

                result = that.createLookupTemplate(that.getLookupPropertyValue(dataItem.fld.columnId.LId), dataItem);
                break;
            case "dropdown":
            
                result = that.createDropDownTemplate(that.getDropDownPropertyValue(dataItem.fld.columnId.LId), dataItem);
                break;
            case "currency":
                result = that.createCurrencyTemplate(dataItem);
                break;
            case "boolean":
            case "bool":
                dataItem.set("val.value", dataItem.val.value == "" ? false : dataItem.val.value);
                var element = '<input type="checkbox" data-type="boolean" ng-model="dataItem.val.value" data-bind="text:dataItem.val.value" />';
                kendo.bind(element, dataItem);
                result = element;

                break;
            default:
                result = "<span>#=data.val.value#</span>";
                break;
        }

        return kendo.template(result)(dataItem);
    }

    createLookupTemplate(info, model) {

        var currentGrid = ngSearchHelper.getActiveGridSearch();

        
           
        model.lookupDblClick = (args) => {
           
            ngSearchHelper.setActiveGridSearch(currentGrid)

        };
        var lookup = "<cust-lookup lookup-id='" + info.lookupName + "_" + model.uid.split('-')[0] + "'"
            + " title=\"'" + info.title + "'\""
            + " value-name=dataItem.val.value"
            + " display-name=dataItem.val.text"
            + " lkp-value-name='" + info.valueName + "'"
            + " lkp-display-name='" + info.displayName + "'"
            //+ " selected-item=val"
            + " view-model-name='" + info.viewModelName + "'"
            + " lkp-prop-name='" + info.viewInfoName + "'"
            + " lkp-dbl-click='dataItem.lookupDblClick(args)'"
            //+ " lkp-init=onInitLookup(args)"
            + " win-width=900"
            + " win-height=500>"
            + " </cust-lookup>";
        kendo.bind(lookup, model);


        return lookup;
    }

    createDropDownTemplate(info, model) {
  
        var dropDown = "<drop-down-list id=search_" + info.DisplayName + "_" + model.uid
            + " display-name=" + info.displayName
            + " value-name=" + info.valueName
            + " db-category-name=" + info.dbCategoryName
            + " url=" + info.url
            + " property-id=dataItem.val.value"
            + " property-name=dataItem.val.text"
            // + " custom-change=onDropDownChange(args)"
            // + " on-data-bound=onDropDownDataBound(args,scope)"
            + " ></drop-down-list>"
        kendo.bind(dropDown, model);

        return dropDown;
    }

    createCurrencyTemplate(model) {
        var numericTextBox = '<input type="text" data-value-update="keyup" data-bind="value: dataItem.val.value, text: dataItem.val.text" price-format price-value="dataItem.val.value"/>';
        kendo.bind(numericTextBox, model);
        return numericTextBox;
    }

    setValueAccordingByType(record) {

        var typeName = record.fld.columnId.type.toLowerCase();

        if (record.keys != undefined || record.keys != null) {
            //TODO:Check whether the value is multiple or singular.
            record.val = record.keys;
        }

        if (typeName == "datetime" || typeName == "date") {
            record.val.value += ",dt";
        }
        else if (typeName == "persiandate") {
            record.val.value += ",pdt";
        }

        else if (typeName === "lookup") {
            var lookupObj = this.getLookupPropertyValue(record.fld.columnId.LId);

            record.val.value += ",lkp:" + lookupObj.bindingName;
        }
        else if (typeName === "dropdown") {

            var dropdownInfo = this.getDropDownPropertyValue(record.fld.columnId.LId);

            record.val.value += ",ddl:" + dropdownInfo.propertyNameForBinding;
        }
        else if (typeName === "currency") {
            record.val.value = record.val.value;
        }
        else if (typeName === "boolean" || typeName === "bool") {
            record.val.text = record.val.value;
        }

        return record;
    }

    createFiltersTree(dataItem, logic, allFilters) {
        //no items in filters
        if (allFilters.filters.length == 0 && !allFilters.logic) {
            //allFilters.logic = logic,
            allFilters.filters.push(dataItem);
        }
        else {
            // 1 item is in the list and has no sub filters
            if (allFilters.filters.length == 1 && !allFilters.filters[0].filters) {
                allFilters.filters.push(dataItem);
                if (logic)
                    allFilters.logic = logic
            }
            // item have multiple filters 
            else {
                var subFilter = { filters: [allFilters, dataItem], logic: logic };


                allFilters.filters = JSON.parse(JSON.stringify(subFilter.filters)),
                    allFilters.logic = JSON.parse(JSON.stringify(subFilter.logic));
            }

        }
    }

    extractFilterObjectFromDataSource() {

        var alreadyInsertedCond = [];
        var that = this,
            data = that.scope["grdSearch" + that.cGId].dataSource.data(),
            copiedData = JSON.parse(JSON.stringify(data));
        //Doing some manipulation on data which are going to be put in filterObject.
        $.each(copiedData, (key, item) => {

            alreadyInsertedCond.push(that.setValueAccordingByType(item));

        });

        var baseFilter = jQuery.extend(true, {}, this.scope.initialFilter),
            mainFilters = { logic: "", filters: [] },
            filtersGroup = null;

        if (this.scope.initialFilter && this.scope.initialFilter.filters.length > 0) {
            mainFilters.logic = baseFilter.logic;
            mainFilters.filters = baseFilter.filters;

        }


        $.each(alreadyInsertedCond, (index, record) => {
           
            var filterItem = that.makeFilterItem(record);
            that.createFiltersTree(filterItem, record.andor.andorId, mainFilters);

        });


      
        return mainFilters;
    }

    makeFilterItem(insertingCond) {

        if (insertingCond.val == undefined || insertingCond.val === '') return null;
        else {
            var condition = insertingCond.val,
                filterItem = { field: insertingCond.fld.columnId.LId, operator: insertingCond.opt.operatorId, value: condition.value ? condition.value : condition.text };
            return filterItem;
        }
    }



    getNavigationPropertyValue(fieldVal) {
        var navProp = this.schemaTypes[fieldVal].navProp;
        return navProp;
    }

    getLookupPropertyValue(fieldVal) {
        var lookupProp = this.schemaTypes[fieldVal].lookupInfo;
        return lookupProp;
    }
    getDropDownPropertyValue(fieldVal) {
        var dropdownInfo = this.schemaTypes[fieldVal].dropdownInfo;
        return dropdownInfo;
    }
    removeAnyPreviouslyInsertedItems(con) {
        con.find('span').remove();
        con.find('input').remove();
    }

    //makeBooleanCheckBoxStr(cont, columnCriteria, options) {
    //    var that = this;
    //    if (this.schemaTypes[columnCriteria.columnId.TId]) {
    //        var columnItem = that.schemaTypes[columnCriteria.columnId.TId];
    //        if (columnItem) {
    //            $('<input type="text" id="bool_val"  data-text-field="text" data-value-field="tag" data-bind="value:' + options.field + '"/>')
    //                .appendTo(cont)
    //                .kendoDropDownList({
    //                    autoBind: false,
    //                    dataSource: [{ tag: false, text: columnItem.falseEqui }, { tag: true, text: columnItem.trueEqui }],
    //                    dataBound: (e) => {
    //                    },
    //                    select: (e) => {
    //                        that.gridSelector.setDataSourceItemVal(e.item.text(), e.sender.dataSource.data[e.item.index()].tag);
    //                    }
    //                });
    //        }
    //    }
    //}

    makeEnumDdl(cont, columnCriteria, options) {
        var that = this;//ngSearchHelper.getCurrentActiveSearch();
        if (that.schemaTypes[columnCriteria.columnId.TId]) {
            var columnItem = that.schemaTypes[columnCriteria.columnId.TId];
            if (columnItem) {
                $('<input type="text" id="enum_ddl"  data-text-field="text" data-value-field="tag" data-bind="value:' + options.field + '"/>')
                    .appendTo(cont)
                    .kendoDropDownList({
                        autoBind: false,
                        dataSource: that.getEnumDataSourceEquivalent(columnItem),
                        dataBound: function (e) {
                        },
                        select: function (e) {
                            that.gridSelector.setDataSourceItemVal(e.item.text(), e.sender.dataSource.data[e.item.index()].tag);
                        }
                    });
            }
        }
    }

    getEnumDataSourceEquivalent(colItem) {
        var eDic = colItem.enumDic;
        var enumDataSource = [];
        if (eDic) {
            $.each(eDic, function (key, val) {
                enumDataSource.push({ tag: key, text: val });
            });
            return enumDataSource;
        }
    }

    makeInputElement(cont, options) {

        var that = ngSearchHelper.getActiveGridSearch(); //ngSearchHelper.getGridSearchInstanceById(cont.closest("[kendo-grid]").attr("kendo-grid"));
        //  var parentRowId = cont.parent("tr").attr("data-uid");
        //    if (that.selectedColumnCriteria[parentRowId]) {

        var columnCriteria = options.model.fld, //that.scope["grdSearch" + that.cGId].dataSource.getByUid(parentRowId).fld,
            columnType = columnCriteria.columnId.type;
        //that.removeAnyPreviouslyInsertedItems(cont);

        switch (columnType.toLowerCase()) {
            case 'number':
            case 'digit':
                $('<input type="number" data-role="numerictextbox" data-spinners="false" class="k-input" data-bind="value:val.value"/>').appendTo(cont);
                break;
            case 'currency':
                $('<input type="text" data-value-update="keyup" data-bind="value:val.value, text: val.text" price-format price-value="val.value"/>').appendTo(cont);//    //ng-value="val.text|currency:\' \':0"
                break;
            case 'bool':
            case 'boolean':

                $('<input type="checkbox"  data-type="boolean" value= "true" data-bind="checked: val.value, text:val.value" ng-model="dataItem.val.value" />').appendTo(cont);


                break;
            case 'enum':
                that.makeEnumDdl(cont, columnCriteria, options);
                break;
            case 'string':
                //case 'navigation':
                $('<input type="text"  class="k-input k-textbox"  data-bind="value:val.value"/>').appendTo(cont);
                break;
            case 'dropdown':
                var uid = $(cont).parent("tr").attr("data-uid"),
                    dropdowInfo = that.getDropDownPropertyValue(columnCriteria.columnId.TId);

                this.scope.onDropDownChange = (args) => {

                    setColumnValue(args);

                };
                this.scope.onDropDownDataBound = (args, scope) => {

                    setColumnValue(args);
                }

                var setColumnValue = (widget) => {

                    $(cont).parents('[kendo-grid]').data("kendoGrid").dataSource.getByUid(uid).val.value = widget.sender.value();
                    $(cont).parents('[kendo-grid]').data("kendoGrid").dataSource.getByUid(uid).val.text = widget.sender.text();
                }
             
                var dropDownListElement = "<drop-down-list id=search_" + columnCriteria.columnId.TId
                    + " display-name=" + dropdowInfo.displayName
                    + " value-name=" + dropdowInfo.valueName
                    + " db-category-name=" + dropdowInfo.dbCategoryName
                    + " url=" + dropdowInfo.url
                    + " property-id=val.value"
                    + " property-name=val.text"
                    + " custom-change=onDropDownChange(args)"
                    + " on-data-bound=onDropDownDataBound(args,scope)"
                    + " ></drop-down-list>";

                cont.append(dropDownListElement);
                // this.compile(dropDownListElement)(this.scope);

                break;
            case 'lookup':

                var lookup = that.getLookupPropertyValue(columnCriteria.columnId.TId),
                    lkpIndx = cont.parent('tr').index(),
                    currentInstance = ngSearchHelper.getActiveGridSearch();
                
                this.scope.lookupDblClick = (args, parentRowId) => {
                    currentInstance.scope["grdSearch" + that.cGId].dataSource.getByUid(parentRowId).val.value = args.scope.valueName,
                        currentInstance.scope["grdSearch" + that.cGId].dataSource.getByUid(parentRowId).val.text = args.scope.displayName;
                    ngSearchHelper.setActiveGridSearch(currentInstance)
                    //var currentGrid = ngSearchHelper.getActiveGridSearch().scope["grdSearch" + that.cGId],
                    //    parentRowId = $(cont).parent("tr").attr("data-uid");
                   
                    // $(cont).text(args.scope.displayName);
                    

                };

                var lkp = this.compile("<cust-lookup lookup-id=" + lookup.lookupName + "_" + lkpIndx
                    + " title=\"'" + lookup.title + "'\""
                    + " value-name= dataItem.val.value"
                    + " display-name = dataItem.val.text"
                    + " lkp-value-name='" + lookup.valueName + "'"
                    + " lkp-display-name='" + lookup.displayName + "'"
                    //+ " selected-item=val"
                    + " view-model-name='" + lookup.viewModelName + "'"
                    + " lkp-prop-name='" + lookup.viewInfoName + "'"
                    + " lkp-dbl-click=lookupDblClick(args,'" + cont.parent('tr').data().uid + "')"
                    //+ " lkp-init=onInitLookup(args)"
                    + " win-width=900"
                    + " win-height=500>"
                    + " < /cust-lookup>")(this.scope);

                lkp.appendTo(cont);

                break;
            case 'persiandate':
            case 'date':
                var inputHtml = '<input  type="text" class="k-input k-textbox"  id="dateTimeInput" data-bind="value:val.value"/>';
                cont.append(inputHtml);
                that.setDateTimePicker(cont, inputHtml);
                break;
            //case 'datetime':

            //    break;
            case 'time':
                break;
            default:
                $('<input type="text" class="k-input k-textbox" data-bind="value:val.value"/>').appendTo(cont);
        }
        //}
    }


    setDateTimePicker(container, inputHtml): void {
        var that = this; //ngSearchHelper.getCurrentActiveSearch();

        var customDatePicker: any = $("#dateTimeInput").datepicker({
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            yearRange: 'c-90:c+10',
            dateFormat: 'yy/mm/dd',
            onSelect: function (dataText, inst) {
                container.html(dataText);
                var parentRowId = $(container).parent("tr").attr("data-uid"),
                    currentGrid = ngSearchHelper.getActiveGridSearch().scope["grdSearch" + that.cGId];
                currentGrid.dataSource.getByUid(parentRowId).val.value = dataText,
                    currentGrid.dataSource.getByUid(parentRowId).val.text = dataText;
                //$(container).parents('[kendo-grid]').data("kendoGrid").dataSource.getByUid(parentRowId).val.value = dataText,
                //   $(container).parents('[kendo-grid]').data("kendoGrid").dataSource.getByUid(parentRowId).val.text = dataText;
            },
            beforeShow: function (element: any, inst: any) {
                inst.id = element.id;
                return null;
            },
            onClose: function (e) {
                copyCustomDatePickerElement.remove();
            },
            showOn: 'focus'
        });

        var copyCustomDatePickerElement = $(customDatePicker).clone(true, true).hide();
        $(customDatePicker).parents("table[role=grid]").append(copyCustomDatePickerElement[0]);
        $('#ui-datepicker-div').css('zIndex', '10000003');
        $('#ui-datepicker-div').css('font-size', 'x-small');


    }

    ifFieldIsNotExcluding(colItem) {
        var itemInclude = true;
        var st = this.schemaTypes;
        if (st.excludingFields)
            if (st.excludingFields.indexOf(',') != -1) {
                var excludingFields = st.excludingFields.split(',');
            }
            else {
                var excludingField = st.excludingFields;
                if (colItem.field === excludingField) { itemInclude = false; }
            }
        return itemInclude;
    }

    buildCurrentSearchList() {

        var that = this, //ngSearchHelper.getCurrentActiveSearch();
            extractedFilters = that.extractFilterObjectFromDataSource(),
            filterCollection = extractedFilters.filters.length > 0 ? extractedFilters : [];

        //var iFItems = ngSearchHelper.getInitialFilterItems();
        that.scope[that.scope.gridName]._requestInProgress = undefined;
        that.scope[that.scope.gridName].dataSource._requestInProgress = undefined;


        //Call to underlaying grid to do read based on compiled filter rules.
        that.scope[that.cGId].dataSource.filter(filterCollection);
        that.closeSearchWin();
    }


    closeSearchWin() {
        var that = this;
        that.scope["search" + that.cGId].close();
    }


}

module ngSearchHelper {

    export function getGridSearchInstanceById(gridId): ngSearchObj {
        var gridSearchService = angular.injector(["grdSearchServiceModule", "ng"]).get("grdSearchService");
        return gridSearchService.getGridSearchInstance(gridId);
    }
    export function getActiveGridSearch(): ngSearchObj {
        var gridSearchService = angular.injector(["grdSearchServiceModule", "ng"]).get("grdSearchService");
        return gridSearchService.getActiveGridSearchInstance();
    }
    export function setActiveGridSearch(instance: ngSearchObj): void {
        var gridSearchService = angular.injector(["grdSearchServiceModule", "ng"]).get("grdSearchService");
        return gridSearchService.setActiveGridSearchInstance(instance);
    }
    //export function initializeSearchObj(instanc :ngSearchObj): void {
    //    instanc.initializeSearchObj();
    //}

    export function initializeNewGridSearch(domElement, grdScope, $compile, gridId, ngOkCallback, ngCancelOrCloseCallback, title, seWinWidth, seWinHeight): ngSearchObj {

        var gridSearchObj = new ngSearchObj(domElement, grdScope, $compile, gridId, ngOkCallback, ngCancelOrCloseCallback, title, seWinWidth, seWinHeight);

        return gridSearchObj;

    }

}
