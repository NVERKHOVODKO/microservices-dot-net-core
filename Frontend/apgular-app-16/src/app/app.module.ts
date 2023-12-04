// app.module.ts

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'; // добавлено

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { UserMenuComponent } from './components/user-menu/user-menu.component';
import { AdminMenuComponent } from './admin-menu/admin-menu.component';
import { ProductMenuComponent } from './components/product-menu/product-menu.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './components/register/register.component';
import { VerifyEmailComponent } from './components/verify-email/verify-email.component';
import { ProfileMenuComponent } from './components/profile-menu/profile-menu.component';
import { ChangePasswordMessageComponent } from './components/change-password-message/change-password-message.component';
import { RestorePasswordComponent } from './components/restore-password/restore-password.component';


const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'product-menu', component: ProductMenuComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'change-password-message', component: ChangePasswordMessageComponent },
  { path: 'restore-password', component: RestorePasswordComponent },
  { path: 'profile-menu', component: ProfileMenuComponent },
  { path: 'verify-email', component: VerifyEmailComponent },
  { path: '**', component:  NotFoundComponent}
];


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    UserMenuComponent,
    AdminMenuComponent,
    RegisterComponent,
    ProductMenuComponent,
    NotFoundComponent,
    VerifyEmailComponent,
    ProfileMenuComponent,
    ChangePasswordMessageComponent,
    RestorePasswordComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    RouterModule.forRoot(routes),
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
