using Abc_Pharmacy.Application.Dto;
using Abc_Pharmacy.Application.Modules.Medicine.Commands;
using Abc_Pharmacy.Application.Modules.Medicine.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Abc_Pharmacy.Web.Api.Controllers.Medicine
{
    public class MedicineController : BaseController
    {
        public MedicineController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get(string? search)
        {
            var result = await Mediator.Send(new GetMedicineQuery(search));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddMedicineDto dto)
        {
            var result = await Mediator.Send(new AddMedicineCommand(dto));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateMedicineDto dto)
        {
            var result = await Mediator.Send(new UpdateMedicineCommand(dto));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var result = await Mediator.Send(new DeleteMedicineCmmand(id));
            if (!result) return NotFound();
            return NoContent();
        }


    }
}
