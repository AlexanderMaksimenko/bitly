import { Injectable } from '@angular/core';
//var myWindow = window;


function redirect(link) {
    //console.log('!!!!!' + link);
    //console.log(myWindow.location);
    //window.location.href = link;   
}

@Injectable()
export class ExternalRedirectMaker {
    public MakeRedirerct(link): void {
        redirect(link);
    }
}