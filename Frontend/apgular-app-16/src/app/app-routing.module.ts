import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ProductMenuComponent } from './components/product-menu/product-menu.component';
import { NotFoundComponent } from './components/not-found/not-found.component';


const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'product-menu', component: ProductMenuComponent },
  { path: '**', component:  NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
