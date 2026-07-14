using Abc_Pharmacy.Application.Dto;
using Abc_Pharmacy.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Abc_Pharmacy.Application.Modules.Sales.Queries
{
    public class GetSalesQuery : IRequest<List<SaleRecordDto>>
    {
    }

    public class GetSalesHandler : IRequestHandler<GetSalesQuery, List<SaleRecordDto>>
    {
        private readonly IJsonDataService _jsonDataService;

        public GetSalesHandler(IJsonDataService jsonDataService)
        {
            _jsonDataService = jsonDataService;
        }

        public async Task<List<SaleRecordDto>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
        {
            List<SaleRecordDto> salesData = await _jsonDataService.ReadAsync<SaleRecordDto>("sales.json", cancellationToken);

            if (salesData == null)
            {
                return [];
            }

            var data = salesData.Select(sale => new SaleRecordDto
            {
                Id = sale.Id,
                MedicineId = sale.MedicineId,
                FullName = sale.FullName,
                Brand = sale.Brand,
                UnitPrice = sale.UnitPrice,
                QuantitySold = sale.QuantitySold,
                TotalPrice = sale.TotalPrice == 0 ? (sale.UnitPrice * sale.QuantitySold) : sale.TotalPrice,
                SaleDate = sale.SaleDate == default ? DateTime.UtcNow : sale.SaleDate
            }).ToList();

            return data;
        }
    }
}
