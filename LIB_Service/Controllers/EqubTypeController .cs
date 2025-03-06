using AutoMapper;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.EqubType.Request.Command;
using LIB.API.Application.CQRS.EqubType.Request.Queries;
using LIB.API.Application.CQRS.Lottery.Request.Command;
using LIB.API.Application.CQRS.Lottery.Request.Queries;
using LIB.API.Application.CQRS.Transaction.Handler.Command;
using LIB.API.Application.CQRS.Transaction.Request.Command;
using LIB.API.Application.CQRS.Transaction.Request.Queries;
using LIB.API.Application.DTOs.EqubType.Validators;
using LIB.API.Application.DTOs.Lottery;
using LIB.API.Application.DTOs.Transaction;
using LIB.API.Domain;
using LIB.API.Persistence;
using LIB.API.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIBPROPERTY_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EqubTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EqubTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<EqubTypeController>
        [HttpGet]
        public async Task<ActionResult<List<EqubTypeDto>>> Get()
        {
            var EqubTypes = await _mediator.Send(new GetEqubTypeListRequest());
            return Ok(EqubTypes);
        }
        // GET: api/<EqubTypeController>
        [HttpGet("All")]
        public async Task<ActionResult<List<EqubTypeDto>>> GetAll()
        {
            var EqubTypes = await _mediator.Send(new GetEqubTypeAllListRequest());
            return Ok(EqubTypes);
        }

        // GET api/<EqubTypeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<EqubTypeDto>>> Get(string id)
        {
            var EqubType = await _mediator.Send(new GetEqubTypeDetailRequest { Id = id });
            return Ok(EqubType);
        }

        // POST api/<EqubTypeController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EqubTypeDto EqubType)
        {
            var command = new CreateEqubTypeCommand { EqubTypeDto = EqubType };
            await _mediator.Send(command);
            return Ok(command);

        }
        // PUT api/<EqubTypeController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] EqubTypeDto equbType)
        {
            if (id != equbType.Id)
            {
                return BadRequest("ID mismatch");
            }

            var command = new UpdateEqubTypeCommand { EqubTypeDto = equbType };
            await _mediator.Send(command);
            return NoContent();
        }



        // DELETE api/<EqubTypeController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var command = new DeleteEqubTypeCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }

}