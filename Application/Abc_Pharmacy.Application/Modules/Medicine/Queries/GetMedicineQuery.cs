using Abc_Pharmacy.Application.Dto;
using Abc_Pharmacy.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Abc_Pharmacy.Application.Modules.Medicine.Queries
{
    public class GetMedicineQuery : IRequest<List<MedicineDto>>
    {
        public string Search { get; set; }
        public GetMedicineQuery(string search)
        {
            Search = search;
        }

        public class GetMedicineQueryHandler : IRequestHandler<GetMedicineQuery, List<MedicineDto>>
        {
            private readonly IJsonDataService _jsonDataService;

            public GetMedicineQueryHandler(IJsonDataService jsonDataService)
            {
                _jsonDataService = jsonDataService;
            }
            public async Task<List<MedicineDto>> Handle(GetMedicineQuery request, CancellationToken cancellationToken)
            {

                List<MedicineDto> medicineList = await _jsonDataService.ReadAsync<MedicineDto>("medicines.json", cancellationToken);

                if (medicineList == null)
                {
                    return [];
                }

                var query = medicineList.Where(m => m.IsActive);

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    query = query.Where(m => m.FullName.Contains(request.Search, StringComparison.OrdinalIgnoreCase));
                }


                var data = query.Select(med => new MedicineDto
                    {
                        Id = med.Id,
                        FullName = med.FullName,
                        Brand = med.Brand,
                        ExpiryDate = med.ExpiryDate,
                        Price = med.Price,
                        Quantity = med.Quantity,
                        Notes = med.Notes,
                        IsActive = med.IsActive,

                    }).ToList();

    

                return data;
            }
        }
    }
}
