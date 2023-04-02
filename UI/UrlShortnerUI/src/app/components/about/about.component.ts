import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { NgModule } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {

  // could be easily obtained from web api
  description: string = "Algorithm: on serverside each url gets GUID(globally unique identifier) after this you can navigete throw /navigate/ endpoint just with eding shortened site guid.";
  form!: FormGroup;
  constructor(private auth: AuthService, private toast: ToastrService, private router: Router){}

  ngOnInit(): void {
    this.form = new FormGroup({
      description: new FormControl(this.description)
    });

    this.form.get('description')?.valueChanges.subscribe((res) =>{
      this.description = res.description;
    });
  }

  onSubmit(){
    this.description = this.form.value.description;
  }

  isAdmin(): boolean{
    return this.auth.isAdmin();
  }

  logOut() {
    this.auth.signOut();
  }

  isVisible(urlObj: any) {
    if (this.isAuthorized()) {
      return this.isAdmin();
    }
    return false;
  }

  isAuthorized(): boolean {
    return this.auth.isLoggedIn();
  }

  logInRedirect() {
    this.router.navigate(['login']);
  }

  toAbout(){
    this.router.navigate(['about']);;
  }

  toHome(){
    this.router.navigate(['home']);
  }
}
