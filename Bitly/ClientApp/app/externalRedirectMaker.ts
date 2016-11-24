import { Injectable } from '@angular/core';

function redirect(link) {
    console.log('!!!!!' + link);
    //window.location.href = link;   
}

@Injectable()
export class ExternalRedirectMaker {
    public MakeRedirerct(link): void {
        redirect(link);
    }
}