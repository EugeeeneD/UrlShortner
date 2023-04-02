import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { NavigationExtras, Router } from '@angular/router';
import ValidateForm from 'src/app/helper/validateForm';
import { ShortenedUrls } from 'src/app/models/ShortenedUrl';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {

  @ViewChild(MatPaginator) paginator !: MatPaginator;

  public shortenedUrls: any[] = [];
  public observable$: any;
  displayedColumns: string[] = ['LongUrl', 'Shortened', 'Action'];
  dataSource: any;

  createUrlForm!: FormGroup;

  constructor(private auth: AuthService, private api: ApiService, private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {

    this.api.Refresh$.subscribe(() => {
      this.getUrls()
    })

    this.getUrls();
    this.createUrlForm = this.fb.group({
      Url: ['', Validators.required],
    });
  }

  private getUrls() {
    this.api.getShortenedUrls()
      .subscribe(res => {
        this.shortenedUrls = res;
        this.dataSource = new MatTableDataSource(this.shortenedUrls);
        this.dataSource.paginator = this.paginator;
      });
  }

  logOut() {
    this.auth.signOut();
  }

  addNewUrl() {
    if (this.createUrlForm.value) {
      this.api.addUrl(this.createUrlForm.value);
      this.createUrlForm.reset();
    } else {
      ValidateForm.validateAllFormField(this.createUrlForm)
    }
  }

  deleteShortenedUrl(row: any) {
    this.api.deleteShortenedUrl(row);
  }

  getUrlInfo(row: any) {
    this.router.navigate(['url-info'], { queryParams: { data: JSON.stringify(row) } });
  }

  isAuthorized(): boolean {
    return this.auth.isLoggedIn();
  }

  logInRedirect() {
    this.router.navigate(['login']);
  }

  isVisible(urlObj: any) {
    if (this.isAuthorized()) {
      if (this.isAdminOrOwner(urlObj)) {
        return true;
      }
    }

    return false;
  }

  isAdminOrOwner(urlObj: any) {
    var role = this.auth.getUserRoleFromToken();
    var id = this.auth.getUserIdFromToken();

    if (role === "admin") {
      return true;
    }

    if (id === urlObj.userId) {
      return true;
    }

    return false;
  }

  toAbout(){
    this.router.navigate(['about']);;
  }

  toHome(){
    this.router.navigate(['home']);
  }
}
