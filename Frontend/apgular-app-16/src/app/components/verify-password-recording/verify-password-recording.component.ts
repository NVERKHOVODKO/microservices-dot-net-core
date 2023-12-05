import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';


@Component({
  selector: 'app-verify-password-recording',
  templateUrl: './verify-password-recording.component.html',
  styleUrls: ['./verify-password-recording.component.css']
})
export class VerifyPasswordRecordingComponent {
  code: string = '';
  email: string = '';

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) { }


  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'];
    });
  }

  confirmPassword() {
    console.log('code:', this.code);
    console.log('email:', this.email);


    const data = {
      email: this.email,
      code: this.code,
    };

    console.log(data);
    const apiUrl = 'http://localhost:5092/Auth/confirm-restore-password';

    this.http.post(apiUrl, data).subscribe(
      (response) => {
        console.log(response);
        this.router.navigate(['/']);
      },
      (error) => {
        console.error('Error during request:', error);
        alert(error.message)
      }
    );

  }
}