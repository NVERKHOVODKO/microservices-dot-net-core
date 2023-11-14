import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { UserMenuComponent } from './components/user-menu/user-menu.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'userNemu', component: UserMenuComponent },
  // Другие маршруты могут быть добавлены здесь
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
