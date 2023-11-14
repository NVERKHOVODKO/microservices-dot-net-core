// product-list.component.ts

import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    // Возможно, в реальном приложении вы будете получать данные с сервера
    // через сервис или HTTP-запросы. В данном примере данные просто хардкодятся.
  }

  getProducts() {
    const url = 'http://localhost:5122/Product/products';

    this.http.get(url).subscribe(
      (response: any) => {
        this.products = response.$values;
      },
      error => {
        console.error('Error:', error);
      }
    );
  }

  editProduct(productId: string) {
    // Здесь вы можете добавить логику для редактирования продукта
    console.log('Edit Product with ID:', productId);
  }
}
