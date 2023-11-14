// login.component.ts

import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  constructor(private http: HttpClient) {}

  login() {
    const url = 'http://localhost:5187/gateway/login';
    const data = { login: this.username, password: this.password };

    this.http.post(url, data).subscribe(
      (response: any) => {
        if (response && response.token) {
          const token = response.token;
          console.log('Token: ', token);
        } else {
          console.error('Token not found in response.');
        }
      },
      error => {
        console.error('Error:', error);
      }
    );
  }
}
