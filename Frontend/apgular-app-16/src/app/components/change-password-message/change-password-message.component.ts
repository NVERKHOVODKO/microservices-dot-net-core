import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-password-message',
  templateUrl: './change-password-message.component.html',
  styleUrls: ['./change-password-message.component.css']
})
export class ChangePasswordMessageComponent {

  constructor(private router: Router) { }


  goToLogin(){
    this.router.navigate(['/']);
  }
}
