import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router, private toast: ToastrService) {}

  canActivate(): boolean {
    if (this.auth.isLoggedIn()){
      return true;
    } else {
      this.toast.warning('Login first', "Unauthorized");
      this.router.navigate(['/login']);
      return false;
    }
  }
  
}
