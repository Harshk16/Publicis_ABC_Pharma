import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { SaleRecord } from '../../interfaces/sales-record';
import { ApiService } from '../../services/api-service';

@Component({
  selector: 'app-sale-component',
  imports: [CommonModule],
  templateUrl: './sale-component.html',
  styleUrl: './sale-component.scss',
})
export class SaleComponent implements OnInit {
  sales: SaleRecord[] = [];

  private apiService = inject(ApiService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit(): void {
    this.loadSales();
  }

  loadSales(): void {
    this.apiService.getSales().subscribe({
      next: (data) => {
        this.sales = data;
        console.log('Sales data loaded into array:', this.sales);
        
        this.cdr.detectChanges(); 
      },
      error: (err) => {
        console.error('Failed to fetch sales records', err);
      }
    });
  }
}
