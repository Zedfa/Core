import { Component, Input, Output, EventEmitter, ElementRef, AfterViewInit, OnChanges, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { SharedResourcesService } from 'app/common/server-api-services/shared-resources/shared-resources.service';
import { validateConfig } from '@angular/router/src/config';
import { retry } from 'rxjs/operators/retry';

@Component({
    selector: 'drop-down-list',
    moduleId: module.id,
    templateUrl: 'drop-down-list.component.html',
})
export class DropDownListComponent implements AfterViewInit {
    @Input() id: string;
    @Input() constantCategoryName: string;
    @Input() dataSourceUrl: string;
    @Input() dataSourceObject: Array<any>;
    @Input() displayPropertyName: string;
    @Input() valuePropertyName: string;
    @Output() selectedItemValueChange: EventEmitter<number> = new EventEmitter<number>();
    @Input() selectedItem: any;
    private _selectedItemValue: number;
    @Input() set selectedItemValue(value: number) {
        var oldValue = this._selectedItemValue;
        if (value) {
            this._selectedItemValue = value;
            if (value != oldValue) {
                if (this.isKendoWidgetCreated() == true) {
                    this.selectItemByValue(value);
                }
            }
        }
    }
    get selectedItemValue(): number {
        return this._selectedItemValue;
    }

    constructor() {
        if (this.constantCategoryName) {
            this.displayPropertyName = "Key",
                this.valuePropertyName = "Value",
                this.dataSourceUrl = "/api/ConstantsAPi/GetConstantByNameOfCategory?category=" + this.constantCategoryName;
        }
    }

    ngAfterViewInit(): void {
        var selectedItemValue = this.selectedItemValue;

        if (this.dataSourceObject) {
            var dropDownListComponent = this;

            $("#" + this.id).kendoDropDownList({
                dataValueField: dropDownListComponent.valuePropertyName,
                dataTextField: dropDownListComponent.displayPropertyName,
                dataSource: dropDownListComponent.dataSourceObject,
                select: function (e) {
                    var selectedItem = this.dataItems()[e.item.index()];
                    this.selectedItem = selectedItem;
                    dropDownListComponent.setSelectedItemValue(selectedItem[dropDownListComponent.valuePropertyName]);
                    e.preventDefault();
                },
                dataBound: function (e) {
                    var dropdownlistItems = this.dataSource.data();
                    var searchfield = "";

                    $.each(dropdownlistItems, (index, item) => {
                        if (item.SubjectID == selectedItemValue) {
                            $('#' + this.id).data("kendoDropDownList").value(selectedItemValue.toString());
                            $('#' + this.id).data("kendoDropDownList").trigger("change");
                        }
                    });
                }
            });
        }
    }

    setSelectedItemValue(value: number) {
        this.selectedItemValue = value;
        this.selectedItemValueChange.emit(this.selectedItemValue);
    }

    isKendoWidgetCreated() {
        if ($('#' + this.id).data("kendoDropDownList"))
            return true;
        return false;
    }
    selectItemByValue(value: number) {
        var dropdownlist = $('#' + this.id).data("kendoDropDownList");
        if (dropdownlist) {
            var dropdownlistItems = dropdownlist.dataSource.data();
            var searchfield = "";

            $.each(dropdownlistItems, (index, item) => {
                if (item.SubjectID == value) {
                    $('#' + this.id).data("kendoDropDownList").value(value.toString());
                    $('#' + this.id).data("kendoDropDownList").trigger("change");
                }
            });
        }
    };
}