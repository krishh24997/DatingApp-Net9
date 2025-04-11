import { Component, inject, OnInit } from '@angular/core';
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
export class HomeComponent implements OnInit {
  http = inject(HttpClient);
  registermode=false;
  title = 'Dating App';
  users: any;

  ngOnInit(): void {
    this.getUsers();
  }
  registerToggle(){
    this.registermode= !this.registermode;
  }
  cancelRegisterMode(event:boolean){
    this.registermode=event
  }
 
  getUsers() {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log("Request has completed", Response),
    })
  
  }

}
