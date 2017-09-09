var ngSearchObj = (function () {
    function ngSearchObj(dom, scope, $compile, immAncesstorGridId, ngOkCallback, ngCancelOrCloseCallback, title, seWinWidth, seWinHeight) {
        this.afterFirstContentUrlAssigned = false;
        this.andOrDropDownAlreadyAssigned = false;
        this.searchableColumnsList = [];
        this.selectedColumnCriteria = {};
        this.fieldOptcont = null;
        this.columnCont = null;
        this.schemaTypes = null;
        this.booleanValContainer = {};
        this.multiValueFields = {};
        this.seWinWidth = "auto";
        this.seWinHeight = "250";
        this.cGId = "";
        this.filterSeperator = "|";
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
    ngSearchObj.prototype.initializeSearchObj = function () {
        this.buildWindowContentTemplate(this.cGId);
    };
    ngSearchObj.prototype.buildWindowContentAttributes = function (gridId) {
        this.searchWindowAttributes = { 'contentSearchId': 'searchGrid_' + gridId, 'winContainerId': 'win_se_' + gridId };
    };
    ngSearchObj.prototype.buildWindowContentTemplate = function (gridId) {
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
            var data = that.scope["grdSearch" + that.cGId].dataSource.data(), pager = that.outerDom.find("div[data-role=pager]");
            try {
                that.buildCurrentSearchList();
                pager.children("[class='search-text']").remove();
                if (data.length > 0) {
                    var container = $("<div>", { "class": "search-text" }).text("جستجوی فعلی : ");
                    $.each(data, function (index, record) {
                        var logic = index == 0 ? "" : record.andor.andorName, displayText = record.fld.columnId.type == "boolean" ? record.val.value : record.val.text, conditionText = "<span class='search-text-item'> <strong > " + logic + " </strong>"
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
        };
        this.scope.cancelSearch = function (sc) {
            that.closeSearchWin();
        };
        this.scope.searchVisible = false;
        this.scope.addSearchRuleRow = function () {
            that.addCriteriaRecord();
        };
        var searchAddRuleBtn = '<button k-button ng-click="addSearchRuleRow()" > <span class="k-icon k-i-plus" ></span></button>';
        this.scope.searchToolbarTemplate = searchAddRuleBtn;
        this.scope.searchOpen = function (arg) {
        };
        this.scope.searchClose = function (arg) {
            that.scope["search" + that.cGId].refresh();
        };
        var wnGrdTemplate = $('<div kendo-window="search' + gridId + '" k-modal="true"  k-width="' + this.seWinWidth + '"k-heigth="' + this.seWinHeight + '" title="' + this.title + '" k-visible="searchVisible" on-open="searchOpen($event)"   on-close="searchClose($event)"  >' +
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
    };
    ngSearchObj.prototype.getCurrentSearchGrid = function () {
        return this.scope["grdSearch" + this.cGId];
    };
    ngSearchObj.prototype.openSearchWindow = function () {
        this.scope.searchVisible = true;
        this.scope["search" + this.cGId].title('جستجو');
        this.scope["search" + this.cGId].center().open();
    };
    ngSearchObj.prototype.getSchemaInfoByField = function (field) {
        var that = this, schemaInfo = null, loc = "";
        $.each(that.schemaTypes, function (schemaField, info) {
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
    };
    ngSearchObj.prototype.findColumnAccordingName = function (field) {
        var columns = this.scope[this.scope.gridName].columns, result = {};
        $.each(columns, function (i, col) {
            if (col.field == field) {
                result = col;
                return false;
            }
        });
        return result;
    };
    ngSearchObj.prototype.getValueModelByType = function (type, val) {
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
                valueInfo.value = typeof (val) == "string" ? val.split(this.filterSeperator)[0] : val;
                break;
            default:
                valueInfo.text = val,
                    valueInfo.value = val;
                break;
        }
        return valueInfo;
    };
    ngSearchObj.prototype.convertFiltersToData = function (data, filterCollection, logic, initfilter) {
        var self = this, rowLogic = data.length >= 1 ? logic : "";
        if (filterCollection.filters) {
            $.each(filterCollection.filters, function (index, filter) {
                if (filter.filters) {
                    self.convertFiltersToData(data, filter, rowLogic, initfilter);
                }
                else if (!isEqual(initfilter, filter) || (Object.hasOwnProperty(filter.show) && filter.show))
                    data.push(self.createModelAccordingFilter(filter, filterCollection.logic));
            });
        }
        else if (!isEqual(initfilter, filterCollection) || (Object.hasOwnProperty(filterCollection.show) && filterCollection.show)) {
            data.push(self.createModelAccordingFilter(filterCollection, rowLogic));
        }
    };
    ngSearchObj.prototype.createModelAccordingFilter = function (filter, logic) {
        var that = this, fieldInfo = null, andOrInfo = that.getLogicModel(logic), columns = this.scope[this.scope.gridName].columns, operatorInfo = null, valueInfo;
        var navigateInfo = that.getSchemaInfoByField(filter.field), navigationColumn = navigateInfo.info, colName = navigateInfo.position;
        if (navigationColumn) {
            switch (navigationColumn.custType.toLowerCase()) {
                case "lookup":
                    fieldInfo = { columnId: { type: "lookup", LId: colName, TId: colName }, columnName: navigationColumn.lookupInfo.title };
                    break;
                case "dropdown":
                    fieldInfo = { columnId: { type: "dropdown", LId: colName, TId: colName }, columnName: navigationColumn.dropdownInfo.DisplayName };
                    break;
            }
            ;
            valueInfo = that.getValueModelByType(navigationColumn.custType, filter.value);
        }
        else {
            $.each(columns, function (i, col) {
                fieldInfo = that.getColumnObj(col);
                if (col.field == filter.field) {
                    valueInfo = that.getValueModelByType(fieldInfo.columnId.type, filter.value);
                    return false;
                }
            });
        }
        $.each(that.getOperatorsByType(fieldInfo.columnId.type), function (i, opt) {
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
        };
    };
    ngSearchObj.prototype.buildSearchGrid = function () {
        var _this = this;
        var that = this, columnObj = that.getColumnObj(that.searchableColumnsList[0]), dataArray = [], mainGridFilters = that.scope[that.scope.gridName].dataSource.filter(), currentData = [];
        if (mainGridFilters) {
            $.each(mainGridFilters.filters, function (i, filter) {
                if (that.scope.initialFilter && that.scope.initialFilter.filters.length > 0) {
                    $.each(_this.scope.initialFilter.filters, function (j, initFilter) {
                        that.convertFiltersToData(currentData, filter, mainGridFilters.logic, initFilter);
                    });
                }
                else {
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
                { command: { text: "حذف ", click: that.removeCurrentRow }, width: 110 }
            ],
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
        };
        return seConfig;
    };
    ngSearchObj.prototype.selectDeselectCheckBox = function (chkItem, trueText, falseText) {
        if ($(this).prop('checked')) {
            $(this).text(trueText);
        }
        else {
            $(this).text(falseText);
        }
    };
    ngSearchObj.prototype.getLogicModel = function (logic) {
        var result = null, self = this, logicCollection = self.getWholeLogicModel();
        if (logic) {
            $.each(logicCollection, function (index, item) {
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
    };
    ngSearchObj.prototype.getWholeLogicModel = function () {
        return [{ andorId: "and", andorName: "و" }, { andorId: "or", andorName: "یا" }];
    };
    ngSearchObj.prototype.addCriteriaRecord = function () {
        var that = this, dataSource = this.scope["grdSearch" + that.cGId].dataSource, columnObj = that.getColumnObj(that.searchableColumnsList[0]), logic = dataSource.data().length > 0 ? that.getWholeLogicModel()[0] : { andorId: "", andorName: "" }, newItem = kendo.observable({
            fld: columnObj,
            opt: that.getFirstItemOperatorBasedOnColType(columnObj.columnId.type),
            andor: logic,
            val: {
                text: "",
                value: ""
            }
        });
        dataSource.add(newItem);
    };
    ngSearchObj.prototype.getDataFields = function () {
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
    };
    ngSearchObj.prototype.getColumnObj = function (column) {
        var that = this;
        var fields = that.scope[that.cGId].options.dataSource.schema.model.fields;
        var retcol;
        $.each(fields, function (fieldName, fieldType) {
            if (column.field.toLowerCase() === fieldName.toLowerCase()) {
                retcol = {
                    columnId: { type: that.getFieldSpecificType(column, fieldType, that.schemaTypes), LId: that.getTrueFieldName(fieldName), TId: fieldName },
                    columnName: column.title,
                };
                return false;
            }
        });
        return retcol;
    };
    ngSearchObj.prototype.getTrueFieldName = function (key) {
        var that = this;
        if (that.schemaTypes[key]) {
            if (that.schemaTypes[key].mdlPropName)
                return that.schemaTypes[key].mdlPropName;
        }
        return key;
    };
    ngSearchObj.prototype.getFieldSpecificType = function (column, val, sTypes) {
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
    };
    ngSearchObj.prototype.removeCurrentRow = function (event) {
        var that = ngSearchHelper.getActiveGridSearch(), searchGrid = that.scope[$(event.delegateTarget).attr("kendo-grid")], currentRowUid = $(event.target).closest("tr").attr("data-uid");
        searchGrid.dataSource.remove(searchGrid.dataSource.getByUid(currentRowUid));
        event.preventDefault();
    };
    ngSearchObj.prototype.columnDropDownEditor = function (cont, options) {
        var that = ngSearchHelper.getActiveGridSearch();
        that.columnCont = cont;
        var input = $('<input id="column" data-text-field="columnName" data-value-field="columnId.LId" data-bind="value:' + options.field + '"/>');
        input.appendTo(cont);
        input.kendoDropDownList({
            autoBind: true,
            dataSource: { data: that.getDataFields(), events: { requestEnd: function (e) { } } },
            dataBound: function (e) {
            },
            select: function (e) {
                $.each(e.sender.dataSource.options.data, function (key, val) {
                    if (key == $(e.item).index()) {
                        var parentRowId = cont.parent("tr").attr("data-uid");
                        that.selectedColumnCriteria[parentRowId] = val;
                        that.columnCont.parent("tr").find("td:nth-child(3)").focus();
                        that.columnCont.parent("tr").find("td:nth-child(3)").text();
                        return;
                    }
                });
            },
            change: function (e) {
                var model = that.scope["grdSearch" + that.cGId].dataSource.getByUid(that.columnCont.parent("tr").attr("data-uid"));
                that.columnCont.parent("tr").find("td:nth-child(3)").text(that.getFirstItemOperatorBasedOnColType(model.fld.columnId.type).operatorName);
                model.val.value = "",
                    model.val.text = "",
                    model.opt.operatorId = that.getFirstItemOperatorBasedOnColType(model.fld.columnId.type).operatorId,
                    model.opt.operatorName = that.getFirstItemOperatorBasedOnColType(model.fld.columnId.type).operatorName;
                that.columnCont.parent("tr").find("td:nth-child(4)").click();
            }
        });
    };
    ngSearchObj.prototype.getFirstItemOperatorBasedOnColType = function (colType) {
        var that = this;
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
    };
    ngSearchObj.prototype.getOperatorsByType = function (fldType) {
        var that = this;
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
    };
    ngSearchObj.prototype.getBooleanFirstItem = function (colItem) {
        var _this = this;
        var col = _.find(this.schemaTypes, function (col_item) {
            return _this.schemaTypes[colItem.field] && col_item.custType.toLowerCase() == 'boolean';
        });
        var boolObj;
        if (col) {
            boolObj = { boolVal: col.falseEqui, boolText: col.falseEqui };
        }
        return boolObj;
    };
    ngSearchObj.prototype.operatorDropDownEditor = function (cont, options) {
        var that = ngSearchHelper.getActiveGridSearch(), fieldOptcont = cont, parentRowId = cont.parent("tr").attr("data-uid"), fld = options.model.fld;
        $('<input required data-text-field="operatorName" data-value-field="operatorId" data-bind="value:' + options.field + '"/>')
            .appendTo(cont)
            .kendoDropDownList({
            autoBind: false,
            dataSource: that.getOperatorsByType(options.model.fld.columnId.type),
            dataBound: function (e) {
            },
            select: function (e) {
            }
        });
    };
    ngSearchObj.prototype.getIntRelatedOperators = function () {
        return [{ operatorName: "برابر با", operatorId: "eq" },
            { operatorId: "neq", operatorName: "رقمی غیر از" },
            { operatorName: "بزرگتر از", operatorId: "gt" },
            { operatorName: "کوچکتر از ", operatorId: "lt" }];
    };
    ngSearchObj.prototype.getBooleanRelatedOperators = function () {
        return [{ operatorName: "برابر با", operatorId: "eq" },
            { operatorId: "neq", operatorName: "نا برابر با" }];
    };
    ngSearchObj.prototype.getStringRelatedOperators = function () {
        return [
            { operatorName: "شامل عبارت", operatorId: "contains" },
            { operatorName: "برابر با", operatorId: "eq" },
            { operatorName: "شامل نشود عبارت", operatorId: "doesnotcontain" },
            { operatorName: "(به ترتیب الفبا)بعد از عبارت", operatorId: "gte" },
            { operatorName: "(به ترتیب الفبا)قبل از عبارت", operatorId: "lte" },
        ];
    };
    ngSearchObj.prototype.getLookupRelatedOperators = function () {
        return [{ operatorName: "برابر با", operatorId: "eq" }];
    };
    ngSearchObj.prototype.getDateRelatedOperators = function () {
        return [{ operatorName: "برابر با", operatorId: "eq" },
            { operatorId: "neq", operatorName: "غیر از تاریخ" },
            { operatorName: "از تاریخ", operatorId: "gt" },
            { operatorName: "تا تاریخ", operatorId: "lt" }];
    };
    ngSearchObj.prototype.getDateTimeRelatedOperators = function () {
        return [{ operatorName: "برابر با", operatorId: "eq" },
            { operatorName: "نا برابر با", operatorId: "neq" },
            { operatorName: "از تاریخ", operatorId: "gt" },
            { operatorName: "تا تاریخ", operatorId: "lt" }];
    };
    ngSearchObj.prototype.getTimeRelatedOperators = function () {
        return [{ operatorName: "برابر با", operatorId: "eq" },
            { operatorId: "neq", operatorName: "غیر از زمان" },
            { operatorName: "بعد از زمان", operatorId: "gt" },
            { operatorName: "قبل از زمان", operatorId: "lt" }];
    };
    ngSearchObj.prototype.getTheWholeRules = function () {
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
            { operatorName: "غیر از تاریخ", operatorId: "neq", },
            { operatorName: "از تاریخ", operatorId: "gte" },
            { operatorName: "تا تاریخ", operatorId: "lte" },
            { operatorName: "اعمال", operatorId: "eq" },
            { operatorName: "عدم اعمال", operatorId: "neq" },
            { operatorName: "غیر از زمان", operatorId: "neq" },
            { operatorName: "بعد از زمان", operatorId: "gte" },
            { operatorName: "قبل از زمان", operatorId: "lte" }
        ];
    };
    ngSearchObj.prototype.andorDropDownEditor = function (container, options) {
        if (!container.parent().is("tr:first-child")) {
            var that = ngSearchHelper.getActiveGridSearch();
            $('<input required data-text-field="andorName" data-value-field="andorId" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                autoBind: false,
                dataSource: that.getWholeLogicModel()
            });
        }
    };
    ngSearchObj.prototype.valComplexInputsEditor = function (container, options) {
        var that = ngSearchHelper.getActiveGridSearch();
        that.makeInputElement(container, options);
    };
    ngSearchObj.prototype.createValueTemplate = function (dataItem) {
        var result, that = ngSearchHelper.getActiveGridSearch();
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
    };
    ngSearchObj.prototype.createLookupTemplate = function (info, model) {
        var currentGrid = ngSearchHelper.getActiveGridSearch();
        model.lookupDblClick = function (args) {
            ngSearchHelper.setActiveGridSearch(currentGrid);
        };
        var lookup = "<cust-lookup lookup-id='" + info.lookupName + "_" + model.uid.split('-')[0] + "'"
            + " title=\"'" + info.title + "'\""
            + " value-name=dataItem.val.value"
            + " display-name=dataItem.val.text"
            + " lkp-value-name='" + info.valueName + "'"
            + " lkp-display-name='" + info.displayName + "'"
            + " view-model-name='" + info.viewModelName + "'"
            + " lkp-prop-name='" + info.viewInfoName + "'"
            + " lkp-dbl-click='dataItem.lookupDblClick(args)'"
            + " win-width=900"
            + " win-height=500>"
            + " </cust-lookup>";
        kendo.bind(lookup, model);
        return lookup;
    };
    ngSearchObj.prototype.createDropDownTemplate = function (info, model) {
        var dropDown = "<drop-down-list id=search_" + info.DisplayName + "_" + model.uid
            + " display-name=" + info.displayName
            + " value-name=" + info.valueName
            + " db-category-name=" + info.dbCategoryName
            + " url=" + info.url
            + " property-id=dataItem.val.value"
            + " property-name=dataItem.val.text"
            + " ></drop-down-list>";
        kendo.bind(dropDown, model);
        return dropDown;
    };
    ngSearchObj.prototype.createCurrencyTemplate = function (model) {
        var numericTextBox = '<input type="text" data-value-update="keyup" data-bind="value: dataItem.val.value, text: dataItem.val.text" price-format price-value="dataItem.val.value"/>';
        kendo.bind(numericTextBox, model);
        return numericTextBox;
    };
    ngSearchObj.prototype.setValueAccordingByType = function (record) {
        var typeName = record.fld.columnId.type.toLowerCase();
        if (record.keys != undefined || record.keys != null) {
            record.val = record.keys;
        }
        if (typeName == "datetime" || typeName == "date") {
            record.val.value += this.filterSeperator + "dt";
        }
        else if (typeName == "persiandate") {
            record.val.value += this.filterSeperator + "pdt";
        }
        else if (typeName === "lookup") {
            var lookupObj = this.getLookupPropertyValue(record.fld.columnId.LId);
            record.val.value += this.filterSeperator + "lkp:" + lookupObj.bindingName;
        }
        else if (typeName === "dropdown") {
            var dropdownInfo = this.getDropDownPropertyValue(record.fld.columnId.LId);
            record.val.value += this.filterSeperator + "ddl:" + dropdownInfo.propertyNameForBinding;
        }
        else if (typeName === "currency") {
            record.val.value = record.val.value;
        }
        else if (typeName === "boolean" || typeName === "bool") {
            record.val.text = record.val.value;
        }
        return record;
    };
    ngSearchObj.prototype.createFiltersTree = function (dataItem, logic, allFilters) {
        if (allFilters.filters.length == 0 && !allFilters.logic) {
            allFilters.filters.push(dataItem);
        }
        else {
            if (allFilters.filters.length == 1 && !allFilters.filters[0].filters) {
                allFilters.filters.push(dataItem);
                if (logic)
                    allFilters.logic = logic;
            }
            else {
                var subFilter = { filters: [allFilters, dataItem], logic: logic };
                allFilters.filters = JSON.parse(JSON.stringify(subFilter.filters)),
                    allFilters.logic = JSON.parse(JSON.stringify(subFilter.logic));
            }
        }
    };
    ngSearchObj.prototype.extractFilterObjectFromDataSource = function () {
        var alreadyInsertedCond = [];
        var that = this, data = that.scope["grdSearch" + that.cGId].dataSource.data(), copiedData = JSON.parse(JSON.stringify(data));
        $.each(copiedData, function (key, item) {
            alreadyInsertedCond.push(that.setValueAccordingByType(item));
        });
        var baseFilter = jQuery.extend(true, {}, this.scope.initialFilter), mainFilters = { logic: "", filters: [] }, filtersGroup = null;
        if (this.scope.initialFilter && this.scope.initialFilter.filters.length > 0) {
            mainFilters.logic = baseFilter.logic;
            mainFilters.filters = baseFilter.filters;
        }
        $.each(alreadyInsertedCond, function (index, record) {
            var filterItem = that.makeFilterItem(record);
            that.createFiltersTree(filterItem, record.andor.andorId, mainFilters);
        });
        return mainFilters;
    };
    ngSearchObj.prototype.makeFilterItem = function (insertingCond) {
        if (insertingCond.val == undefined || insertingCond.val === '')
            return null;
        else {
            var condition = insertingCond.val, filterItem = { field: insertingCond.fld.columnId.LId, operator: insertingCond.opt.operatorId, value: condition.value ? condition.value : condition.text };
            return filterItem;
        }
    };
    ngSearchObj.prototype.getNavigationPropertyValue = function (fieldVal) {
        var navProp = this.schemaTypes[fieldVal].navProp;
        return navProp;
    };
    ngSearchObj.prototype.getLookupPropertyValue = function (fieldVal) {
        var lookupProp = this.schemaTypes[fieldVal].lookupInfo;
        return lookupProp;
    };
    ngSearchObj.prototype.getDropDownPropertyValue = function (fieldVal) {
        var dropdownInfo = this.schemaTypes[fieldVal].dropdownInfo;
        return dropdownInfo;
    };
    ngSearchObj.prototype.removeAnyPreviouslyInsertedItems = function (con) {
        con.find('span').remove();
        con.find('input').remove();
    };
    ngSearchObj.prototype.makeEnumDdl = function (cont, columnCriteria, options) {
        var that = this;
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
    };
    ngSearchObj.prototype.getEnumDataSourceEquivalent = function (colItem) {
        var eDic = colItem.enumDic;
        var enumDataSource = [];
        if (eDic) {
            $.each(eDic, function (key, val) {
                enumDataSource.push({ tag: key, text: val });
            });
            return enumDataSource;
        }
    };
    ngSearchObj.prototype.makeInputElement = function (cont, options) {
        var that = ngSearchHelper.getActiveGridSearch();
        var columnCriteria = options.model.fld, columnType = columnCriteria.columnId.type;
        switch (columnType.toLowerCase()) {
            case 'number':
            case 'digit':
                $('<input type="number" data-role="numerictextbox" data-spinners="false" class="k-input" data-bind="value:val.value"/>').appendTo(cont);
                break;
            case 'currency':
                $('<input type="text" data-value-update="keyup" data-bind="value:val.value, text: val.text" price-format price-value="val.value"/>').appendTo(cont);
                break;
            case 'bool':
            case 'boolean':
                $('<input type="checkbox"  data-type="boolean" value= "true" data-bind="checked: val.value, text:val.value" ng-model="dataItem.val.value" />').appendTo(cont);
                break;
            case 'enum':
                that.makeEnumDdl(cont, columnCriteria, options);
                break;
            case 'string':
                $('<input type="text"  class="k-input k-textbox"  data-bind="value:val.value"/>').appendTo(cont);
                break;
            case 'dropdown':
                var uid = $(cont).parent("tr").attr("data-uid"), dropdowInfo = that.getDropDownPropertyValue(columnCriteria.columnId.TId);
                this.scope.onDropDownChange = function (args) {
                    setColumnValue(args);
                };
                this.scope.onDropDownDataBound = function (args, scope) {
                    setColumnValue(args);
                };
                var setColumnValue = function (widget) {
                    $(cont).parents('[kendo-grid]').data("kendoGrid").dataSource.getByUid(uid).val.value = widget.sender.value();
                    $(cont).parents('[kendo-grid]').data("kendoGrid").dataSource.getByUid(uid).val.text = widget.sender.text();
                };
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
                break;
            case 'lookup':
                var lookup = that.getLookupPropertyValue(columnCriteria.columnId.TId), lkpIndx = cont.parent('tr').index(), currentInstance = ngSearchHelper.getActiveGridSearch();
                this.scope.lookupDblClick = function (args, parentRowId) {
                    currentInstance.scope["grdSearch" + that.cGId].dataSource.getByUid(parentRowId).val.value = args.scope.valueName,
                        currentInstance.scope["grdSearch" + that.cGId].dataSource.getByUid(parentRowId).val.text = args.scope.displayName;
                    ngSearchHelper.setActiveGridSearch(currentInstance);
                };
                var lkp = this.compile("<cust-lookup lookup-id=" + lookup.lookupName + "_" + lkpIndx
                    + " title=\"'" + lookup.title + "'\""
                    + " value-name= dataItem.val.value"
                    + " display-name = dataItem.val.text"
                    + " lkp-value-name='" + lookup.valueName + "'"
                    + " lkp-display-name='" + lookup.displayName + "'"
                    + " view-model-name='" + lookup.viewModelName + "'"
                    + " lkp-prop-name='" + lookup.viewInfoName + "'"
                    + " lkp-dbl-click=lookupDblClick(args,'" + cont.parent('tr').data().uid + "')"
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
            case 'time':
                break;
            default:
                $('<input type="text" class="k-input k-textbox" data-bind="value:val.value"/>').appendTo(cont);
        }
    };
    ngSearchObj.prototype.setDateTimePicker = function (container, inputHtml) {
        var that = this;
        var customDatePicker = $("#dateTimeInput").datepicker({
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            yearRange: 'c-90:c+10',
            dateFormat: 'yy/mm/dd',
            onSelect: function (dataText, inst) {
                container.html(dataText);
                var parentRowId = $(container).parent("tr").attr("data-uid"), currentGrid = ngSearchHelper.getActiveGridSearch().scope["grdSearch" + that.cGId];
                currentGrid.dataSource.getByUid(parentRowId).val.value = dataText,
                    currentGrid.dataSource.getByUid(parentRowId).val.text = dataText;
            },
            beforeShow: function (element, inst) {
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
    };
    ngSearchObj.prototype.ifFieldIsNotExcluding = function (colItem) {
        var itemInclude = true;
        var st = this.schemaTypes;
        if (st.excludingFields)
            if (st.excludingFields.indexOf(',') != -1) {
                var excludingFields = st.excludingFields.split(',');
            }
            else {
                var excludingField = st.excludingFields;
                if (colItem.field === excludingField) {
                    itemInclude = false;
                }
            }
        return itemInclude;
    };
    ngSearchObj.prototype.buildCurrentSearchList = function () {
        var that = this, extractedFilters = that.extractFilterObjectFromDataSource(), filterCollection = extractedFilters.filters.length > 0 ? extractedFilters : [];
        that.scope[that.scope.gridName]._requestInProgress = undefined;
        that.scope[that.scope.gridName].dataSource._requestInProgress = undefined;
        that.scope[that.cGId].dataSource.filter(filterCollection);
        that.closeSearchWin();
    };
    ngSearchObj.prototype.closeSearchWin = function () {
        var that = this;
        that.scope["search" + that.cGId].close();
    };
    return ngSearchObj;
}());
var ngSearchHelper;
(function (ngSearchHelper) {
    function getGridSearchInstanceById(gridId) {
        var gridSearchService = angular.injector(["grdSearchServiceModule", "ng"]).get("grdSearchService");
        return gridSearchService.getGridSearchInstance(gridId);
    }
    ngSearchHelper.getGridSearchInstanceById = getGridSearchInstanceById;
    function getActiveGridSearch() {
        var gridSearchService = angular.injector(["grdSearchServiceModule", "ng"]).get("grdSearchService");
        return gridSearchService.getActiveGridSearchInstance();
    }
    ngSearchHelper.getActiveGridSearch = getActiveGridSearch;
    function setActiveGridSearch(instance) {
        var gridSearchService = angular.injector(["grdSearchServiceModule", "ng"]).get("grdSearchService");
        return gridSearchService.setActiveGridSearchInstance(instance);
    }
    ngSearchHelper.setActiveGridSearch = setActiveGridSearch;
    function initializeNewGridSearch(domElement, grdScope, $compile, gridId, ngOkCallback, ngCancelOrCloseCallback, title, seWinWidth, seWinHeight) {
        var gridSearchObj = new ngSearchObj(domElement, grdScope, $compile, gridId, ngOkCallback, ngCancelOrCloseCallback, title, seWinWidth, seWinHeight);
        return gridSearchObj;
    }
    ngSearchHelper.initializeNewGridSearch = initializeNewGridSearch;
})(ngSearchHelper || (ngSearchHelper = {}));
