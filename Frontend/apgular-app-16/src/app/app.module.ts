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


const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'product-menu', component: ProductMenuComponent },
  { path: '**', component:  NotFoundComponent}
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    UserMenuComponent,
    AdminMenuComponent,
    ProductMenuComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    RouterModule.forRoot(routes),
    HttpClientModule // добавлено
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
