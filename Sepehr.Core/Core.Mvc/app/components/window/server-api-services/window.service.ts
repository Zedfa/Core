import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiServiceBase } from '../../../../../../Areas/Core/Scripts/tools/app/services/server-api/api-service-base';
import { RequestBase } from '../../../../Scripts/typings/request-base';
import { ResponseBase } from '../../../../Scripts/typings/response-base';
import { ResponseData } from '../../../../Scripts/typings/response-data';

@Injectable()
export class WindowService extends ApiServiceBase<RequestBase, ResponseBase<ResponseData>> {
    public contentText
    constructor(http: HttpClient) {
        super(http);
    }

    getHtml() {
        this.getAsObservable().subscribe((data) => {
            this.contentText = data
        });
        return this.contentText
    }

}