import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Shipper } from '../models/shipper.model';

@Injectable({
  providedIn: 'root'
})
export class ShippersService {
  private apiUrl = 'http://localhost:5163/api/Shippers';

  constructor(private http: HttpClient) { }

  getAllShippers(): Observable<Shipper[]> {
    return this.http.get<Shipper[]>(this.apiUrl);
  }
}

