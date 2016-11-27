import { Component, Inject } from '@angular/core';
import { Headers, RequestOptions, Http } from '@angular/http'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { Configuration } from '../../app.constants';
import { DataService } from '../../services/DataService';
import { Cookie } from 'ng2-cookies/ng2-cookies';

@Component({
    selector: 'home',
    providers: [DataService, Configuration],
    template: require('./home.component.html')
})
export class HomeComponent {
    mainForm: FormGroup;
    sourceLink: AbstractControl;
    shortLinkResult: ShortLink;
    generating: boolean = false;

    constructor(fb: FormBuilder, private http: Http, private config: Configuration, private dataService: DataService) {
        this.mainForm = fb.group({
            'sourceLink': ['', Validators.required]
        });
        this.sourceLink = this.mainForm.controls['sourceLink'];
    }

    onSubmit(formData): void {
        this.generating = true;
        this.shortLinkResult = null;
        let user = null;
        let token = Cookie.get(this.config.CookieKey);

        if (token) {
            formData.user = { id: token };
        }

        this.dataService.AddLink(formData).subscribe(result => {
            this.shortLinkResult = {
                shortLink: result.shortLink,
                sourceLink: result.sourceLink
            };
            if (!token) {
                Cookie.set(this.config.CookieKey, result.user.id);
            }
            this.generating = false;
        });
    }
}

interface ShortLink {
    sourceLink: string,
    shortLink: string
}