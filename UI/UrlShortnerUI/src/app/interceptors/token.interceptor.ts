import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth: AuthService, private toast: ToastrService, private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.auth.getToken();

    if (token){
      request = request.clone({
        setHeaders: {Authorization: `Bearer ${token}`},
        withCredentials: true
      });
    }

    return next.handle(request).pipe(
      catchError((e: HttpErrorResponse) => {
        if (e instanceof HttpErrorResponse)
        {
          if (e.status === 401)
          {
            this.toast.warning("Check credentials and try again", "Warning");
            this.router.navigate(['login']);
          }else if (e.status === 405)
          {
            this.toast.error("Object already exist.");
          } else if (e.status === 400){
            this.toast.error(e.error[Object.keys(e.error)[0]]);
          }
        } else{
          this.toast.error("Error occured");
          return throwError(() => new Error("Error occured."));
        }
        return throwError(() => new Error("Error occured."));
      })
    );
  }
}
