import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { HttpHeaders } from '@angular/common/http';

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

  constructor(private http: HttpClient, private router: Router) { }

  isUsernameValid(field: string): boolean {
    const value = this[field as keyof RegisterComponent];
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


  verifyEmail() {

    if (this.password !== this.confirmPassword) {
      console.error('Passwords do not match');
      alert('Passwords do not match');
      return;
    }

    if (this.email.length < 4) {
      return;
    }

    if (this.password.length < 4) {
      return;
    }

    if (this.username.length < 4) {
      return;
    }

    const userData = {
      login: this.username,
      password: this.password,
      email: this.email,
    };
    console.log('userData: ', userData);
    this.http.post("http://localhost:5187/gateway/send-verification-code", { email: this.email }).subscribe(
      (response: any) => {
        if (response && response.status === "Sended") {
          console.log('Registration successful:', response);
          this.router.navigate(['/verify-email'], { queryParams: { userData } });
        } else {
          console.error('Unexpected response:', response);
          alert('An unexpected response occurred. Please check the console for details.');
        }
      },
      (error) => {
        console.error('HTTP error:', error);
        if (error.status === 200) {
          this.router.navigate(['/verify-email'], { queryParams: { login: this.username, password: this.password, email: this.email} });
        }
        else if (error.status === 400) {
          alert(error.error.message);
        }else if (error.status === 404) {
          alert(error.error.message);
        } else {
          alert('An unexpected error occurred. Please check the console for details.');
        }
      }
    );
  }
}