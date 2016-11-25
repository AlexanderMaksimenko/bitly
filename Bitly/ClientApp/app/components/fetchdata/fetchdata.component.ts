import { Component } from '@angular/core';
import { Headers, RequestOptions, Http } from '@angular/http';
import { Cookie } from 'ng2-cookies/ng2-cookies';

@Component({
    selector: 'fetchdata',
    template: require('./fetchdata.component.html')
})
export class FetchDataComponent {
    links: LinkInfo[];
    noInfo: boolean = false;

    constructor(http: Http) {
        let UserIdKey = 'userId';
        let token = Cookie.get(UserIdKey);
        if (!token) {
            this.noInfo = true;
            return;
        }
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        http.get('/api/links/byUser?id=' + token,
            new RequestOptions({
                headers: headers
            })).subscribe(result => {
                this.links = result.json();
            });
    }
}

interface LinkInfo {
    creationDate: string;
    sourceLink: string;
    shortLink: string;
    clicks: number;
}
