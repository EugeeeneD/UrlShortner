import { HttpClient, HttpErrorResponse, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ShortenedUrls } from '../models/ShortenedUrl';
import { Observable, Subject, tap } from 'rxjs';
import { Toast, ToastrModule, ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl: string = "https://localhost:7026/urls/";

  constructor(private http: HttpClient, private toast: ToastrService) { }

  headers = new HttpHeaders().set('Access-Control-Request-Method', 'POST');
  options = { withCredentials: true };

  private _refresh$ = new Subject<void>;

  get Refresh$() {
    return this._refresh$;
  }

  getShortenedUrls(): Observable<ShortenedUrls[]> {
    return this.http.get<ShortenedUrls[]>(this.baseUrl);
  }

  deleteShortenedUrl(shortenedUrl: any) {
    return this.http.delete(this.baseUrl + shortenedUrl.id).pipe(tap({
      next: () => {
        this._refresh$.next();
      },
      error: (e: HttpErrorResponse) => {
        if (e.status === 405) {
          this.toast.error("Url already shortened.");
        } else {
          this.toast.error(e.message);
        }
      }
    })
    ).subscribe((res) => {
      console.log(res)
    });
  }

  getShortenedUrl(id: string) {
    return this.http.get<ShortenedUrls>(this.baseUrl + id);
  }

  addUrl(urlToAdd: any) {
    this.http.post<any>(this.baseUrl, urlToAdd).pipe(tap({
      next: () => {
        console.log(1)
        this._refresh$.next();
      }
    })
    ).subscribe((res) => {
      console.log(res)
    });
  }

  getUrlInfo(id: string) {
    return this.http.get(this.baseUrl + id);
  }
}
