import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent {
  username: string = '';
  password: string = '';
  confirmPassword: string = '';
  email: string = '';
  errorMessageUsername = '';
  errorMessagePassword = '';
  errorMessageEmail = '';
 


  constructor(private http: HttpClient, private router: Router) {}

  isUsernameValid(field: string): boolean {
    const value = this[field as keyof RegisterComponent];  // Use keyof to access the property by name
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


  isPasswordValid(field: string): boolean {
    const value = this[field as keyof RegisterComponent];
    if (value.length > 30) {
      this.errorMessagePassword = 'Password must be less than 30 characters';
      return true;
    }
    if (value.length < 4) {
      this.errorMessagePassword = 'Password must be more than 4 characters';
      return true;
    }
    const isValid = !(value === '' || value === undefined || value === null);
    if (!isValid) {
      this.errorMessagePassword = 'Username is required';
    } else {
      this.errorMessagePassword = '';
    }
    return !isValid;
  }


  isEmailValid(field: string): boolean {
    const value = this[field as keyof RegisterComponent];
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


  register() {
    const url = 'http://localhost:5187/gateway/users';

    if (this.password !== this.confirmPassword) {
      console.error('Passwords do not match');
      alert('Passwords do not match');
      return;
    }

    const body = {
      login: this.username,
      password: this.password,
      email: this.email,
    };

    console.log(this.email);
    console.log(this.username);
    console.log(this.password);

    this.http.post(url, body).subscribe(
      (response: any) => {
        console.log(response);
        console.log('Registration successful:', response);
        const token = response.token;
        console.log(token);
        this.router.navigate(['/product-menu'], { queryParams: { token: token }});
      },
      (error) => {
        console.error('Registration error:', error);
        console.log('Error message:', error.error.message);
        alert(error.error.message);
      }
    );
  }
}
