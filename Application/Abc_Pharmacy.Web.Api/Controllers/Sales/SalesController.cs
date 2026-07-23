using Abc_Pharmacy.Application.Dto;
using Abc_Pharmacy.Application.Modules.Sales.Commands;
using Abc_Pharmacy.Application.Modules.Sales.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Abc_Pharmacy.Web.Api.Controllers.Sales
{
    public class SalesController : BaseController
    {
        public SalesController(IMediator mediator) : base(mediator)
        {

        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddSaleCommand command)
        {
            var result = await Mediator.Send(command);
            if (result == null) return BadRequest("Invalid sale request.");
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await Mediator.Send(new GetSalesQuery());

            return Ok(result);
        }

    }
}
