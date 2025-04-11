import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common'; // ✅ this is missing
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountservice = inject(AccountService);
  private toastr =inject(ToastrService);
cancelRegister= output<boolean>(); 
model:any ={};

register(){
  this.accountservice.register(this.model).subscribe({
    next:Response=>{
      console.log(Response);
      this.cancel();
    },
    error:error=>this.toastr.error(error.error)
  });
}

cancel(){
  this.cancelRegister.emit(false);
}
}
