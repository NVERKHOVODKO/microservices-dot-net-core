// product-list.component.ts

import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'userMenu',
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.css']
})
export class UserMenuComponent {
  products: any[] = [];
  apiUrl = 'http://localhost:5122/Product/products';

  loadProducts() {
    const token = 'ваш_токен_здесь'; // Замените этот токен на ваш реальный токен
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    this.http.get<any[]>(this.apiUrl, { headers }).subscribe(
      (data: any[]) => {
        this.products = data;
      },
      error => {
        console.error('Error fetching products:', error);
      }
    );
  }

  constructor(private http: HttpClient) {}
}