import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ProductMenuComponent } from './components/product-menu/product-menu.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { RegisterComponent } from './components/register/register.component';
import { VerifyEmailComponent } from './components/verify-email/verify-email.component';
import { ProfileMenuComponent } from './components/profile-menu/profile-menu.component';


const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'product-menu', component: ProductMenuComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile-menu', component: ProfileMenuComponent },
  { path: 'verify-email', component: VerifyEmailComponent },
  { path: '**', component:  NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
