import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { OrdersService } from '../../services/orders.service';
import { Order } from '../../models/order.model';

@Component({
  selector: 'app-view-orders-modal',
  standalone: true,
  imports: [CommonModule, MatDialogModule, FormsModule],
  templateUrl: './view-orders-modal.component.html',
  styleUrls: ['./view-orders-modal.component.css']
})
export class ViewOrdersModalComponent implements OnInit {
  customerOrders: Order[] = [];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;
  startIndex = 0;
  endIndex = 0;

  constructor(
    public dialogRef: MatDialogRef<ViewOrdersModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { customerId: number, customerName: string },
    private ordersService: OrdersService
  ) {}

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.ordersService.getCustomerOrders(this.data.customerId).subscribe({
      next: (orders: Order[]) => {
        this.customerOrders = orders;
        this.totalItems = orders.length;
        this.updatePagination();
      },
      error: (error: Error) => console.error('Error loading orders:', error)
    });
  }

  updatePagination() {
    this.startIndex = (this.currentPage - 1) * this.pageSize;
    this.endIndex = Math.min(this.startIndex + this.pageSize, this.totalItems);
    this.customerOrders = this.customerOrders.slice(this.startIndex, this.endIndex);
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.updatePagination();
  }

  closeDialog() {
    this.dialogRef.close();
  }
}