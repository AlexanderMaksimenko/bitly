import { Component, OnInit, Inject } from '@angular/core';
import { ExternalRedirectMaker } from '../../ExternalRedirectMaker'
import { Location } from '@angular/common';
import { Headers, RequestOptions, Http } from '@angular/http';

@Component({
    selector: 'counter',
    providers: [ExternalRedirectMaker],
    template: require('./redirect.component.html')
})

export class RedirectComponent {
    public headers = new Headers();
    public noInfo: boolean = false;
     
    constructor(private redirect: ExternalRedirectMaker, location: Location, http: Http) {
        var shortLink = location.path().substring(1);
        console.log('shortLink - ' + shortLink);

        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Access-Control-Allow-Origin', '*');

        http.get('/api/links/' + shortLink,
            new RequestOptions({
                headers: this.headers
            })).subscribe(result => {
                var link = result.json();
                if (link) {
                    console.log('making redirect to ' + link.sourceLink);
                    redirect.MakeRedirerct(link.sourceLink);
                }
                this.noInfo = true;
            });
    }
}