import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-menu',
  templateUrl: './product-menu.component.html',
  styleUrls: ['./product-menu.component.css']
})
export class ProductMenuComponent implements OnInit {
  products: any[] = [];
  token: string | undefined;
  userId: string | undefined;
  showCreateProductPopup: boolean = false;
  showEditProductPopup: boolean = false;
  selectedProductId: string | null = null;  
  editedProduct: any = { name: '', description: '', price: 0, availability: false };
  newProduct: any = {
    name: '',
    description: '',
    price: 0
  };

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
    });
    const helper = new JwtHelperService();
    if (this.token) {
      const decodedToken = helper.decodeToken(this.token);
      this.userId = decodedToken.id;
      console.log('Decoded Token:', decodedToken);
    } else {
      console.error('Token is undefined or null');
    }
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

  createProduct() {
    // Показываем всплывающее окно для создания продукта
    this.showCreateProductPopup = true;
  }

  saveProduct() {
    const url = 'http://localhost:5187/gateway/products';
      const productData = {
      name: this.newProduct.name,
      description: this.newProduct.description,
      price: this.newProduct.price,
      availability: this.newProduct.availability,
      creatorId: this.userId
    };
  
    const headers = {
      'Authorization': `Bearer ${this.token}`
    };
    this.http.post(url, productData, { headers, observe: 'response' }).subscribe(
      (response: any) => {
        console.log(response);
        console.log('Product created successfully:', response);
        alert('Product created successfully!');
      },
      error => {
        console.error('Error:', error);
        alert('An error occurred while creating the product.');
      }
    );
  
    this.showCreateProductPopup = false;
    this.newProduct = { name: '', description: '', price: 0, availability: false };
    this.getProducts();
  }


  closeCreateProductPopup() {
    // Закрываем всплывающее окно без сохранения
    this.showCreateProductPopup = false;
    // Очищаем поля нового продукта
    this.newProduct = { name: '', description: '', price: 0, availability: false };
  }


  deleteProduct(productId: string) {
    const url = `http://localhost:5187/gateway/products/${productId}`;
    const headers = {
      'Authorization': `Bearer ${this.token}`
    };

    this.http.delete(url, { headers, observe: 'response' }).subscribe(
      (response: any) => {
        if (response && response.$values) {
          // Успешное удаление продукта
          console.log('Ответ:', response);
          this.products = response.$values;
          alert('Product deleted successfully!');
        } else {
          console.error('Unexpected response:', response);
          alert('An unexpected error occurred while deleting the product.');
        }
      },
      error => {
        console.error('Error:', error);

        if (error.status === 403) {
          alert('Access forbidden. You do not have permission to delete this product.');
        } else if (error.status === 401) {
          alert('Unauthorized. Please log in and try again.');
        } else if (error.status === 404) {
          alert('Product not found. It may have been deleted by someone else.');
        } else {
          alert('An error occurred while deleting the product.');
        }
      }
    );
    this.getProducts();
  }


  editProduct(productId: string) {
    console.log('Edit Product with ID:', productId);

    this.selectedProductId = productId;
    this.showEditProductPopup = true;

    const selectedProduct = this.products.find(p => p.id === productId);
    if (selectedProduct) {
      this.editedProduct = { ...selectedProduct };
    }
  }

  saveEditedProduct() {
    // TODO: Добавить логику сохранения изменений на сервере

    this.closeEditProductPopup();
  }

  closeEditProductPopup() {
    this.showEditProductPopup = false;
    this.selectedProductId = null;
    this.editedProduct = { name: '', description: '', price: 0, availability: false };
  }

  goBack(){
    this.router.navigate(['/']);
  }
}
