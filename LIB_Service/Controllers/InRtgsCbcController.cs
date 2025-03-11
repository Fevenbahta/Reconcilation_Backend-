using AutoMapper;
using LIB.API.Application.Contracts.Persistence;

using LIB.API.Application.CQRS.InRtgsCbc.Request.Command;
using LIB.API.Application.CQRS.InRtgsCbc.Request.Queries;
using LIB.API.Application.DTOs.InRtgsCbc;
using LIB.API.Application.DTOs.OutReconciled;
using LIB.API.Domain;
using LIB.API.Persistence;
using LIB.API.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIBPROPERTY_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InRtgsCbcController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly LIBAPIDbSQLContext _context;
        private readonly IInRtgsCbcRepository _InRtgsCbcRepository;

        private readonly UpdateLogService _updateLogService;
        private readonly IOutRtgsCbcRepository _OutRtgsCbcRepository;
        private readonly IInRtgsAtsRepository _InRtgsAtsRepository;

        public InRtgsCbcController(IMediator mediator, IMapper mapper, LIBAPIDbSQLContext context,
   
            IInRtgsCbcRepository InRtgsCbcRepository,
            UpdateLogService updateLogService,
            IOutRtgsCbcRepository OutRtgsCbcRepository,
            IInRtgsAtsRepository InRtgsAtsRepository
            )
        {
            _updateLogService = updateLogService;
            _OutRtgsCbcRepository = OutRtgsCbcRepository;
            _InRtgsAtsRepository = InRtgsAtsRepository;
            _mediator = mediator;
            _mapper = mapper;
            _context = context;

            _InRtgsCbcRepository = InRtgsCbcRepository;
        }

        // GET: api/InRtgsCbc
        [HttpGet]
        public async Task<ActionResult<List<InRtgsCbcDto>>> GetAllInRtgsCbcs()
        {
            var InRtgsCbc = await _mediator.Send(new GetInRtgsCbcListRequest());
            return Ok(InRtgsCbc);
        }

        // GET: api/InRtgsCbc/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<List<InRtgsCbcDto>>> GetInRtgsCbcById(int id)
        {
            var InRtgsCbc = await _mediator.Send(new GetInRtgsCbcDetaileRequest { Id = id });
            return Ok(InRtgsCbc);
        }

        // POST: api/InRtgsCbc
        [HttpPost]
        public async Task<ActionResult> CreateInRtgsCbc([FromBody] InRtgsCbcDto InRtgsCbc)
        {
            var command = new CreateInRtgsCbcCommand { InRtgsCbcDto = InRtgsCbc };
            await _mediator.Send(command);
            return Ok(command);
        }

        // PUT: api/InRtgsCbc/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInRtgsCbc(int id, [FromBody] InRtgsCbcDto InRtgsCbc)
        {

            try
            {
 var command = new UpdateInRtgsCbcCommand { InRtgsCbcDto = InRtgsCbc };
            //_context.Entry(existingEvent).Property(x => x.ReferenceNumber).IsModified = false;
            await _mediator.Send(command);
            return NoContent();

            }
            catch (Exception ex)
            {
                // Log exception (not shown here)
                return StatusCode(500, $"{ex.Message}");
            }
        }

        // DELETE: api/InRtgsCbc/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInRtgsCbc(int id)
        {
            var command = new DeleteInRtgsCbcCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }


        [HttpGet("dateRange")]
        public async Task<ActionResult<List<InRtgsCbcDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            // Call the mediator to get the data within the specified date range
            var result = await _InRtgsCbcRepository.GetInRtgsCbcDByDateIntervalAsync(startDate, endDate);
            return Ok(result);
        }
        // GET: api/InRtgsCbc/GetUserDetailsByAccountNumber/{accountNumber}


        /*        [HttpGet("GetUserDetailsByAccountNumber/{accountNumber}")]
                public async Task<IActionResult> GetUserDetailsByAccountNumber(string accountNumber)
                {
                    var userDetails = await _InRtgsCbcRepository.GetUserDetailsByAccountNumberAsync(accountNumber);



                    return Ok(userDetails);
                }


         */
        /*        [HttpGet("GetUserDetail")]
                public async Task<IActionResult> GetUserDetails(string branch, string userName, string role)
                {
                    if (string.IsNullOrEmpty(branch) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(role))
                    {
                        return BadRequest("Branch, UserName, and Role are required parameters.");
                    }

                    var userDetails = await _InRtgsCbcRepository.GetUserDetailAsync(branch, userName, role);

                    if (userDetails == null)
                    {
                        return NotFound();
                    }

                    return Ok(userDetails);
                }
        */


        /*     [HttpGet("GetUserDetailByUserName")]
             public async Task<IActionResult> GetUserDetailsByUserName(string userName)
             {
                 if (string.IsNullOrEmpty(userName))
                 {
                     return BadRequest("Branch, UserName, and Role are required parameters.");
                 }

                 var userDetails = await _InRtgsCbcRepository.GetUserDetailAsyncByUserName(userName);

                 if (userDetails == null)
                 {
                     return NotFound();
                 }

                 return Ok(userDetails);
             }
             */



        /*
                [HttpPost("CreateAndApproveInRtgsCbc")]
                public async Task<IActionResult> CreateAndApproveInRtgsCbc(string mobileNumber, string accountNumber, decimal amount, string memberId)
                {
                    if (string.IsNullOrEmpty(mobileNumber) || string.IsNullOrEmpty(accountNumber) || amount <= 0)
                    {
                        return BadRequest("Invalid InRtgsCbc details. Please provide a valid mobile number, account number, and amount.");
                    }

                    try
                    {
                        var request = new InRtgsCbcDto
                        {
                            DepositorPhone = mobileNumber,
                            DAccountNo = accountNumber,
                            Amount = amount,
                            MemberId = memberId
                        };

                        var result = await _InRtgsCbcsqlRepository.CreateAndApproveInRtgsCbcAsync(request);
                        return Ok(result);
                    }
                    catch (Exception ex)
                    {
                        // Return a user-friendly error message
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

        */





    }


}
