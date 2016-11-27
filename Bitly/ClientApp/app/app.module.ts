import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UniversalModule } from 'angular2-universal';
import { AppComponent } from './components/app/app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { RedirectComponent } from './components/redirect/redirect.component';
import { ExternalRedirectMaker } from './ExternalRedirectMaker';
import { Configuration } from './app.constants';
import { DataService } from './services/dataService';

@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        RedirectComponent,
        FetchDataComponent,
        HomeComponent
    ],
    providers: [
        DataService,
        ExternalRedirectMaker,
        Configuration
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        FormsModule,
        ReactiveFormsModule,
        
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            //{ path: 'counter', component: RedirectComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: '**', component: RedirectComponent }
        ])
    ]
})
export class AppModule {
}
