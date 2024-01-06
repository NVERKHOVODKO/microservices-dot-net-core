import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { ProductMenuComponent } from './components/product-menu/product-menu.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './components/register/register.component';
import { VerifyEmailComponent } from './components/verify-email/verify-email.component';
import { ProfileMenuComponent } from './components/profile-menu/profile-menu.component';
import { RestorePasswordComponent } from './components/restore-password/restore-password.component';
import { VerifyPasswordRecordingComponent } from './components/verify-password-recording/verify-password-recording.component';
import { UserMenuComponent } from './components/user-menu/user-menu.component';


const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'product-menu', component: ProductMenuComponent },
  { path: 'user-menu', component: UserMenuComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile-menu', component: ProfileMenuComponent },
  { path: 'restore-password', component: RestorePasswordComponent },
  { path: 'verify-email', component: VerifyEmailComponent },
  { path: 'verify-password-recoverding', component: VerifyPasswordRecordingComponent },
  { path: '**', component:  NotFoundComponent}
];


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    ProductMenuComponent,
    NotFoundComponent,
    VerifyEmailComponent,
    ProfileMenuComponent,
    RestorePasswordComponent,
    VerifyPasswordRecordingComponent,
    UserMenuComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
