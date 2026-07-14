using System;
using System.Collections.Generic;
using System.Text;

namespace Abc_Pharmacy.Application.Dto
{
    public class SaleRecordDto
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string FullName { get; set; }
        public string Brand { get; set; }
        public decimal UnitPrice { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
