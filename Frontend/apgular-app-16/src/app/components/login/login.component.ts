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

  constructor(private http: HttpClient, private router: Router) {}

  login() {
    if(this.password == ""){
      alert("Enter password");
      return;
    }

    if(this.username == ""){
      alert("Enter password");
      return;
    }

    const url = 'http://localhost:5187/gateway/login';
    const data = { login: this.username, password: this.password };

    this.http.post(url, data).subscribe(
      (response: any) => {
        if (response && response.token) {
          console.log('Request: ', Request);
          const token = response.token;
          console.log('response: ', response);
          this.router.navigate(['/product-menu'], { queryParams: { token: token }});
        } else {
          console.error('Token not found in response.');
        }
      },
      error => {
        console.error('HTTP error:', error);
        alert(error.error.message);
       }
    );
  }
}