using System;
using System.Collections.Generic;
using System.Text;

namespace Abc_Pharmacy.Application.Dto
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public required string Brand { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
