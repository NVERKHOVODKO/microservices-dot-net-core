import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-restore-password',
  templateUrl: './restore-password.component.html',
  styleUrls: ['./restore-password.component.css']
})
export class RestorePasswordComponent {
  password: string = '';
  confirmPassword: string = '';
  email: string = '';
  constructor(private route: ActivatedRoute, private router: Router, private http: HttpClient) { }

  verifyByEmail() {
    if (this.password !== this.confirmPassword) {
      console.error('Passwords do not match');
      alert('Passwords do not match');
      return;
    }

    if (this.password.length < 4) {
      alert('Passwords can\'t be less than 4 symbols');
      return;
    }

    const userData = {
      email: this.email,
      newPassword: this.password
    };

    console.log(userData);
    const apiUrl = 'http://localhost:5092/Auth/restore-password';


    this.http.post(apiUrl, userData).subscribe(
      (response) => {
        this.router.navigate(['/verify-password-recoverding'], {
          queryParams: { email: this.email }
        });
            },
      (error) => {
        console.error('Error during request:', error);
        alert(error.message)
      }
    );
  }
}
