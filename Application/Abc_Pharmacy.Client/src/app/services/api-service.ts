
import { delay, of } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';
import { Medicine } from '../interfaces/medicine';
import { SaleRecord } from '../interfaces/sales-record';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AddMedicine } from '../interfaces/add-medicine';
import { inject, Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ApiService {
    private http = inject(HttpClient);
    private baseUrl = 'http://localhost:5292/api/';

    getMedicines(search?: string): Observable<Medicine[]> {
        let params = new HttpParams();
        if (search) {
            params = params.set('search', search);
        }
        return this.http.get<Medicine[]>(`${this.baseUrl}medicine/Get`, { params });
    }

    addMedicine(medicine: AddMedicine): Observable<Medicine> {
        return this.http.post<Medicine>(`${this.baseUrl}medicine/Add`, medicine);
    }

    updateMedicine(med: Medicine): Observable<Medicine> {
        return this.http.put<Medicine>(`${this.baseUrl}Update/${med.id}`, med);
    }

    deleteMedicine(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/Delete/${id}`);
    }

    getSales(): Observable<SaleRecord[]> {
        return this.http.get<SaleRecord[]>(`${this.baseUrl}sales/Get`);
    }

    addSale(medicineId: number, quantitySold: number): Observable<SaleRecord> {
        return this.http.post<SaleRecord>(
            `${this.baseUrl}sales/add`,
            { medicineId, quantitySold }
        );
    }

}
