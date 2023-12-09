import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.css']
})
export class VerifyEmailComponent implements OnInit, OnDestroy {
  verificationCode = '';
  showResendButton = false;
  cooldownTimer = 5;
  timerInterval: any;
  email!: string;
  password!: string;
  login!: string;
  resendCooldown = false;

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'];
      this.password = params['password'];
      this.login = params['login'];
    });

    this.startCooldownTimer();
  }

  ngOnDestroy() {
    clearInterval(this.timerInterval);
  }

  enterCode() {
    const body = { email: this.email, code: this.verificationCode };
    console.log(body);

    this.http.post('http://localhost:5187/gateway/verify-email', body).subscribe(
      (response: any) => {
        alert('Mail successfully confirmed');
        this.createUser();
        this.router.navigate(['/']);
      },
      (error) => {
        if (error.status === 200) {
          alert('Mail successfully confirmed');
          this.createUser();
          this.router.navigate(['/']);
        } else if (error.status === 400 || error.status === 404) {
          alert('Wrong code');
        } else {
          alert('An unexpected error occurred. Please check the console for details');
        }
        console.error('Verification error:', error);
      }
    );
  }

  createUser() {
    const userBody = {
      login: this.login,
      password: this.password,
      email: this.email
    };
    console.log(userBody);

    this.http.post('http://localhost:5187/gateway/users', userBody).subscribe(
      (userResponse: any) => {
        console.log('User creation successful:', userResponse);
      },
      (userError) => {
        console.error('User creation error:', userError);
      }
    );
  }

  resend() {
    if (!this.resendCooldown) {
      alert('Sent');
      this.resendCooldown = true;

      this.http.post('http://localhost:5187/gateway/send-verification-code', { email: this.email }).subscribe(
        (response: any) => { },
        (error) => {
          if (error.status === 200) {
          } else if (error.status === 400) {
            alert(error.error.message);
          } else if (error.status === 404) {
            alert(error.error.message);
          } else {
            alert('An unexpected error occurred. Please check the console for details.');
          }
        }
      );
      this.showResendButton = false;
      this.resetCooldownTimer();
    }
  }

  resetCooldownTimer() {
    this.cooldownTimer = 60;
    this.showResendButton = false;
    this.timerInterval = setInterval(() => {
      if (this.cooldownTimer > 0) {
        this.cooldownTimer--;
      } else {
        clearInterval(this.timerInterval);
        this.showResendButton = true;
        this.resendCooldown = false;
      }
    }, 1000);
  }


  private startCooldownTimer() {
    this.timerInterval = setInterval(() => {
      if (this.cooldownTimer > 0) {
        this.cooldownTimer--;
      } else {
        clearInterval(this.timerInterval);
        this.showResendButton = true;
      }
    }, 1000);
  }
}
