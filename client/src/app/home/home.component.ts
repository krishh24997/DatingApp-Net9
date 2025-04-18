import { Component } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  registermode = false;
  title = 'Dating App';



  registerToggle() {
    this.registermode = !this.registermode;
  }
  cancelRegisterMode(event: boolean) {
    this.registermode = event
  }



}
