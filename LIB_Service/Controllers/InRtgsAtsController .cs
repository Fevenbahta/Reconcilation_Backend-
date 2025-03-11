using AutoMapper;
using LIB.API.Application.CQRS.InRtgsAts.Request.Command;
using LIB.API.Application.CQRS.InRtgsAts.Request.Queries;

using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using System.Globalization;
using System.Text.RegularExpressions;
using LIB.API.Application.DTOs.OutReconciled;
using LIB.API.Persistence.Repositories;


namespace LIBPROPERTY_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InRtgsAtsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInRtgsAtsRepository _inRtgsAtsRepository;

        public InRtgsAtsController(IMediator mediator,IInRtgsAtsRepository inRtgsAtsRepository)
        {
            _mediator = mediator;
            _inRtgsAtsRepository = inRtgsAtsRepository;
        }

        // GET: api/<InRtgsAtsController>
        [HttpGet]
        public async Task<ActionResult<List<InRtgsAtsDto>>> Get()
        {
            var InRtgsAtss = await _mediator.Send(new GetInRtgsAtsListRequest());
            return Ok(InRtgsAtss);
        }
        // GET: api/<InRtgsAtsController>
        [HttpGet("All")]
        public async Task<ActionResult<List<InRtgsAtsDto>>> GetAll()
        {
            var InRtgsAtss = await _mediator.Send(new GetInRtgsAtsAllListRequest());
            return Ok(InRtgsAtss);
        }

        // GET api/<InRtgsAtsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<InRtgsAtsDto>>> Get(string id)
        {
            var InRtgsAts = await _mediator.Send(new GetInRtgsAtsDetailRequest { Id = id });
            return Ok(InRtgsAts);
        }

        // POST api/<InRtgsAtsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InRtgsAtsDto InRtgsAts)
        {
            var command = new CreateInRtgsAtsCommand { InRtgsAtsDto = InRtgsAts };
            await _mediator.Send(command);
            return Ok(command);

        }
        // PUT api/<InRtgsAtsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] InRtgsAtsDto InRtgsAts)
        {
            if (id != InRtgsAts.Reference)
            {
                return BadRequest("ID mismatch");
            }

            var command = new UpdateInRtgsAtsCommand { InRtgsAtsDto = InRtgsAts };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpGet("dateRange")]
        public async Task<ActionResult<List<InRtgsAtsDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            // Call the mediator to get the data within the specified date range
            var result = await _inRtgsAtsRepository.GetInRtgsAtsDByDateIntervalAsync(startDate, endDate);
            return Ok(result);
        }


        // DELETE api/<InRtgsAtsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var command = new DeleteInRtgsAtsCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            string[] expectedHeaders = new[]
   {
        "No.", "Type", "Reference", "Debtor", "Creditor",
        "Ordering Account", "Beneficiary Account", "Business Date",
        "Entry Date", "Currency", "Amount", "Processing Status", "Status"
    };
            try
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheetCount = package.Workbook.Worksheets.Count;

                    // Log number of worksheets
                    Console.WriteLine($"Number of worksheets: {worksheetCount}");

                    if (worksheetCount == 0)
                    {
                        return BadRequest("No worksheets found in the file.");
                    }

                    // Log worksheet names
                    foreach (var sheet in package.Workbook.Worksheets)
                    {
                        Console.WriteLine($"Worksheet name: {sheet.Name}");
                    }

                    // Ensure to use the correct index or name
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault(); // Accessing the first worksheet or adjust if necessary

                    if (worksheet == null)
                    {
                        return BadRequest("Unable to access the first worksheet.");
                    }

                    if (worksheet.Dimension == null)
                    {
                        return BadRequest("Worksheet is empty.");
                    }


                    for (int col = 1; col <= expectedHeaders.Length; col++)
                    {
                        string header = worksheet.Cells[1, col].Text.Trim();



                        if (header != expectedHeaders[col - 1])
                        {
                            return BadRequest($"Invalid header in column {col}: Expected '{expectedHeaders[col - 1]}', but found '{header}'.");
                        }
                    }
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // assuming the first row is the header

                    {

                        string type = worksheet.Cells[row, 2].Text.Trim();

                        // Skip rows where Type is not 202 or 103
                        if (type != "202" && type != "103")
                        {
                            continue;
                        }


                        string orderingAccount = worksheet.Cells[row, 6].Text.Trim();
                        string beneficiaryAccount = worksheet.Cells[row, 7].Text.Trim();
                        string reference = worksheet.Cells[row, 3].GetValue<string>()?.Trim() ?? string.Empty;

                        if (string.IsNullOrEmpty(reference))
                        {
                            continue;
                        }
                        var inRtgsAtsDto = new InRtgsAtsDto
                        {

                            Type = worksheet.Cells[row, 2].Text,
                            Reference = reference,
                            Debitor = worksheet.Cells[row, 4].Text,
                            Creditor = worksheet.Cells[row, 5].Text,
                            OrderingAccount = ConvertScientificNotationToNormalString(orderingAccount),
                            BeneficiaryAccount = ConvertScientificNotationToNormalString(beneficiaryAccount),

                            BusinessDate = DateTime.ParseExact(worksheet.Cells[row, 8].Text, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
                            EntryDate = DateTime.ParseExact(worksheet.Cells[row, 9].Text, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),

                            Currency = worksheet.Cells[row, 10].Text,
                            Amount = worksheet.Cells[row, 11].Text,
                            ProcessingStatus = worksheet.Cells[row, 12].Text,
                            Status = worksheet.Cells[row, 13].Text
                        };

                        // Use mediator to handle the imported data
                        var command = new CreateInRtgsAtsCommand { InRtgsAtsDto = inRtgsAtsDto };
                        await _mediator.Send(command);
                    }
                }
                return Ok("File imported successfully.");
            }
            catch (FormatException ex)
            {
                // Handle parsing errors
                return BadRequest($"Data format error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        private string ConvertScientificNotationToNormalString(string value)
        {
            // Handle common fraction formats and return as plain text
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            // Regex to match common fraction patterns like "1/2", "3/4", etc.
            var fractionRegex = new Regex(@"^\d+/\d+$");
            if (fractionRegex.IsMatch(value))
            {
                return value; // Return the fraction as is
            }

            // Check for scientific notation and convert if necessary
            if (decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal number))
            {
                return number.ToString("G", CultureInfo.InvariantCulture); // "G" for general format
            }

            return value; // Return the original string if parsing fails
        }

    }

}