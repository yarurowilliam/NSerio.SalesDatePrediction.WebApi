export interface SalesPrediction {
    customerId: number;
    customerName: string;
    lastOrderDate: Date;
    nextPredictedOrder: Date;
  }