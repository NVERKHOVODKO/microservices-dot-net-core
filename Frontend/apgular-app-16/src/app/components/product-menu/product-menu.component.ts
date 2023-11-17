import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-product-menu',
  templateUrl: './product-menu.component.html',
  providers: [DatePipe],
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
    id: '',
    name: '',
    description: '',
    price: 0
  };

  editingProduct: any = {
    id: '',
    name: '',
    description: '',
    price: 0
  };
  errorMessageLogin = '';
  errorMessagePrice = '';
  errorMessageDescription = '';
  errorMessageEditName = '';
  errorMessageEditDescription = '';
  errorMessageEditPrice = '';


  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router, private datePipe: DatePipe) { }

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
    this.getProducts();
  }

  formatDateString(dateString: string | null): string {
    if (dateString === null) {
      return ''; // or any other default value you prefer
    }

    const date = new Date(dateString);
    return this.datePipe.transform(date, 'yyyy-MM-dd HH:mm:ss') || ''; // fallback to empty string if transformation fails
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
        if (error.status === 403) {
          alert('Access forbidden. You do not have permission to delete this product.');
        } else if (error.status === 401) {
          alert('Unauthorized. Please log in and try again.');
        } else {
          alert('An error occurred while deleting the product.');
        }
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
        if (error.status === 403) {
          alert('Access forbidden. You do not have permission to delete this product.');
        } else if (error.status === 401) {
          alert('Unauthorized. Please log in and try again.');
        } else {
          alert('An error occurred while deleting the product.');
        }      }
    );
  
    this.showCreateProductPopup = false;
    this.newProduct = { name: '', description: '', price: 0, availability: false };
    this.getProducts();
  }


  editName(field: string) {
    const value = this.editingProduct[field];
    if(value.length > 30){
      this.errorMessageEditName = 'Name must be less than 30 symbols';
      alert('Name must be less than 30 symbols');
      return;
    }
    const isValid = !(value === '' || value === undefined || value === null);
    if (!isValid) {
        this.errorMessageEditName = 'Name is required';
        alert('Name is required');
        return;
    }
    
    const url = 'http://localhost:5187/gateway/editName';
    
    const headers = {
      'Authorization': `Bearer ${this.token}`
    };
  
    const body = {
      productId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
      userId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
      newName: this.editingProduct[field]
    };
  
    this.http.post(url, body, { headers }).subscribe(
      (response: any) => {
        console.log('Edit Name successful:', response);
        alert('Name edited successfully');
      },
      (error) => {
        console.error('Edit Name error:', error);
      }
    );
  }

  isEditNameValid(field: string): boolean {
    const value = this.editingProduct[field];
    if(value.length > 30){
      this.errorMessageEditName = 'Name must be less than 30 symbols';
      return true;
    }
    const isValid = !(value === '' || value === undefined || value === null);
    if (!isValid) {
        this.errorMessageEditName = 'Name is required';
    } else {
        this.errorMessageEditName = '';
    }
    return !isValid;
  }
  
  isLoginValid(field: string): boolean {
    const value = this.newProduct[field];
    if(value.length > 30){
      this.errorMessageLogin = 'Name must be less than 30 symbols';
      return true;
    }
    const isValid = !(value === '' || value === undefined || value === null);
    if (!isValid) {
        this.errorMessageLogin = 'Name is required';
    } else {
        this.errorMessageLogin = '';
    }
    return !isValid;
  }

  isEditDescriptionValid(field: string): boolean {
    const value = this.editingProduct[field];
    if(value.length > 200){
      this.errorMessageEditDescription = 'Description must be less than 200 symbols';
      return true;
    }
    return false;
  }

  isDescriptionValid(field: string): boolean {
    const value = this.newProduct[field];
    if(value.length > 200){
      this.errorMessageDescription = 'Description must be less than 200 symbols';
      return true;
    }
    return false;
  }

  isEditPriceValid(field: string): boolean {
    const value = this.editingProduct[field];
    const isValid = !(value === '' || value === undefined || value === null);
    if (!isValid) {
        this.errorMessageEditPrice = 'Price is required';
        return true;
    }
    if(value <= 0){
      this.errorMessageEditPrice = "Price must be more than 0";
      return true;
    }
    if(value > 999999999){
      this.errorMessageEditPrice = "Price can't be more than 999999999";
      return true;
    }
    return false;
  }

  isPriceValid(field: string): boolean {
    const value = this.newProduct[field];
    const isValid = !(value === '' || value === undefined || value === null);
    if (!isValid) {
        this.errorMessagePrice = 'Price is required';
        return true;
    }
    if(value <= 0){
      this.errorMessagePrice = "Price must be more than 0";
      return true;
    }
    if(value > 999999999){
      this.errorMessagePrice = "Price can't be more than 999999999";
      return true;
    }
    return false;
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
        console.log('Ответ:', response);
        console.log('Request:', Request);
        this.products = response.$values;
        alert('Product deleted successfully!');
        console.log('Response body:', response.body);  // Add this line
        // Move this.getProducts() inside the success block
        this.getProducts();
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


  goToUsersMenu(){
    this.router.navigate(['/users-menu']);
  }


  goBack(){
    this.router.navigate(['/']);
  }
}
