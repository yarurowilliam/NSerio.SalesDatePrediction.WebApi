import { Component } from '@angular/core';
import { SalesPredictionComponent } from './components/sales-prediction/sales-prediction.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [SalesPredictionComponent],
  template: `
    <app-sales-prediction></app-sales-prediction>
  `
})
export class AppComponent {
  title = 'Sales Date Prediction';
}