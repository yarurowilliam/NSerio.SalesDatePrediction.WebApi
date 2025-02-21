export interface NewOrderRequest {
    customerId: number;
    empId: number;
    shipperId: number;
    shipName: string;
    shipAddress: string;
    shipCity: string;
    shipCountry: string;
    orderDate: string;
    requiredDate: string;
    shippedDate?: string;
    freight: number;
    orderDetails: OrderDetail[];
  }
  
  export interface OrderDetail {
    productId: number;
    unitPrice: number;
    qty: number;
    discount: number;
  }