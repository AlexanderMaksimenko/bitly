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

    constructor(private redirect: ExternalRedirectMaker, router: Router, location: Location, private dataService: DataService, config: Configuration) {
        var shortLink = location.path().substring(1);
        this.onInit(shortLink);

    }
    onInit(shortLink): void {
        this.dataService.GetLinksByShortLink(shortLink).subscribe(link => {
        //when use subscribe the get request call one more time, its frustrating (
            if (link) {
                this.redirect.MakeRedirerct(link.sourceLink);
            }
            this.noInfo = true;
        });
    }
}