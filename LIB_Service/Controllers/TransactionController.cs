using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.Transaction.Request.Command;
using LIB.API.Application.CQRS.Transaction.Request.Queries;
using LIB.API.Application.DTOs.Transaction;
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
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly LIBAPIDbSQLContext _context;
        private readonly ITransactionRepository _TransactionRepository;
        private readonly ITransactionSqlRepository _TransactionSqlRepository;
        private readonly UpdateLogService _updateLogService;
        private readonly IEqubMemberRepository _equbMemberRepository;
        private readonly IEqubTypeRepository _equbTypeRepository;

        public TransactionController(IMediator mediator, IMapper mapper, LIBAPIDbSQLContext context,
            ITransactionSqlRepository transactionSqlRepository,
            ITransactionRepository TransactionRepository,
            UpdateLogService updateLogService,
            IEqubMemberRepository equbMemberRepository,
            IEqubTypeRepository equbTypeRepository
            )
        {
            _updateLogService = updateLogService;
            _equbMemberRepository = equbMemberRepository;
            _equbTypeRepository = equbTypeRepository;
            _mediator = mediator;
            _mapper = mapper;
            _context = context;
            _TransactionSqlRepository = transactionSqlRepository;
            _TransactionRepository = TransactionRepository;
        }

        // GET: api/Transaction
        [HttpGet]
        public async Task<ActionResult<List<TransactionDto>>> GetAllTransactions()
        {
            var Transaction = await _mediator.Send(new GetTransactionListRequest());
            return Ok(Transaction);
        }

        // GET: api/Transaction/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<List<TransactionDto>>> GetTransactionById(int id)
        {
            var Transaction = await _mediator.Send(new GetTransactionDetaileRequest { Id = id });
            return Ok(Transaction);
        }

        // POST: api/Transaction
        [HttpPost]
        public async Task<ActionResult> CreateTransaction([FromBody] TransactionDto Transaction)
        {
            var command = new CreateTransactionCommand { TransactionDto = Transaction };
            await _mediator.Send(command);
            return Ok(command);
        }

        // PUT: api/Transaction/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, [FromBody] TransactionDto Transaction)
        {

            try
            {
 var command = new UpdateTransactionCommand { TransactionDto = Transaction };
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

        // DELETE: api/Transaction/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            var command = new DeleteTransactionCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        // GET: api/Transaction/GetUserDetailsByAccountNumber/{accountNumber}
        [HttpGet("GetUserDetailsByAccountNumber/{accountNumber}")]
        public async Task<IActionResult> GetUserDetailsByAccountNumber(string accountNumber)
        {
            var userDetails = await _TransactionRepository.GetUserDetailsByAccountNumberAsync(accountNumber);



            return Ok(userDetails);
        }
        [HttpGet("GetUserDetail")]
        public async Task<IActionResult> GetUserDetails(string branch, string userName, string role)
        {
            if (string.IsNullOrEmpty(branch) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(role))
            {
                return BadRequest("Branch, UserName, and Role are required parameters.");
            }

            var userDetails = await _TransactionRepository.GetUserDetailAsync(branch, userName, role);

            if (userDetails == null)
            {
                return NotFound();
            }

            return Ok(userDetails);
        }


        [HttpGet("GetUserDetailByUserName")]
        public async Task<IActionResult> GetUserDetailsByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("Branch, UserName, and Role are required parameters.");
            }

            var userDetails = await _TransactionRepository.GetUserDetailAsyncByUserName(userName);

            if (userDetails == null)
            {
                return NotFound();
            }

            return Ok(userDetails);
        }
        /*
                [HttpPost("CreateAndApproveTransaction")]
                public async Task<IActionResult> CreateAndApproveTransaction(string mobileNumber, string accountNumber, decimal amount, string memberId)
                {
                    if (string.IsNullOrEmpty(mobileNumber) || string.IsNullOrEmpty(accountNumber) || amount <= 0)
                    {
                        return BadRequest("Invalid transaction details. Please provide a valid mobile number, account number, and amount.");
                    }

                    try
                    {
                        var request = new TransactionDto
                        {
                            DepositorPhone = mobileNumber,
                            DAccountNo = accountNumber,
                            Amount = amount,
                            MemberId = memberId
                        };

                        var result = await _TransactionSqlRepository.CreateAndApproveTransactionAsync(request);
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


        [HttpGet("CheckAccountBalance")]
        public async Task<IActionResult> CheckAccountBalance([FromQuery] string branch, [FromQuery] string account, [FromQuery] decimal amount)
        {
            if (string.IsNullOrEmpty(branch) || string.IsNullOrEmpty(account) || amount <= 0)
            {
                return BadRequest("Invalid parameters.");
            }

            try
            {
                string result = await _TransactionRepository.CheckAccountBalanceAsync(branch, account, amount);

                if (result == "true")
                {
                    return Ok(new { success = true });
                }
                else if (result == "insufficient")
                {
                    return Ok(new { success = false, message = "Insufficient balance." });
                }
                else
                {
                    return StatusCode(500, "Unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                // Log exception (not shown here)
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("CreateAndApproveTransaction")]
        public async Task<IActionResult> CreateAndApproveTransaction([FromBody] CreateTransactionRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.DAccountNo))
                {
                    return BadRequest(new { message = ("Destination account number is required.") });
                }

                if (request.Amount <= 0)
                {
                    return BadRequest(new { message = ("Amount must be greater than zero.") });
                }

                if (string.IsNullOrEmpty(request.MemberId) || request.MemberId == "0")
                {
                    return BadRequest(new { message = ("Member ID is required and cannot be empty or '0'.") });
                }

                var accountDetails = await _TransactionRepository.GetUserDetailsByAccountNumberAsync(request.DAccountNo);
                int memberIdInt = int.Parse(request.MemberId);

                var equbMemberDetail = await _equbMemberRepository.GetById(memberIdInt);

                if (equbMemberDetail == null || equbMemberDetail.Status == "1")
                {
                    return NotFound(new { message = ($"Equb member with ID {memberIdInt} not found.") });
                }

                var equbTypeDetail = await _equbTypeRepository.GetByName(equbMemberDetail.EqubType);
                if (equbTypeDetail == null)
                {
                    return NotFound(new { message = ($"Equb type with name '{equbMemberDetail.EqubType}' not found.") });
                }
                else if (equbTypeDetail.Status == "Terminated")
                {
                    return NotFound(new { message = ($"Equb type with name '{equbMemberDetail.EqubType}' is Terminated.") });
                }




                decimal requiredAmount = decimal.Parse(equbTypeDetail.Amount);
                string penaltyMessage = string.Empty;

                if (equbTypeDetail.PenaltyStatus == "true")
                {
                    requiredAmount += decimal.Parse(equbMemberDetail.PenalityAmount);
                    penaltyMessage = $" Including penalties, the total required amount is {requiredAmount}.";

                }


                if (equbTypeDetail.PenaltyStatus == "true" && equbMemberDetail.TotalAmountLeft == 0 && 
                    decimal.Parse(equbMemberDetail.PaidPenalityAmount) >= decimal.Parse(equbMemberDetail.PenalityAmount))
                {
                    return BadRequest(new
                    {
                        message = "All amount and penalties have been paid"
                    });
                }
                if (equbTypeDetail.PenaltyStatus == "false" && equbMemberDetail.TotalAmountLeft == 0)
                {
                    return BadRequest(new
                    {
                        message = "All amount have been paid."
                    });
                }
                // Check if the provided amount meets or exceeds the required amount
                if (equbTypeDetail.AmountRequired == "true" && request.Amount < requiredAmount)
                {
                    return BadRequest(new
                    {
                        message = $"The required minimum amount for this EqubType is {equbTypeDetail.Amount}. " +
                  $"Please provide the correct amount.{penaltyMessage}"
                    });
                }
            
                
                
                var transactionDto = new TransactionDto
                {
                    TransType = "Mobile Banking",
                    MemberId = request.MemberId,
                    CAccountBranch = accountDetails?.BRANCH ?? "",
                    CAccountNo = "00310095104",
                    CAccountOwner = equbTypeDetail.EqubAccountNumber,
                    DAccountNo = request.DAccountNo,
                    DDepositeName = accountDetails?.FULL_NAME ?? "",
                    DepositorPhone = accountDetails?.TELEPHONENUMBER,
                    Amount = request.Amount,
                    Branch = "",
                    CreatedBy = "Mobile Banking",
                    ApprovedBy = "Mobile Banking",
                    ReferenceNo = "",
                    MesssageNo = "",
                    PaymentNo = "",
                    Status = "Pending",
                    UpdatedDate = DateTime.UtcNow.ToString(),
                    UpdatedBy = "",
                    EqupType = equbMemberDetail.EqubType
                };

                var balanceCheckResult = await CheckAccountBalance(transactionDto.CAccountBranch, request.DAccountNo, request.Amount);

                if (balanceCheckResult is OkObjectResult okResult && okResult.Value is { } value)
                {
                    var response = value as dynamic;

                    if (response.success != null && (bool)response.success == false)
                    {
                        return BadRequest(new { message = ("Insufficient balance.") });
                    }
                }
                else if (balanceCheckResult is StatusCodeResult statusCodeResult && statusCodeResult.StatusCode == 500)
                {
                    return StatusCode(500, "Invalid Account.");
                }
                else
                {
                    return StatusCode(500, "Invalid Account.");
                }

                var createTransactionCommand = new CreateTransactionCommand { TransactionDto = transactionDto };
                var createTransactionResult = await _mediator.Send(createTransactionCommand);

                int lastInsertedId = 1;
                if (lastInsertedId == default(int))
                {
                    throw new InvalidOperationException("Failed to retrieve last inserted ID.");
                }

                var createdTransaction = true;
                if (createdTransaction == null)
                {
                    throw new InvalidOperationException($"Failed to retrieve newly created transaction with ID {createTransactionResult.Id}");
                }

                var updateTransactionCommand = new UpdateTransactionCommand { TransactionDto = _mapper.Map<TransactionDto>(createdTransaction) };
                var updateTransactionResult = await _mediator.Send(updateTransactionCommand);

                if (updateTransactionResult == null)
                {
                    throw new InvalidOperationException("Update operation returned null result.");
                }

                return Ok(new TransactionResult
                {
                    Success = true,
                    TransactionId = createTransactionResult.Id
                });
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

        [HttpPost("GetTransactionsByDateInterval")]
        public async Task<IActionResult> GetTransactionsByDateInterval([FromBody] DateIntervalRequest request)
        {
            if (request.StartDate > request.EndDate)
            {
                return BadRequest(new { message = ("Start date cannot be later than end date.") });
            }

            try
            {
                // Get transactions from repository
                var transactions = await _TransactionSqlRepository.GetTransactionsByDateIntervalAsync(request.StartDate, request.EndDate);

                if (transactions == null || !transactions.Any())
                {
                    return NotFound(new { message = ("No transactions found for the given date interval.") });
                }

                // Map to TransactionSummaryDto
                var result = transactions.Select(t => new TransactionSummaryDto
                {
                    TransDate = t.TransDate,
                    TransType = t.TransType,
                    Id = t.Id,
                    Branch = t.Branch,
                    ApprovedBy = t.ApprovedBy,
                    DDepositeName = t.DDepositeName,
                    DAccountNo = t.DAccountNo,
                    CAccountBranch = t.CAccountBranch,
                    DepositorPhone = t.DepositorPhone,
                    MemberId = t.MemberId,
                    CAccountOwner = t.CAccountOwner,
                    CAccountNo = t.CAccountNo,
                    Amount = t.Amount
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log exception (not shown here)
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("GetTransactionByReferenceNo")]
        public async Task<ActionResult<TransactionSummaryDto>> GetTransactionByReferenceNo([FromBody] ReferenceNoRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ReferenceNo))
            {
                return BadRequest(new { message = ("Reference number is required.") });
            }

            try
            {
                var transaction = await _TransactionSqlRepository.GetTransactionByReferenceNoAsync(request.ReferenceNo);

                if (transaction == null)
                {
                    return NotFound(new { message = ($"No transaction found with reference number {request.ReferenceNo}.") });
                }

                // Map to TransactionSummaryDto
                var result = new TransactionSummaryDto
                {
                    TransDate = transaction.TransDate,
                    TransType = transaction.TransType,
                    Id = transaction.Id,
                    Branch = transaction.Branch,
                    ApprovedBy = transaction.ApprovedBy,
                    DDepositeName = transaction.DDepositeName,
                    DAccountNo = transaction.DAccountNo,
                    CAccountBranch = transaction.CAccountBranch,
                    DepositorPhone = transaction.DepositorPhone,
                    MemberId = transaction.MemberId,
                    CAccountOwner = transaction.CAccountOwner,
                    CAccountNo = transaction.CAccountNo,
                    Amount = transaction.Amount
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }


}
