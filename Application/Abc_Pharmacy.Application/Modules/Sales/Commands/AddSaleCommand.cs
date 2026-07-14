using Abc_Pharmacy.Application.Dto;
using Abc_Pharmacy.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Abc_Pharmacy.Application.Modules.Sales.Commands
{
    public class AddSaleCommand : IRequest<SaleRecordDto?>
    {
        public int MedicineId { get; set; }
        public int QuantitySold { get; set; }

        public class AddSaleCommandHandler : IRequestHandler<AddSaleCommand, SaleRecordDto?>
        {
            private readonly IJsonDataService _jsonDataService;
            public AddSaleCommandHandler(IJsonDataService jsonDataService)
            {
              _jsonDataService = jsonDataService;
            }
            public async Task<SaleRecordDto?> Handle(AddSaleCommand request, CancellationToken cancellationToken)
            {
                List<MedicineDto> medicines = await _jsonDataService.ReadAsync<MedicineDto>(
                    "medicines.json", cancellationToken);

                if (medicines == null || !medicines.Any())
                    return null;

                var medicine = medicines.FirstOrDefault(m => m.Id == request.MedicineId);
                if (medicine == null || medicine.Quantity < request.QuantitySold)
                    return null;

                medicine.Quantity -= request.QuantitySold;

                await _jsonDataService.WriteAsync("medicines.json", medicines, cancellationToken);

                List<SaleRecordDto> sales = await _jsonDataService.ReadAsync<SaleRecordDto>(
                    "sales.json", cancellationToken);

                if (sales == null)
                    sales = new List<SaleRecordDto>();

                var newSaleId = sales.Any() ? sales.Max(s => s.Id) + 1 : 1;

                var sale = new SaleRecordDto
                {
                    Id = newSaleId,
                    MedicineId = request.MedicineId,
                    FullName = medicine.FullName,
                    Brand = medicine.Brand,
                    UnitPrice = medicine.Price,
                    QuantitySold = request.QuantitySold,
                    TotalPrice = request.QuantitySold * medicine.Price,
                    SaleDate = DateTime.UtcNow
                };

                sales.Add(sale);

                await _jsonDataService.WriteAsync("sales.json", sales, cancellationToken);

                return sale;
            }
        }
    }
}
