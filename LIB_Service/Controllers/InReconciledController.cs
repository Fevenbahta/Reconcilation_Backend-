
using AutoMapper;
using LIB.API.Application.Contracts.Persistence;



using MediatR;
using Microsoft.AspNetCore.Mvc;

using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.CQRS.InReconciled.Request.Queries;

using LIB.API.Application.CQRS.InReconciled.Request.Command;
using LIB.API.Application.DTOs.InReconciled;

namespace LIBPROPERTY_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InReconciledController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IInReconciledRepository _InReconciledRepository;

        public InReconciledController(IMediator mediator, IInReconciledRepository InReconciledRepository)
        {
            _mediator = mediator;
            _InReconciledRepository = InReconciledRepository;
        }

        // GET: api/<InReconciledController>
        [HttpGet]
        public async Task<ActionResult<List<InReconciledDto>>> Get()
        {
            var InReconcileds = await _mediator.Send(new GetInReconciledListRequest());
            return Ok(InReconcileds);
        }

        // GET api/<InReconciledController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<InReconciledDto>>> Get(int id)
        {
            var InReconciled = await _mediator.Send(new GetInReconciledDetailRequest { Id = id });
            return Ok(InReconciled);
        }

        // POST api/<InReconciledController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InReconciledDto InReconciled)
        {
            var command = new CreateInReconciledCommand { InReconciledDto = InReconciled };
            await _mediator.Send(command);
            return Ok(command);

        }


        // DELETE api/<InReconciledController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var command = new DeleteInReconciledCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }





    }

}
