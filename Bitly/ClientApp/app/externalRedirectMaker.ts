import { Injectable } from '@angular/core';
//var myWindow = window;


function redirect(link) {
    console.log('Should be window.location.href = ' + link);
    //console.log(myWindow.location);
    //window.location.href = link;   
}

@Injectable()
export class ExternalRedirectMaker {
    public MakeRedirerct(link): void {
        redirect(link);
    }
}