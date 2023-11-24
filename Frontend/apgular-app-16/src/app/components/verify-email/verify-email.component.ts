import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.css']
})
export class VerifyEmailComponent {
  verificationCode = '';
  showResendButton = false;
  cooldownTimer = 60;
  timerInterval: any;
  email!: string; // Use definite assignment assertion
  password!: string; // Use definite assignment assertion
  login!: string; // Use definite assignment assertion

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'];
      this.password = params['password'];
      this.login = params['login'];
    });
  }

  enterCode() {
    const body = { email: this.email, code: this.verificationCode };
    console.log(body);
  
    this.http.post('http://localhost:5092/Auth/verifyEmail', body).subscribe(
      (response: any) => {
        alert('Mail successfully confirmed');
        this.createUser();
        this.router.navigate(['/'],);
      },
      (error) => {
        if (error.status === 200) {
          alert('Mail successfully confirmed');
          this.createUser();
          this.router.navigate(['/'],);
        } 
        else if (error.status === 400 || error.status === 404) {
          alert('Wrong code');
          this.createUser();
        }else {
          alert('An unexpected error occurred. Please check the console for details');
        }
        console.error('Verification error:', error);
      }
    );
  }

  createUser(){
    const userBody = {
      login: this.login,
      password: this.password,
      email: this.email
    };
    console.log(userBody);

    this.http.post('http://localhost:5092/User/users', userBody).subscribe(
      (userResponse: any) => {
        console.log('User creation successful:', userResponse);
      },
      (userError) => {
        console.error('User creation error:', userError);
      }
    );
  }
  

  resend() {
    // Implement resend logic if needed
    // You can use the HttpClient.post method to send a new verification code
  }

  private startCooldownTimer() {
    // Implement cooldown timer logic if needed
  }
}
