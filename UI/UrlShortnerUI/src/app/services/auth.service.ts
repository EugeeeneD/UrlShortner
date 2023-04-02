import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http"
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import jwt_decode from 'jwt-decode';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl: string = "https://localhost:7026/api/UserAuthentication/";
  
  private roleClaimName = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

  constructor(private http: HttpClient, private router: Router, private cookieService: CookieService) { }

  signUp(registerUser: any){
    return this.http.post<any>(`${this.baseUrl}register`, registerUser);
  }

  logIn(loginUser: any){
    return this.http.post<any>(`${this.baseUrl}login`, loginUser);
  }

  signOut(){
    localStorage.removeItem('jwt');
    this.cookieService.delete('jwt');
    this.router.navigate(['login']);
  }

  storeToken(tokenValue: string){
    localStorage.setItem('jwt', tokenValue);
    this.cookieService.set('jwt', tokenValue);
  }

  getToken(){
    return localStorage.getItem('jwt');
  }

  isLoggedIn(): boolean{
    return !!localStorage.getItem('jwt');
  }

  getUserIdFromToken(): any {
    var token = this.getToken();
    if(token === null){
      return this.router.navigate(['login']);
    }

    const decodedToken: any = jwt_decode(token);
    return decodedToken.id;
  }

  getUserRoleFromToken(): any {
    var token = this.getToken();
    if(token === null){
      return this.router.navigate(['login']);
    }

    const decodedToken: any = jwt_decode(token);
    return decodedToken[this.roleClaimName];
  }

  isAdmin(): boolean{
    var token = this.getToken();
    if(token === null){
      return false;
    }

    const decodedToken: any = jwt_decode(token);
    return decodedToken[this.roleClaimName] === "admin";
  }
}
