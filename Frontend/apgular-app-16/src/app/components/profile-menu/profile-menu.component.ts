import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile-menu',
  templateUrl: './profile-menu.component.html',
  styleUrls: ['./profile-menu.component.css']
})
export class ProfileMenuComponent {
  newLogin: string = '';
  newEmail: string = '';
  errorMessageUsername = '';
  errorMessagePassword = '';
  errorMessageEmail = '';
  token: string | undefined;
  userName = '';
  email = '';
  roles = []
  user: any = {
    id: '',
    name: '',
    email: '',
    roles: []
  };

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) { }


  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
    });
    const helper = new JwtHelperService();
    if (this.token) {
      const decodedToken = helper.decodeToken(this.token);
      this.user.id = decodedToken.id;
      this.user.name = decodedToken.name;
      this.user.email = decodedToken.email;
      if (decodedToken.hasOwnProperty('http://schemas.microsoft.com/ws/2008/06/identity/claims/role')
        && Array.isArray(decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'])) {
        this.user.roles = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        console.log('User Roles:', this.user.roles);
      } else {
        console.error('Roles not found or not an array in the token.');
      }
      console.log('Decoded Token:', decodedToken);
    } else {
      console.error('Token is undefined or null');
    }
    this.roles = this.user.roles;
    this.email = this.user.email;
    this.userName = this.user.name;
    this.updateUserInfo();
  }

  isUsernameValid(field: string): boolean {
    const value = this[field as keyof ProfileMenuComponent];
    if (value === undefined) {
      return false;
    }
    if (value.length > 20) {
      this.errorMessageUsername = 'Username must be less than 20 characters';
      return true;
    }
    if (value.length < 4) {
      this.errorMessageUsername = 'Username must be more than 4 characters';
      return true;
    }
    const isValid = !(value === '' || value === undefined || value === null);
    if (!isValid) {
      this.errorMessageUsername = 'Username is required';
    } else {
      this.errorMessageUsername = '';
    }
    return !isValid;
  }


  isEmailValid(field: string): boolean {
    const value = this[field as keyof ProfileMenuComponent];
    if (value === undefined) {
      return false;
    }
    if (value.length > 100) {
      this.errorMessagePassword = 'Email must be less than 100 characters';
      return true;
    }
    const isValid = !(value === '' || value === undefined || value === null);
    if (!isValid) {
      this.errorMessageEmail = 'Email is required';
    } else {
      this.errorMessageEmail = '';
    }
    return !isValid;
  }

  updateUserInfo(){
    const url = `http://localhost:5187/gateway/users/${this.user.id}`;
    console.log(url);
    console.log(this.user.id);
    const headers = {
      'Authorization': `Bearer ${this.token}`
    };

    this.http.get(url, { headers, observe: 'response' }).subscribe(
      (response: any) => {
        console.log(response);
        this.userName = response.body.login;
      },
      error => {
        if (error.status === 403) {
          alert('Access forbidden. You do not have permission to delete this product.');
        } else if (error.status === 401) {
          alert('Unauthorized. Please log in and try again.');
        } else {
          alert(error.status.message);
        }
      }
    );
  }


  editLogin() {
    const url = 'http://localhost:5092/User/users/editLogin';

    const headers = {
      'Authorization': `Bearer ${this.token}`
    };

    const body = {
      userId: this.user.id,
      newLogin: this.newLogin,
    };
    console.log(body);
    this.http.patch(url, body, { headers, observe: 'response' }).subscribe(
      (response: any) => {
        console.log(response);
        this.updateUserInfo();
        alert("Login edited");
      },
      error => {
        if (error.status === 403) {
          alert('Access forbidden. You do not have permission to delete this product.');
        } else if (error.status === 401) {
          alert('Unauthorized. Please log in and try again.');
        } else {
          alert('An error occurred while deleting the product.');
        }
      }
    );
  }

  changePassword() {
  }

  editEmail() {
  }


  goToMainMenu() {
    this.router.navigate(['/product-menu'], { queryParams: { token: this.token } });
  }

}
