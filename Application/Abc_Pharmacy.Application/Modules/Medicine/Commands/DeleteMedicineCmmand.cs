using Abc_Pharmacy.Application.Dto;
using Abc_Pharmacy.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Abc_Pharmacy.Application.Modules.Medicine.Commands
{
    public class DeleteMedicineCmmand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeleteMedicineCmmand(int id) => Id = id;

        public class DeleteCommandHandler : IRequestHandler<DeleteMedicineCmmand, bool>
        {
            private readonly IJsonDataService _jsonDataService;
            public DeleteCommandHandler(IJsonDataService jsonDataService)
            {
                _jsonDataService = jsonDataService;
            }
            public async Task<bool> Handle(DeleteMedicineCmmand request, CancellationToken cancellationToken)
            {
                List<MedicineDto> medicines = await _jsonDataService.ReadAsync<MedicineDto>("medicines.json", cancellationToken);

                var existing = medicines.FirstOrDefault(m => m.Id == request.Id);
                if (existing == null)
                    return false;

               
                existing.IsActive = false;

               
                await _jsonDataService.WriteAsync("medicines.json", medicines, cancellationToken);

                return true;

            }
        }

    }
}
