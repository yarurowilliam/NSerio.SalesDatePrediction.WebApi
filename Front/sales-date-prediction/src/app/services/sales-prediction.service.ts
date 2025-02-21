import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SalesPrediction } from '../models/sales-prediction.model';

@Injectable({
  providedIn: 'root'
})
export class SalesPredictionService {
  private apiUrl = 'http://localhost:5163/api'; 

  constructor(private http: HttpClient) { }

  getAllPredictions(): Observable<SalesPrediction[]> {
    return this.http.get<SalesPrediction[]>(`${this.apiUrl}/SalesDatePrediction`);
  }
}