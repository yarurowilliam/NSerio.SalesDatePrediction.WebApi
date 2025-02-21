import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { OrdersService } from '../../services/orders.service';
import { EmployeesService } from '../../services/employees.service';
import { ShippersService } from '../../services/shippers.service';
import { ProductsService } from '../../services/products.service';
import { Employee } from '../../models/employee.model';
import { Shipper } from '../../models/shipper.model';
import { Product } from '../../models/product.model';
import { NewOrderRequest } from '../../models/new-order.model';

@Component({
  selector: 'app-new-order-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule],
  templateUrl: './new-order-modal.component.html',
  styleUrls: ['./new-order-modal.component.css']
})
export class NewOrderModalComponent implements OnInit {
  orderForm: FormGroup;
  employees: Employee[] = [];
  shippers: Shipper[] = [];
  products: Product[] = [];

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<NewOrderModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { customerId: number, customerName: string },
    private ordersService: OrdersService,
    private employeesService: EmployeesService,
    private shippersService: ShippersService,
    private productsService: ProductsService
  ) {
    this.orderForm = this.fb.group({
      empId: ['', Validators.required],
      shipperId: ['', Validators.required],
      shipName: ['', Validators.required],
      shipAddress: ['', Validators.required],
      shipCity: ['', Validators.required],
      shipCountry: ['', Validators.required],
      orderDate: ['', Validators.required],
      requiredDate: ['', Validators.required],
      shippedDate: [''],
      freight: [0, [Validators.required, Validators.min(0)]],
      productId: ['', Validators.required],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      quantity: [1, [Validators.required, Validators.min(1)]],
      discount: [0, [Validators.required, Validators.min(0), Validators.max(1)]]
    });
  }

  ngOnInit() {
    this.loadEmployees();
    this.loadShippers();
    this.loadProducts();
  }

  loadEmployees() {
    this.employeesService.getAllEmployees().subscribe({
      next: (employees) => {
        this.employees = employees;
      },
      error: (error) => {
        console.error('Error loading employees:', error);
      }
    });
  }

  loadShippers() {
    this.shippersService.getAllShippers().subscribe({
      next: (shippers) => {
        this.shippers = shippers;
      },
      error: (error) => {
        console.error('Error loading shippers:', error);
      }
    });
  }

  loadProducts() {
    this.productsService.getAllProducts().subscribe({
      next: (products) => {
        this.products = products;
      },
      error: (error) => {
        console.error('Error loading products:', error);
      }
    });
  }

  onSubmit() {
    if (this.orderForm.valid) {
      const formValue = this.orderForm.value;
      
      const orderData: NewOrderRequest = {
        customerId: this.data.customerId,
        empId: parseInt(formValue.empId),
        shipperId: parseInt(formValue.shipperId),
        shipName: formValue.shipName,
        shipAddress: formValue.shipAddress,
        shipCity: formValue.shipCity,
        shipCountry: formValue.shipCountry,
        orderDate: formValue.orderDate,
        requiredDate: formValue.requiredDate,
        shippedDate: formValue.shippedDate || null,
        freight: parseFloat(formValue.freight),
        orderDetails: [{
          productId: parseInt(formValue.productId),
          unitPrice: parseFloat(formValue.unitPrice),
          qty: parseInt(formValue.quantity),
          discount: parseFloat(formValue.discount)
        }]
      };
  
      this.ordersService.createOrder(orderData).subscribe({
        next: (response) => {
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error creating order:', error);
        }
      });
    }
  }

  onClose() {
    this.dialogRef.close();
  }
}