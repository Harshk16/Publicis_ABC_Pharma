using Abc_Pharmacy.Application.Dto;
using Abc_Pharmacy.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Abc_Pharmacy.Application.Modules.Medicine.Commands
{
    public class UpdateMedicineCommand : IRequest<bool>
    {
        public UpdateMedicineDto _Dto { get; set; }
        public UpdateMedicineCommand(UpdateMedicineDto dto) => _Dto = dto;

        public class UpdateMedicineCommandHandler : IRequestHandler<UpdateMedicineCommand, bool>
        {
            private readonly IJsonDataService _jsonDataService;
            public UpdateMedicineCommandHandler(IJsonDataService jsonDataService)
            {
                _jsonDataService = jsonDataService;
            }
            public async Task<bool> Handle(UpdateMedicineCommand request, CancellationToken cancellationToken)
            {
                List<MedicineDto> medicines = await _jsonDataService.ReadAsync<MedicineDto>(
         "medicines.json", cancellationToken);

                if (medicines == null || !medicines.Any())
                    return false;

              
                var medicineToUpdate = medicines.FirstOrDefault(m => m.Id == request._Dto.Id);
                if (medicineToUpdate == null)
                    return false;

                medicineToUpdate.FullName = request._Dto.FullName;
                medicineToUpdate.Notes = request._Dto.Notes;
                medicineToUpdate.Price = request._Dto.Price;
                medicineToUpdate.Quantity = request._Dto.Quantity;
                medicineToUpdate.Brand = request._Dto.Brand;
                medicineToUpdate.ExpiryDate = request._Dto.ExpiryDate;

                await _jsonDataService.WriteAsync("medicines.json", medicines, cancellationToken);

                return true;
            }
        }
    }
}
