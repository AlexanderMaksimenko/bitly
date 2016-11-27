import { Component, OnInit, Inject } from '@angular/core';
import { ExternalRedirectMaker } from '../../ExternalRedirectMaker'
import { Location } from '@angular/common';
import { Headers, RequestOptions, Http } from '@angular/http';
import { Router} from "@angular/router";
import { Configuration } from '../../app.constants';
import { DataService } from '../../services/DataService';

@Component({
    selector: 'counter',
    providers: [DataService, Configuration, ExternalRedirectMaker],
    template: require('./redirect.component.html')
})

export class RedirectComponent {
    public noInfo: boolean = false;

    constructor(redirect: ExternalRedirectMaker, router: Router, location: Location, http: Http, dataService: DataService, config: Configuration) {
        var shortLink = location.path().substring(1);
        console.log('shortLink - ' + shortLink);

        dataService.GetLinksByShortLink(shortLink).subscribe(link => {
            if (link) {
                console.log(this.noInfo);
                //need something to reload page
                redirect.MakeRedirerct(link.sourceLink);
                router.navigateByUrl('/api/redirect/' + link.sourceLink.replace(/\//g, '-'));
            }
            this.noInfo = true;
        });
    }
}