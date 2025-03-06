
using AutoMapper;
using LIB.API.Application.Contracts.Persistence;



using MediatR;
using Microsoft.AspNetCore.Mvc;

using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.CQRS.OutReconciled.Request.Queries;

using LIB.API.Application.CQRS.OutReconciled.Request.Command;
using LIB.API.Application.DTOs.OutReconciled;

namespace LIBPROPERTY_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutReconciledController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IOutReconciledRepository _OutReconciledRepository;

        public OutReconciledController(IMediator mediator, IOutReconciledRepository OutReconciledRepository)
        {
            _mediator = mediator;
            _OutReconciledRepository = OutReconciledRepository;
        }

        // GET: api/<OutReconciledController>
        [HttpGet]
        public async Task<ActionResult<List<OutReconciledDto>>> Get()
        {
            var OutReconcileds = await _mediator.Send(new GetOutReconciledListRequest());
            return Ok(OutReconcileds);
        }

        // GET api/<OutReconciledController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<OutReconciledDto>>> Get(int id)
        {
            var OutReconciled = await _mediator.Send(new GetOutReconciledDetailRequest { Id = id });
            return Ok(OutReconciled);
        }

        // POST api/<OutReconciledController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] OutReconciledDto OutReconciled)
        {
            var command = new CreateOutReconciledCommand { OutReconciledDto = OutReconciled };
            await _mediator.Send(command);
            return Ok(command);

        }


        // DELETE api/<OutReconciledController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var command = new DeleteOutReconciledCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }





    }

}
