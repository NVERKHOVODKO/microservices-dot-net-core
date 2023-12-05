import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-user-menu',
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.css']
})
export class UserMenuComponent {
  users: any[] = [];
  token: string | undefined;
  sortedColumn: string | null = null;
  sortDirection: string = 'asc';

  constructor(private route: ActivatedRoute, private http: HttpClient, private router: Router) {}


  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
    });
    this.getUsers();
  }

  getUsers() {
    const apiUrl = 'http://localhost:5187/gateway/users';

    this.http.get(apiUrl).subscribe(
      (response: any) => {
        this.users = response.$values; 
      },
      (error) => {
        console.error('Error during request:', error);
      }
    );
  }

  createUser(){

  }

  goToProducts(){
    this.router.navigate(['/product-menu'], { queryParams: { token: this.token }});
  }


  editUser(user: any){

  }

  deleteUser(userId: string){
    const apiUrl = 'http://localhost:5187/gateway/users/' + userId;

    this.http.delete(apiUrl).subscribe(
      (response: any) => {
        this.users = response.$values; 
      },
      (error) => {
        console.error('Error during request:', error);
      }
    );
    this.getUsers();
  }

  sortTable(column: string): void {
    if (column === this.sortedColumn) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortedColumn = column;
      this.sortDirection = 'asc';
    }

    this.users.sort((a, b) => {
      const aValue = a[column];
      const bValue = b[column];

      if (aValue < bValue) {
        return this.sortDirection === 'asc' ? -1 : 1;
      } else if (aValue > bValue) {
        return this.sortDirection === 'asc' ? 1 : -1;
      } else {
        return 0;
      }
    });
  }
}
