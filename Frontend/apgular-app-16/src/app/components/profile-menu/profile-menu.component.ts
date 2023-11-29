import { Component } from '@angular/core';

@Component({
  selector: 'app-profile-menu',
  templateUrl: './profile-menu.component.html',
  styleUrls: ['./profile-menu.component.css']
})
export class ProfileMenuComponent {
  newLogin: string = '';
  newEmail: string = '';
  errorMessageUsername = '';
  errorMessagePassword = '';
  errorMessageEmail = '';
  // Методы для обработки событий


  isUsernameValid(field: string): boolean {
    const value = this[field as keyof ProfileMenuComponent];
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


  isEmailValid(field: string): boolean {
    const value = this[field as keyof ProfileMenuComponent];
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

  editLogin() {
  }

  changePassword() {
  }

  editEmail() {
  }

  // Другие методы, если необходимо

}
