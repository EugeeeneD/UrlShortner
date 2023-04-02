import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import ValidateForm from 'src/app/helper/validateForm';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  loginForm!: FormGroup;

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  constructor(private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private toast: ToastrService) {

  }

  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";
  }

  onLogin() {
    if (this.loginForm.valid) {
      this.auth.logIn(this.loginForm.value)
        .subscribe(
          {
            next: (res) => {
              this.loginForm.reset();
              this.auth.storeToken(res.token);
              this.router.navigate(['home']);
            }
          }
        )
    } else {
      ValidateForm.validateAllFormField(this.loginForm)
    }
  }

  isAuthorized(): boolean {
    return this.auth.isLoggedIn();
  }

  toAbout(){
    this.router.navigate(['about']);;
  }

  toHome(){
    this.router.navigate(['home']);
  }
}