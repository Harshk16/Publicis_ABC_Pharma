import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { ApiService } from '../../services/api-service';

@Component({
  selector: 'app-sale-component',
  imports: [CommonModule],
  templateUrl: './sale-component.html',
  styleUrl: './sale-component.scss',
})
export class SaleComponent implements OnInit {
   sales = signal<any[]>([]);

  private apiService = inject(ApiService);

  ngOnInit(): void {
    this.loadSales();
  }

  loadSales(): void {
    this.apiService.getSales().subscribe({
      next: (data) => {
        var res = data;
        console.log('Sales data loaded into array:', this.sales);
        
        this.sales.set(res);
      },
      error: (err) => {
        console.error('Failed to fetch sales records', err);
      }
    });
  }
}
