export interface SaleRecord {
  id: number;
  medicineId: number;
  fullName: string;
  brand: string;
  unitPrice: number;
  quantitySold: number;
  totalPrice: number;
  saleDate: Date;
}
