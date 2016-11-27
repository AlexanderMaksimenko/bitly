import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';
import { Configuration } from '../app.constants';

@Injectable()
export class DataService {

    private actionUrl: string;
    private headers: Headers;

    constructor(private _http: Http, private _configuration: Configuration) {

        this.actionUrl = _configuration.ApiUrl + 'links/';

        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
    }

    public GetLinksByUserId = (id): Observable<any> => {
        return this._http.get(this.actionUrl + 'byUser?id=' + id).map((response: Response) => <any>response.json());
    }

    public GetLinksByShortLink = (shortLink): Observable<any> => {
        return this._http.get(this.actionUrl + shortLink).map((response: Response) => <any>response.json());
    }

    public AddLink = (formData): Observable<any> => {
        var toAdd = JSON.stringify(formData);
        return this._http.post(this.actionUrl, toAdd, { headers: this.headers }).map(res => res.json());
    }
}