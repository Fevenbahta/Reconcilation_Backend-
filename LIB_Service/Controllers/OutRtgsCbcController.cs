using AutoMapper;
using LIB.API.Application.CQRS.OutRtgsCbc.Request.Command;

using LIB.API.Persistence;
using LIB.API.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using LIB.API.Application.Contracts.Persistence;

using LIB.API.Application.DTOs.OutRtgsCbc;
using LIB.API.Application.CQRS.OutRtgsAts.Request.Queries;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using LIB.API.Application.CQRS.OutRtgsCbc.Request.Queries;
using LIB.API.Application.DTOs.OutReconciled;


namespace LIBPROPERTY_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutRtgsCbcController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly LIBAPIDbSQLContext _context;
        private readonly InRtgsAtsRepository inRtgsAtsRepository;
        private readonly IInRtgsAtsRepository _InRtgsAtsRepository;
        private readonly IOutRtgsCbcRepository _OutRtgsCbcRepository;
 

        public OutRtgsCbcController(IMediator mediator, LIBAPIDbSQLContext context,
            IInRtgsAtsRepository InRtgsAtsRepository, IOutRtgsCbcRepository OutRtgsCbcRepository)
        {
            _mediator = mediator;
            _context = context;
            _InRtgsAtsRepository = InRtgsAtsRepository;
            _OutRtgsCbcRepository = OutRtgsCbcRepository;
          
        }

        [HttpGet]
        public async Task<ActionResult<List<OutRtgsCbcDto>>> Get()
        {
            var OutRtgsCbcs = await _mediator.Send(new GetOutRtgsCbcListRequest());
            return Ok(OutRtgsCbcs);
        }

        // GET: api/<OutRtgsCbcController>


        /*     [HttpGet]
        public async Task<ActionResult<List<OutRtgsCbcDto>>> Get()
        {
            var OutRtgsCbcs = await _mediator.Send(new GetOutRtgsCbcListRequest());
            return Ok(OutRtgsCbcs);
        }*/


        // POST api/<OutRtgsCbcController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] OutRtgsCbcDto OutRtgsCbc)
        {
            var command = new CreateOutRtgsCbcCommand { OutRtgsCbcDto = OutRtgsCbc };
            await _mediator.Send(command);
            return Ok(command);

        }


        // DELETE api/<OutRtgsCbcController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var command = new DeleteOutRtgsCbcCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("dateRange")]
        public async Task<ActionResult<List<OutRtgsCbcDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            // Call the mediator to get the data within the specified date range
            var result = await _OutRtgsCbcRepository.GetOutRtgsCbcDByDateIntervalAsync(startDate, endDate);
            return Ok(result);
        }
        private int GetTotalPeriods(string timeUnits, string timeQuantity, DateTime startDate, DateTime endDate)
        {
            int totalDays = (endDate - startDate).Days + 1;
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
        /*    [HttpPost("importOutRtgsCbcs")]
            public async Task<IActionResult> ImportOutRtgsCbcs([FromForm] IFormFile file, [FromQuery] string InRtgsAts)
            {
                if (file == null || file.Length <= 0)
                {
                    return BadRequest("Invalid file");
                }

                if (string.IsNullOrEmpty(InRtgsAts))
                {
                    return BadRequest("InRtgsAts is required");
                }

                try
                {
                    var OutRtgsCbcs = new List<OutRtgsCbcs>();

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
                        var InRtgsAtsRecord = await _InRtgsAtsRepository.GetByName(InRtgsAts);
                        DateTime startDate = InRtgsAtsRecord.EqupStartDay;
                        DateTime endDate = InRtgsAtsRecord.EqupEndDay;

                        int totalPeriods = GetTotalPeriods(InRtgsAtsRecord.TimeUnits, InRtgsAtsRecord.TimeQuantity, startDate, endDate);
                        decimal totalAmountToPay = decimal.Parse(InRtgsAtsRecord.Amount) * totalPeriods;

                        for (int row = 2; row <= rowCount; row++) // Assuming the first row is header
                        {
                            var fullName = worksheet.Cells[row, 1].Value?.ToString().Trim();
                            var phoneNo = worksheet.Cells[row, 2].Value?.ToString().Trim();

                            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phoneNo))
                            {
                                continue; // Skip rows with missing data
                            }

                            // Generate the next ID
                            var newId = await _OutRtgsCbcRepository.GenerateNextIdAsync();

                            var OutRtgsCbc = new OutRtgsCbcs
                            {
                                Id = newId, // Generate a new ID
                                InRtgsAts = InRtgsAts,
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

                            _context.OutRtgsCbcs.Add(OutRtgsCbc);
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
    */

    }
    }


    

