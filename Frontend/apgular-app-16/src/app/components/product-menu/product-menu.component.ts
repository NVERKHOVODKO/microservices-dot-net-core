import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-product-menu',
  templateUrl: './product-menu.component.html',
  styleUrls: ['./product-menu.component.css']
})
export class ProductMenuComponent implements OnInit {
  products: any[] = [];
  token: string | undefined;

  constructor(private http: HttpClient, private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
    });

    //this.getProducts();
  }

  getProducts() {
    const url = 'http://localhost:5187/gateway/products';
      const headers = {
      'Authorization': `Bearer ${this.token}`
    };
  
    this.http.get(url, { headers }).subscribe(
      (response: any) => {
        this.products = response.$values;
      },
      error => {
        console.error('Error:', error);
      }
    );
  }


  deleteProduct(productId: string) {
    const url = `http://localhost:5122/Product/products/${productId}`;
    const headers = {
      'Authorization': `Bearer ${this.token}`
    };
    this.http.delete(url, { headers }).subscribe(
      (response: any) => {
        this.products = response.$values;
      },
      error => {
        console.error('Error:', error);
      }
    );
  }
  

  createProduct() {
    console.log('Token:', this.token);
  }

  editProduct(productId: string) {
    console.log('Edit Product with ID:', productId);
  }
}
