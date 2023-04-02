import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-url-info',
  templateUrl: './url-info.component.html',
  styleUrls: ['./url-info.component.css']
})
export class UrlInfoComponent implements OnInit {
  user: any;
  displayedColumns: string[] = ['Id', 'LongUrl', 'Shortened Url', 'Date of creation', 'Owner id'];
  dataSource: any;

constructor(private auth: AuthService, 
  private activatedRoute: ActivatedRoute,
  private router: Router) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe((res:any) => {
      this.user = JSON.parse(res.data);
      this.dataSource = new MatTableDataSource([this.user]);
    });
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
