import { Routes } from '@angular/router';
import { MedicineComponent } from './components/medicines/medicine-component';
import { SaleComponent } from './components/sales/sale-component';


export const routes: Routes = [
  { path: 'medicine', component: MedicineComponent },
  { path: 'sales', component: SaleComponent },
  { path: '', redirectTo: 'medicine', pathMatch: 'full' }
];
