using AutoMapper;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.EqubMember.Request.Command;
using LIB.API.Application.CQRS.EqubMember.Request.Queries;
using LIB.API.Application.CQRS.Transaction.Request.Command;
using LIB.API.Application.CQRS.Transaction.Request.Queries;
using LIB.API.Application.DTOs.EqubMember;
using LIB.API.Application.DTOs.EqubMember.Validators;
using LIB.API.Application.DTOs.Transaction;
using LIB.API.Domain;
using LIB.API.Persistence;
using LIB.API.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeOpenXml;

using System.Data;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using LIB.API.Application.Contracts.Persistence;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Linq;
using LIB.API.Application.CQRS.Transaction.Handler.Command;


namespace LIBPROPERTY_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EqubMemberController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly LIBAPIDbSQLContext _context;
        private readonly IEqubTypeRepository _equbTypeRepository;
        private readonly IEqubMemberRepository _equbMemberRepository;
        private readonly PenaltyService _penaltyService;

        public EqubMemberController(IMediator mediator, LIBAPIDbSQLContext context,
            IEqubTypeRepository equbTypeRepository,IEqubMemberRepository equbMemberRepository, PenaltyService penaltyService)
        {
            _mediator = mediator;
            _context = context;
            _equbTypeRepository = equbTypeRepository;
            _equbMemberRepository = equbMemberRepository;
            _penaltyService = penaltyService;
        }

        // GET: api/<EqubMemberController>
        [HttpGet]
        public async Task<ActionResult<List<EqubMemberDto>>> Get()
        {
            var EqubMembers = await _mediator.Send(new GetEqubMemberListRequest());
            return Ok(EqubMembers);
        }

        [HttpPost("equbMembers/GetById")]
        public async Task<ActionResult<EqubMemberSummaryDto>> Get([FromBody] GetEqubMemberRequest request)
        {
            try
            {
                // Validate request
                if (request == null || request.Id <= 0)
                {
                    return BadRequest("A valid ID must be provided.");
                }

                // Retrieve the EqubMember based on the ID
                var equbMember = await _mediator.Send(new GetEqubMemberDetailRequest { Id = request.Id });

                // Check if EqubMember was found
                if (equbMember == null)
                {
                    return NotFound(new { message = "No EqubMember found for the specified ID." });
                }

                // Map to EqubMemberSummaryDto
                var result = new EqubMemberSummaryDto
                {
                    FullName = equbMember.FullName,
                    EqubType = equbMember.EqubType,
                    TotalAmountPaid = equbMember.TotalAmountPaid,
                    EqubAmountLeft = equbMember.TotalAmountLeft,
                    PenalityAmount = equbMember.PenalityAmount,
                    PaidPenalityAmount = equbMember.PaidPenalityAmount,
                    TotalAmountLeft = equbMember.TotalAmountLeft + (decimal.Parse(equbMember.PenalityAmount))
                };

                return Ok(result);
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
                                 }
                };

          return NotFound(new { message = "No EqubMember found for the specified ID." });
                ;
            }
        }
        // POST api/<EqubMemberController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EqubMemberDto EqubMember)
        {
            var command = new CreateEqubMemberCommand { EqubMemberDto = EqubMember };
            await _mediator.Send(command);
            return Ok(command);

        }


        // DELETE api/<EqubMemberController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var command = new DeleteEqubMemberCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        private int GetTotalPeriods(string timeUnits, string timeQuantity, DateTime startDate, DateTime endDate)
        {
            int totalDays = (endDate - startDate).Days +1 ;
            int periodQuantity = int.Parse(timeQuantity);

            switch (timeUnits.ToLower())
            {
                case "day":
                    return totalDays / periodQuantity;
                case "week":
                    return totalDays / (7 * periodQuantity);
                case "month":
                    return totalDays / (30 * periodQuantity); // Approximation
                default:
                    throw new ArgumentException("Invalid time units");
            }
        }
        [HttpPost("importEqubMembers")]
        public async Task<IActionResult> ImportEqubMembers([FromForm] IFormFile file, [FromQuery] string equbType)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file");
            }

            if (string.IsNullOrEmpty(equbType))
            {
                return BadRequest("EqubType is required");
            }

            try
            {
                var equbMembers = new List<EqubMembers>();

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        return BadRequest("The Excel file does not contain any worksheets.");
                    }

                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        return BadRequest("The Excel file does not contain the specified worksheet.");
                    }

                    var rowCount = worksheet.Dimension.Rows;
                    var equbTypeRecord = await _equbTypeRepository.GetByName(equbType);
                    DateTime startDate = equbTypeRecord.EqupStartDay;
                    DateTime endDate = equbTypeRecord.EqupEndDay;

                    int totalPeriods = GetTotalPeriods(equbTypeRecord.TimeUnits, equbTypeRecord.TimeQuantity, startDate, endDate);
                    decimal totalAmountToPay = decimal.Parse(equbTypeRecord.Amount) * totalPeriods;

                    for (int row = 2; row <= rowCount; row++) // Assuming the first row is header
                    {
                        var fullName = worksheet.Cells[row, 1].Value?.ToString().Trim();
                        var phoneNo = worksheet.Cells[row, 2].Value?.ToString().Trim();

                        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phoneNo))
                        {
                            continue; // Skip rows with missing data
                        }

                        // Generate the next ID
                        var newId = await _equbMemberRepository.GenerateNextIdAsync();

                        var equbMember = new EqubMembers
                        {
                            Id = newId, // Generate a new ID
                            EqubType = equbType,
                            FullName = fullName,
                            PhoneNo = phoneNo,
                            CompletedStatus = "Incomplete",
                            NoOfTimesPaid = 0,
                            TotalAmountPaid = 0,
                            PenalityAmount = "0",
                            PaidPenalityAmount = "0",
                            PenalityType = "",
                            TotalAmountLeft = totalAmountToPay,
                            Status = "Active",
                            UpdatedDate = DateTime.Now.ToString(),
                            UpdatedBy = "system"
                        };

                            await _context.SaveChangesAsync();
                    }
                }


                return Ok(new { message = "Data imported successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpPost("updatePenalties")]
    public async Task<IActionResult> UpdatePenalties()
    {
        try
        {
            await _penaltyService.UpdatePenalties();
            return Ok(new { message = "Penalties updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error: {ex.Message}");
        }
    }
}
    }


    

