// app.module.ts

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'; // добавлено

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { UserMenuComponent } from './components/user-menu/user-menu.component';
import { AdminMenuComponent } from './admin-menu/admin-menu.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    UserMenuComponent,
    AdminMenuComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule // добавлено
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
