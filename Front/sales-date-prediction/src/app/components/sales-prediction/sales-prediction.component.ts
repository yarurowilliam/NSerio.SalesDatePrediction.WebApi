import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { SalesPredictionService } from '../../services/sales-prediction.service';
import { SalesPrediction } from '../../models/sales-prediction.model';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ViewOrdersModalComponent } from '../view-orders-modal/view-orders-modal.component';
import { NewOrderModalComponent } from '../new-order-modal/new-order-modal.component';

@Component({
  selector: 'app-sales-prediction',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    FormsModule,
    MatDialogModule,
    ViewOrdersModalComponent,
    NewOrderModalComponent
  ],
  templateUrl: './sales-prediction.component.html',
  styleUrls: ['./sales-prediction.component.css']
})
export class SalesPredictionComponent implements OnInit {
  predictions: SalesPrediction[] = [];
  filteredPredictions: SalesPrediction[] = [];
  searchTerm: string = '';
  pageSize: number = 10;
  currentPage: number = 1;
  totalItems: number = 0;
  startIndex: number = 0;
  endIndex: number = 0;

  constructor(
    private salesPredictionService: SalesPredictionService,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.loadPredictions();
  }

  loadPredictions() {
    this.salesPredictionService.getAllPredictions().subscribe({
      next: (data) => {
        this.predictions = data;
        this.applyFilter();
        this.updatePagination();
      },
      error: (error) => {
        console.error('Error loading predictions:', error);
      }
    });
  }

  applyFilter() {
    if (this.searchTerm) {
      this.filteredPredictions = this.predictions.filter(p => 
        p.customerName.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    } else {
      this.filteredPredictions = [...this.predictions];
    }
    this.totalItems = this.filteredPredictions.length;
    this.updatePagination();
  }

  updatePagination() {
    this.startIndex = (this.currentPage - 1) * this.pageSize;
    this.endIndex = Math.min(this.startIndex + this.pageSize, this.totalItems);
    this.filteredPredictions = this.predictions.slice(this.startIndex, this.endIndex);
  }

  onPageSizeChange() {
    this.currentPage = 1;
    this.updatePagination();
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination();
    }
  }

  nextPage() {
    if (this.endIndex < this.totalItems) {
      this.currentPage++;
      this.updatePagination();
    }
  }

  viewOrders(prediction: SalesPrediction) {
    const dialogRef = this.dialog.open(ViewOrdersModalComponent, {
      width: '900px',
      data: {
        customerId: prediction.customerId,
        customerName: prediction.customerName
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('Modal cerrado');
    });
  }

  newOrder(prediction: SalesPrediction) {
    const dialogRef = this.dialog.open(NewOrderModalComponent, {
      width: '600px',
      data: {
        customerId: prediction.customerId,
        customerName: prediction.customerName
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadPredictions();
      }
    });
  }
}