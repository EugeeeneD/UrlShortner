import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import ValidateForm from 'src/app/helper/validateForm';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  signupForm!: FormGroup;

  ngOnInit(): void{
    this.signupForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', Validators.required]
    });
  }

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router, private toast: ToastrService) { }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";
  }

  onSignup(){
    if (this.signupForm.valid){
      this.auth.signUp(this.signupForm.value)
      .subscribe(
        {
          next: (res =>{
            this.signupForm.reset();
            this.toast.success("Грац", "Registred");
            console.log("response", res);
            this.router.navigate(['login']);
          })
        }
      )
    }
    else{
      ValidateForm.validateAllFormField(this.signupForm)
      }
    }

  toAbout(){
    this.router.navigate(['about']);;
  }

  toHome(){
    this.router.navigate(['home']);
  }
}
