import { Component } from '@angular/core';
import { Headers, RequestOptions, Http } from '@angular/http';
import { Cookie } from 'ng2-cookies/ng2-cookies';
import { Configuration } from '../../app.constants';
import { DataService } from '../../services/DataService';

@Component({
    selector: 'fetchdata',
    providers: [DataService, Configuration],
    template: require('./fetchdata.component.html')
})
export class FetchDataComponent {
    links: LinkInfo[];
    noInfo: boolean = false;

    constructor(http: Http, dataService: DataService, config: Configuration) {
        let token = Cookie.get(config.CookieKey);
        if (!token) {
            this.noInfo = true;
            return;
        }
        dataService.GetLinksByUserId(token).subscribe(result => {
            this.links = result;
        });
    }
}

interface LinkInfo {
    creationDate: string;
    sourceLink: string;
    shortLink: string;
    clicks: number;
}