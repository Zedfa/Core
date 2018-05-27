import { ServiceBase } from "../service-base";
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/toPromise';
import { ResponseBase } from "../../../../typings/response-base";
import { RequestBase } from "../../../../typings/request-base";

@Injectable()
export class ApiServiceBase<TRequest, TResponse> extends ServiceBase {
    constructor(public http: HttpClient, public url?: string) {
        super();
    }

    public getAsObservable(): Observable<ResponseBase<TResponse>> {
        const response = this.http.get<ResponseBase<TResponse>>(this.url, { responseType: "json", params: { request: JSON.stringify(new RequestBase()) } });

        return response;
    }

    public getAsObservableWithRequest(request: TRequest): Observable<ResponseBase<TResponse>> {

        const response = this.http.get<ResponseBase<TResponse>>(this.url, { responseType: "json", params: { request: JSON.stringify(request) } });
        return response;

    }
    public async getAsPromise(): Promise<ResponseBase<TResponse>> {

        const response = this.http.get<ResponseBase<TResponse>>(this.url, { responseType: "json", params: { request: JSON.stringify(new RequestBase()) } }
        ).toPromise();

        return response;
    }
    public async getAsPromiseWithRequest(request: TRequest): Promise<ResponseBase<TResponse>> {


        const response = this.http.get<ResponseBase<TResponse>>(this.url, { responseType: "json", params: { request: JSON.stringify(request) } }).toPromise();

        return response;
    }
    public postAsObservableWithRequest(request: TRequest): Observable<ResponseBase<TResponse>> {

        const response = this.http.post<ResponseBase<TResponse>>(this.url, request);

        return response;
    }
    public async postAsPromiseWithRequest(request: TRequest): Promise<ResponseBase<TResponse>> {

        const response = this.http.post<ResponseBase<TResponse>>(this.url, request).toPromise();

        return response;
    }
    public async deleteAsPromiseWithRequest(request: TRequest): Promise<ResponseBase<TResponse>> {

        const response = this.http.delete<ResponseBase<TResponse>>(this.url, request).toPromise();
        return response;
    }

}                                         