import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { CookieService } from 'ngx-cookie-service';
import { JwtHelperService } from '@auth0/angular-jwt';



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
  showEditUserPopup: boolean = false;
  roles: string[] = [];
  editingUser: any = {
    id: '',
    name: '',
    email: '',
    roles: []
  };
  user: any = {
    id: '',
    name: '',
    email: '',
    roles: []
  };

  constructor(private cookieService: CookieService, private route: ActivatedRoute, private http: HttpClient, private router: Router) { }


  ngOnInit() {
    /* this.route.queryParams.subscribe(params => {
      this.token = params['token'];
    }); */
    this.token = this.cookieService.get('userToken');
    const helper = new JwtHelperService();
    if (this.token) {
      const decodedToken = helper.decodeToken(this.token);

      this.user.id = decodedToken.id;
      this.user.name = decodedToken.name;
      this.user.email = decodedToken.email;
      if (decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']) {
        this.user.roles = Array.isArray(decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'])
          ? decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
          : [decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']];

        console.log('User Roles:', this.user.roles);
      } else {
        console.error('Roles not found in the token.');
      }

      console.log('Decoded Token:', decodedToken);
    } else {
      console.error('Token is undefined or null');
    }
    this.getUsers();
  }


  editRoles(): void {
    console.log('Selected Roles:', this.roles);
    const userId = this.editingUser.id;

    const newRoles = this.roles.filter(role => !this.editingUser.roleNames.$values.includes(role));

    newRoles.forEach(role => {
      this.http.post('http://localhost:5092/User/users/add-role', { userId, roleName: role })
        .subscribe(
          (response) => {
            console.log(`Role '${role}' added successfully for user ${userId}`);
            this.updateToken();
          },
          (error) => {
            alert(error.message);
            console.error(`Error adding role '${role}' for user ${userId}:`, error);
          }
        );
    });
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

  createUser() {

  }

  openEditUserPopup(user: any) {
    const apiUrl = `http://localhost:5092/User/users/${user.id}`;
    this.http.get(apiUrl).subscribe(
      (response: any) => {
        this.editingUser = { ...response };
        this.showEditUserPopup = true;
        this.activateRoleCheckboxes();
      },
      (error) => {
        console.error('Error during request:', error);
      }
    );
  }
  
  activateRoleCheckboxes() {
    if (this.editingUser && this.editingUser.roleNames) {
      const roles: string[] = this.editingUser.roleNames.$values;
      roles.forEach((role: string) => {
        if (!this.roles.includes(role)) {
          this.setRole(role, true);
        }
      });
    }
  }
  
  setRole(role: string, isActive: boolean = true): void {
    if (isActive) {
      this.roles.push(role);
    } else {
      this.roles = this.roles.filter(r => r !== role);
    }
  }
  

  closeEditUserPopup() {
    this.showEditUserPopup = false;
    this.editingUser = {};
  }

  saveUser() {
    this.closeEditUserPopup();
  }


  goToProducts() {
    this.router.navigate(['/product-menu']);
  }

  updateToken() {
    const requestBody = {
      id: this.user.id
    };

    this.http.post<any>('http://localhost:5187/gateway/get-user-token', requestBody)
      .subscribe(
        (response) => {
          if (response && response.token) {
            this.cookieService.set('token', response.token, { expires: 1 });
            console.log('Token updated successfully:', response.token);
          } else {
            console.error('Invalid token response:', response);
          }
        },
        (error) => {
          console.error('Error fetching token:', error);
        }
      );
  }


  editUser(user: any) {

  }

  deleteUser(userId: string) {
    const apiUrl = `http://localhost:5187/gateway/users?deleterId=${this.user.id}&deletedId=${userId}`;

    this.http.delete(apiUrl).subscribe(
      (response: any) => {
        this.users = response.$values;
      },
      (error) => {
        alert(error.message);
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
