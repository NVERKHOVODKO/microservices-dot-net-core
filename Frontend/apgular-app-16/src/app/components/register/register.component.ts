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

  constructor(private http: HttpClient, private router: Router) {}

  register() {
    const url = 'http://localhost:5187/gateway/users';

    // Check if passwords match
    if (this.password !== this.confirmPassword) {
      console.error('Passwords do not match');
      // You might want to display an error message to the user here
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
        this.router.navigate(['/product-menu'], { queryParams: { token: response.token }});
      },
      (error) => {
        console.error('Registration error:', error);
      }
    );
  }
}
