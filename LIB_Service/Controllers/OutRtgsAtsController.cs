
using AutoMapper;
using LIB.API.Application.Contracts.Persistence;



using MediatR;
using Microsoft.AspNetCore.Mvc;

using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.CQRS.OutRtgsAts.Request.Queries;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using LIB.API.Application.CQRS.OutRtgsAts.Request.Command;
using OfficeOpenXml;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LIBPROPERTY_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutRtgsAtsController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IOutRtgsAtsRepository _OutRtgsAtsRepository;
        private readonly IOutReconciledRepository _outReconciledRepository;

        public OutRtgsAtsController(IMediator mediator, IOutRtgsAtsRepository OutRtgsAtsRepository,IOutReconciledRepository outReconciledRepository)
        {
            _mediator = mediator;
            _OutRtgsAtsRepository = OutRtgsAtsRepository;
            _outReconciledRepository = outReconciledRepository;
        }

        // GET: api/<OutRtgsAtsController>
        [HttpGet]
        public async Task<ActionResult<List<OutRtgsAtsDto>>> Get()
        {
            var OutRtgsAtss = await _mediator.Send(new GetOutRtgsAtsListRequest());
            return Ok(OutRtgsAtss);
        }

        // GET api/<OutRtgsAtsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<OutRtgsAtsDto>>> Get(int id)
        {
            var OutRtgsAts = await _mediator.Send(new GetOutRtgsAtsDetailRequest { Id = id });
            return Ok(OutRtgsAts);
        }

        // POST api/<OutRtgsAtsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] OutRtgsAtsDto OutRtgsAts)
        {
            var command = new CreateOutRtgsAtsCommand { OutRtgsAtsDto = OutRtgsAts };
            await _mediator.Send(command);
            return Ok(command);

        }


        // DELETE api/<OutRtgsAtsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteOutRtgsAtsCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
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

                        string amount = worksheet.Cells[row, 11].Text.Trim();
                        string businessDateText = worksheet.Cells[row, 8].Text.Trim();
                        DateTime businessDate = Convert.ToDateTime(businessDateText);


                        if (string.IsNullOrEmpty(reference))
                        {
                            continue;
                        }

                        // Check if a matching record exists in OutRtgsAts or OutReconciled
                        var existingOutRtgsAts = await _OutRtgsAtsRepository.GetByRefNoAmountAndDate(reference, amount, businessDate);
                        var existingOutReconciled = await _outReconciledRepository.GetByRefNoAmountAndDate(reference, Convert.ToDecimal(amount), businessDate);

                        if (existingOutRtgsAts != null || existingOutReconciled != null)
                        {
                            // Skip this row if a matching record already exists
                            Console.WriteLine($"Skipping row {row} as a matching reference '{reference}', amount '{amount}', and business date '{businessDate}' already exist.");
                            continue;
                        }
                        var outRtgsAtsDto = new OutRtgsAtsDto
                        {

                            Type = worksheet.Cells[row, 2].Text,
                            Reference =reference,
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
                        var command = new CreateOutRtgsAtsCommand { OutRtgsAtsDto = outRtgsAtsDto };
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


    }

}
