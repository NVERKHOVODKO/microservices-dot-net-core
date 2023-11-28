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
  sortedColumn: string | null = null;
  sortDirection: string = 'asc'; // 'asc' or 'desc'

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


  sortTable(column: string): void {
    if (column === this.sortedColumn) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortedColumn = column;
      this.sortDirection = 'asc';
    }

    this.products.sort((a, b) => {
      const aValue = a[column];
      const bValue = b[column];

      if (aValue < bValue) {
        return this.sortDirection === 'asc' ? -1 : 1;
      } else if (aValue > bValue) {
        return this.sortDirection === 'asc' ? 1 : -1;
      } else {
        return 0;
      }
    });
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
        } else if (error.status === 403) {
          alert('Forbidden');
        } else {
          alert('An error occurred while deleting the product.');
        }
      }
    );
  }

  createProduct() {
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
        }
      }
    );

    this.showCreateProductPopup = false;
    this.newProduct = { name: '', description: '', price: 0, availability: false };
    this.getProducts();
  }


  editName() {
    const value = this.editingProduct.name;
    if (value.length > 30) {
      return;
    }
    const isValid = !(value === '' || value === undefined || value === null);
    if (!isValid) {
      return;
    }
    const url = 'http://localhost:5187/gateway/products/editName';
    const headers = {
      'Authorization': `Bearer ${this.token}`
    };
    const body = {
      productId: this.editingProduct.id,
      userId: this.userId,
      newName: this.editingProduct.name
    };
    console.log('body:', body);
    this.http.patch(url, body, { headers }).subscribe(
      (response: any) => {
        console.log('Edit Name successful:', response);
        alert('Name edited successfully');
      },
      (error) => {
        if (error.status === 200) {
          alert('Name edited successfully');
        } else if (error.status === 403) {
          alert('Forbidden');
        } else {
          alert(error.error.message);
          console.error('Edit Name error:', error);
        }
      }
    );
  }


  editPrice() {
    const value = this.editingProduct.price;
    console.log('value: ', value);
    const isValid = !(value === '' || value === undefined || value === null);
    if (!isValid) {
      return;
    }
    if (value <= 0) {
      return;
    }
    if (value > 999999999) {
      return;
    }
    const url = 'http://localhost:5187/gateway/products/editPrice';
    const headers = {
      'Authorization': `Bearer ${this.token}`
    };
    const body = {
      productId: this.editingProduct.id,
      userId: this.userId,
      newPrice: this.editingProduct.price
    };
    console.log('body:', body);
    this.http.patch(url, body, { headers }).subscribe(
      (response: any) => {
        console.log('Edit Price successful:', response);
        alert('Price edited successfully');
      },
      (error) => {
        if (error.status === 200) {
          alert('Price edited successfully');
        } else if (error.status === 403) {
          alert('Forbidden');
        } else {
          alert(error.error.message);
          console.error('Edit Price error:', error);
        }
      }
    );
  }


  editDescription() {
    const value = this.editingProduct.description;
    if (value.length > 200) {
      return;
    }
    const url = 'http://localhost:5187/gateway/products/editDescription';
    const headers = {
      'Authorization': `Bearer ${this.token}`
    };
    const body = {
      productId: this.editingProduct.id,
      userId: this.userId,
      newDescription: this.editingProduct.description
    };
    console.log('body:', body);
    this.http.patch(url, body, { headers }).subscribe(
      (response: any) => {
        console.log('Edit Description successful:', response);
        alert('Description edited successfully');
      },
      (error) => {
        if (error.status === 200) {
          alert('Description edited successfully');
        } else if (error.status === 403) {
          alert('Forbidden');
        } else {
          alert(error.error.message);
          console.error('Edit Description error:', error);
        }
      }
    );
  }


  editAvailability() {
    const url = 'http://localhost:5187/gateway/products/editAvailability';
    const headers = {
      'Authorization': `Bearer ${this.token}`
    };
    const body = {
      productId: this.editingProduct.id,
      userId: this.userId,
      newAvailability: this.editingProduct.availability
    };
    console.log('body:', body);
    this.http.patch(url, body, { headers }).subscribe(
      (response: any) => {
        console.log('Edit Availability successful:', response);
        alert('Availability edited successfully');
      },
      (error) => {
        if (error.status === 200) {
          alert('Availability edited successfully');
        } else if (error.status === 403) {
          alert('Forbidden');
        } else {
          alert(error.error.message);
          console.error('Edit Availability error:', error);
        }
      }
    );
  }

  isEditNameValid(): boolean {
    const value = this.editingProduct.name;
    if (value.length > 30) {
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
    if (value.length > 30) {
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

  isEditDescriptionValid(): boolean {
    const value = this.editingProduct.description;
    if (value.length > 200) {
      this.errorMessageEditDescription = 'Description must be less than 200 symbols';
      return true;
    }
    return false;
  }

  isDescriptionValid(field: string): boolean {
    const value = this.newProduct[field];
    if (value.length > 200) {
      this.errorMessageDescription = 'Description must be less than 200 symbols';
      return true;
    }
    return false;
  }

  isEditPriceValid(): boolean {
    const value = this.editingProduct.price;
    const isValid = !(value === '' || value === undefined || value === null);
    if (!isValid) {
      this.errorMessageEditPrice = 'Price is required';
      return true;
    }
    if (value <= 0) {
      this.errorMessageEditPrice = "Price must be more than 0";
      return true;
    }
    if (value > 999999999) {
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
    if (value <= 0) {
      this.errorMessagePrice = "Price must be more than 0";
      return true;
    }
    if (value > 999999999) {
      this.errorMessagePrice = "Price can't be more than 999999999";
      return true;
    }
    return false;
  }

  closeCreateProductPopup() {
    this.showCreateProductPopup = false;
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
        console.log('Response body:', response.body);
        this.getProducts();
      },
      error => {
        console.error('Error:', error);

        if (error.status === 403) {
          alert('Access forbidden. You do not have permission to delete this product.');
        } else if (error.status === 401) {
          alert('Unauthorized. Please log in and try again.');
        } else if (error.status === 200) {
          alert('Product deleted successfully!');
          this.getProducts();
        } else if (error.status === 403) {
          alert('Forbidden');
        } else if (error.status === 404) {
          alert('Product not found. It may have been deleted by someone else.');
        } else {
          alert('An error occurred while deleting the product.');
        }
      }
    );
  }



  editProduct(product: any) {
    console.log('Edit Product with ID:', product.id);
    this.showEditProductPopup = true;
    this.editingProduct.name = product.name;
    this.editingProduct.id = product.id;
    this.editingProduct.description = product.description;
    this.editingProduct.price = product.price;
  }

  saveEditedProduct() {
    this.closeEditProductPopup();
  }

  closeEditProductPopup() {
    this.showEditProductPopup = false;
    this.editingProduct = { name: '', description: '', price: 0, availability: false };
    this.getProducts();
  }

  goToUsersMenu() {
    this.router.navigate(['/users-menu']);
  }

  goToProfile(){
    this.router.navigate(['/profile-menu']);
  }

  goBack() {
    this.router.navigate(['/']);
  }
}
