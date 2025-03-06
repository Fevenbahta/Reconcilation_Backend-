



using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.Lottery.Request.Command;
using LIB.API.Application.CQRS.Lottery.Request.Queries;
using LIB.API.Application.CQRS.Transaction.Handler.Command;
using LIB.API.Application.CQRS.Transaction.Request.Command;
using LIB.API.Application.CQRS.Transaction.Request.Queries;
using LIB.API.Application.DTOs.Lottery;
using LIB.API.Application.DTOs.Lottery.Validators;
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
    public class LotteryController : ControllerBase
    {
        private readonly IMediator _mediator;
  
        private readonly ILotteryRepository _lotteryRepository;

        public LotteryController(IMediator mediator, ILotteryRepository lotteryRepository)
        {
            _mediator = mediator;
            _lotteryRepository = lotteryRepository;
        }

        // GET: api/<LotteryController>
        [HttpGet]
        public async Task<ActionResult<List<LotteryDto>>> Get()
        {
            var Lotterys = await _mediator.Send(new GetLotteryListRequest());
            return Ok(Lotterys);
        }

        // GET api/<LotteryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<LotteryDto>>> Get(int id)
        {
            var Lottery = await _mediator.Send(new GetLotteryDetailRequest { Id = id });
            return Ok(Lottery);
        }

        // POST api/<LotteryController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LotteryDto Lottery)
        {
            var command = new CreateLotteryCommand { LotteryDto = Lottery };
            await _mediator.Send(command);
            return Ok(command);

        }


        // DELETE api/<LotteryController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteLotteryCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("equbType/winners")]
        public async Task<IActionResult> GetLotteryWinners([FromBody] EqubTypeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.EqubTypeName))
            {
                return BadRequest(new { message = "EqubTypeName is required." });
            }

            try
            {
                var winners = await _lotteryRepository.GetWinnersByEqubType(request.EqubTypeName);

                if (winners == null || winners.Count == 0)
                {
                    return NotFound(new { message = "No winners found for the specified EqubType." });
                }

                return Ok(winners);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "An error occurred while processing your request. Please try again later.",
                    ErrorCode = "500",
                    ErrorDetails = new
                    {
                        ExceptionMessage = ex.Message,
                        ExceptionType = ex.GetType().ToString(),
                        StackTrace = ex.StackTrace
                    }
                };

                return StatusCode(500, errorResponse);
            }
        }


        [HttpPost("equbType/generate")]
        public async Task<IActionResult> Generate([FromBody] EqubTypeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.EqubTypeName))
            {
                return BadRequest(new { message = "EqubTypeName is required." });
            }

            try
            {
                var latestLottery = await _lotteryRepository.GetLatestLotteryByEqubType(request.EqubTypeName);

                if (latestLottery == null)
                {
                    return NotFound(new { message = "No lottery records found for the specified EqubType." });
                }

                return Ok(latestLottery);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "An error occurred while processing your request. Please try again later.",
                    ErrorCode = "500",
                    ErrorDetails = new
                    {
                        ExceptionMessage = ex.Message,
                        ExceptionType = ex.GetType().ToString(),
                        StackTrace = ex.StackTrace
                    }
                };

                return StatusCode(500, errorResponse);
            }
        }


    }

}
