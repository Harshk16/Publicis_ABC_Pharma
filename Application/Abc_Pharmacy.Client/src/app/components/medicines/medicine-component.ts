import { ChangeDetectorRef, Component, OnInit, ViewEncapsulation, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DestroyRef } from '@angular/core';
import { AddMedicine } from '../../interfaces/add-medicine';
import { Medicine } from '../../interfaces/medicine';
import { ApiService } from '../../services/api-service';


@Component({
  selector: 'app-medicine-component',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './medicine-component.html',
  styleUrl: './medicine-component.scss',
  encapsulation: ViewEncapsulation.None
})
export class MedicineComponent implements OnInit {
  medicines: Medicine[] = [];
  newMedicineForm: FormGroup;
  errorMessage: string | null = null;
  searchQuery: string = '';

  private cdr = inject(ChangeDetectorRef)

  constructor(
    private apiService: ApiService,
    private snackBar: MatSnackBar,
    private fb: FormBuilder
  ) {
    this.newMedicineForm = this.fb.group({
      fullName: ['', Validators.required],
      expiryDate: ['', Validators.required],
      quantity: [null, [Validators.required, Validators.min(1)]],
      price: [null, [Validators.required, Validators.min(0.01)]],
      brand: ['', Validators.required],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.getMedicines();
  }

  onSearchChange(): void {
    const query = this.searchQuery.trim();

    if (query.length >= 3 || query.length === 0) {
      this.getMedicines();
    }
  }

  clearSearch(): void {
    this.searchQuery = '';
    this.getMedicines();
      this.cdr.detectChanges();
  }

  getMedicines(): void {
    this.apiService.getMedicines(this.searchQuery).subscribe({
      next: meds => {
        this.medicines = meds.map(med => ({
          ...med,
          isExpiring: this.checkExpiryStatus(med.expiryDate)
        }));

        this.cdr.detectChanges();
      },
      error: err => {
        console.error('Failed to load medicines', err);
        this.snackBar.open('Error loading data', 'Close', { duration: 3000 });
      }
    });
  }


  private checkExpiryStatus(date: string | null | undefined): boolean {
    if (!date) return false;
    const expiry = new Date(date);
    if (isNaN(expiry.getTime())) return false;
    const today = new Date();
    const diffDays = Math.floor((expiry.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
    return diffDays < 30;
  }

  buyMedicine(med: Medicine, qty: string, qtyInput: HTMLInputElement): void {
    const quantity = parseInt(qty, 10);

    if (!quantity || quantity <= 0) {
      this.snackBar.open('Please enter a valid quantity', 'Close', {
        duration: 3000, panelClass: ['error-toast']
      });
      return;
    }

    if (quantity > med.quantity) {
      this.snackBar.open(`Only ${med.quantity} in stock. You entered ${quantity}.`, 'Close', {
        duration: 4000, panelClass: ['error-toast']
      });
      return;
    }

    this.apiService.addSale(med.id, quantity).subscribe({
      next: sale => {
        this.snackBar.open(
          `Sale recorded: ${sale.quantitySold} x ${med.fullName}`,
          'Close',
          { duration: 3000, panelClass: ['success-toast'] }
        );
        this.getMedicines();
        qtyInput.value = '';
      },
      error: err => {
        this.snackBar.open('Sale failed: ' + err.message, 'Close', {
          duration: 4000, panelClass: ['error-toast']
        });
      }
    });
  }

  addMedicine(): void {
    if (this.newMedicineForm.invalid) {
      this.newMedicineForm.markAllAsTouched();
      return;
    }

    const newMedicine: AddMedicine = this.newMedicineForm.value;

    this.apiService.addMedicine(newMedicine).subscribe({
      next: med => {
        this.snackBar.open(
          `Medicine ${med.fullName} added successfully`,
          'Close',
          { duration: 3000, panelClass: ['success-toast'] }
        );
        this.getMedicines();
        this.resetForm();
      },
      error: err => {
        this.snackBar.open(
          'Failed to add medicine: ' + err.message,
          'Close',
          { duration: 4000, panelClass: ['error-toast'] }
        );
      }
    });
  }

  resetForm(): void {
    this.newMedicineForm.reset();
  }
}