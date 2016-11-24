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
    public headers = new Headers();
    //TODO add custom validator
    constructor(fb: FormBuilder, private http: Http) {
        this.mainForm = fb.group({
            'sourceLink': ['', Validators.required]
        });
        this.sourceLink = this.mainForm.controls['sourceLink'];        
    }

    onSubmit(formData): void {
        let UserIdKey = 'userId';
        let token = Cookie.get(UserIdKey);
        console.log('cookie ' + token);
        let user = null;
        if (token) {
            formData.user = { id: token };
        }

        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Access-Control-Allow-Origin', '*');
        this.http.post('/api/links', formData, new RequestOptions({
            headers: this.headers
        })).subscribe(responce => {
            let result = responce.json();
            if (!token) {
                Cookie.set(UserIdKey, result.user.id);
            }
            console.log(result);
        });

        console.log(this.sourceLink.valid + '; you submitted value: ', formData);
    }
}
