import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {

busyRequestCount= 0;
private spinnerservice=inject(NgxSpinnerService);

busy(){
  this.busyRequestCount++;
  this.spinnerservice.show(undefined,{
    type :'line-scale-party',
    bdColor:'rgba(255,255,255,0)',
    color:'#333333'
  })
}
idle(){
  this.busyRequestCount--;
  if(this.busyRequestCount<=0){
    this.busyRequestCount=0;
    this.spinnerservice.hide();
  }
}
}
