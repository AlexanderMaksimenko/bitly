import { Component, Inject } from '@angular/core';
import { Headers, RequestOptions, Http } from '@angular/http'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { Cookie } from 'ng2-cookies/ng2-cookies';

@Component({
    selector: 'home',
    template: require('./home.component.html')
})
export class HomeComponent {
    mainForm: FormGroup;
    sourceLink: AbstractControl;
    shortLinkResult: ShortLink;
    generating: boolean = false;
    
    constructor(fb: FormBuilder, private http: Http) {
        this.mainForm = fb.group({
            'sourceLink': ['', Validators.required]
        });
        this.sourceLink = this.mainForm.controls['sourceLink'];        
    }

    onSubmit(formData): void {
        this.generating = true;
        this.shortLinkResult = null;

        let UserIdKey = 'userId';
        let token = Cookie.get(UserIdKey);
        
        let user = null;
        if (token) {
            formData.user = { id: token };
        }
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');

        this.http.post('/api/links', formData, new RequestOptions({
            headers: headers
        })).subscribe(responce => {
            let result = responce.json();
            this.shortLinkResult = {
                shortLink: result.shortLink,
                sourceLink: result.sourceLink
            };            
            if (!token) {
                Cookie.set(UserIdKey, result.user.id);
            }
            
            this.generating = false;
        });
    }
}

interface ShortLink {
    sourceLink: string,
    shortLink: string
}