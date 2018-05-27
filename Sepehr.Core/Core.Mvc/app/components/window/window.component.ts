import { Component, Input, OnChanges } from '@angular/core';
import { WindowService } from '../window/server-api-services/window.service'
import { Observable } from 'rxjs';
@Component({
    selector: 'window',
    moduleId: module.id,
    templateUrl: 'window.component.html',
})
export class WindowComponent implements OnChanges  {
    @Input() templateUrl: string;
    @Input() isShow: boolean = false;	
    public urlContent: Observable<string>;
    constructor(windowService: WindowService) {
       
        if (this.templateUrl) {
            windowService.url = this.templateUrl;
            this.urlContent = windowService.getHtml()
                .subscribe(responce => this.urlContent = responce);

        }
    }
    ngOnChanges(changes) {
        if (changes['isShow']) {
            if (this.isShow == true) {
                $('#window-modal').modal('show');
            }
            else {
                $('#window-modal').modal('hide');
            }
        }
    }


}