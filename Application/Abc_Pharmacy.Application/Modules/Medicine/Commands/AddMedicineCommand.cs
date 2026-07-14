using Abc_Pharmacy.Application.Dto;
using Abc_Pharmacy.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Abc_Pharmacy.Application.Modules.Medicine.Commands
{
    public class AddMedicineCommand : IRequest<int>
    {
        public AddMedicineDto _Dto { get; set; }
        public AddMedicineCommand(AddMedicineDto dto) => _Dto = dto;

        public class AddMedicineHandler : IRequestHandler<AddMedicineCommand, int>
        {
            private readonly IJsonDataService _jsonDataService;
            public AddMedicineHandler(IJsonDataService jsonDataService) {
                _jsonDataService = jsonDataService;
            }


            public async Task<int> Handle(AddMedicineCommand request, CancellationToken cancellationToken)
            {
              
                List<MedicineDto> medicines = await _jsonDataService.ReadAsync<MedicineDto>("medicines.json", cancellationToken);

                int lastId = medicines.Any() ? medicines.Max(m => m.Id) : 0;
                int newId = lastId + 1;

                var inputDto = request._Dto;
                var newMedicineEntity = new MedicineDto
                {
                    Id = newId,
                    FullName = inputDto.FullName,
                    Brand = inputDto.Brand,
                    ExpiryDate = inputDto.ExpiryDate,
                    Quantity = inputDto.Quantity,
                    Price = inputDto.Price,
                    Notes = inputDto.Notes,
                    IsActive = true
                };

           
                medicines.Add(newMedicineEntity);

                await _jsonDataService.WriteAsync("medicines.json", medicines, cancellationToken);

                return newMedicineEntity.Id;
            }
        }
    }



}
