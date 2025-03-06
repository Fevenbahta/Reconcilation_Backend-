
using AutoMapper;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.Transaction.Request.Command;
using LIB.API.Application.DTOs.Transaction;
using LIB.API.Domain;
using LIB.API.Persistence;
using LIBPROPERTY.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LIB.API.Persistence.Repositories
{
    public class TransactionSqlRepository : GenericRepository<Transactions>, ITransactionSqlRepository
    {
        private readonly LIBAPIDbSQLContext _context;
        private readonly IMapper _mapper;
        // private readonly ITransactionSqlRepository _transactionSqlRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMediator _mediator;

        public TransactionSqlRepository(LIBAPIDbSQLContext context, IMapper mapper, ITransactionRepository transactionRepository, IMediator mediator) : base(context)
        {
            _context = context;
            _mapper = mapper;
            //     _transactionSqlRepository = transactionSqlRepository;
            _transactionRepository = transactionRepository;
            _mediator = mediator;
        }

        public async Task<TransactionResult> CreateAndApproveTransactionAsync(TransactionDto request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validate the request
                if (string.IsNullOrEmpty(request.DepositorPhone) || string.IsNullOrEmpty(request.DAccountNo) || request.Amount <= 0)
                {
                    throw new ArgumentException("Invalid transaction details");
                }

                var accountDetails = await _transactionRepository.GetUserDetailsByAccountNumberAsync(request.DAccountNo);


                request.TransType = "Mobile Banking";
                request.MemberId = request.MemberId;
                request.CAccountBranch = accountDetails?.BRANCH ?? "";
                request.CAccountNo = "00310095104";
                request.CAccountOwner = "FANA YOUTH AND CREDIT LIMITED COOPERATIVE";
                request.DAccountNo = request.DAccountNo;
                request.DDepositeName = accountDetails?.FULL_NAME ?? "";
                request.DepositorPhone = request.DepositorPhone;
                request.Amount = request.Amount;
                request.Branch = "";
                request.CreatedBy = "";
                request.ApprovedBy = "";
                request.ReferenceNo = "";
                request.MesssageNo = "";
                request.PaymentNo = "";
                request.Status = "Pending";
                request.UpdatedDate = DateTime.UtcNow.ToString();
                request.UpdatedBy = "";




                var createTransactionCommand = new CreateTransactionCommand { TransactionDto = request };
                var createTransactionResult = await _mediator.Send(createTransactionCommand);

                var createdTransaction = true;

                if (createdTransaction == null)
                {
                    throw new InvalidOperationException($"Failed to retrieve newly created transaction with ID {createTransactionResult.Id}");
                }

                // Update the transaction
                var updateTransactionCommand = new UpdateTransactionCommand { TransactionDto = _mapper.Map<TransactionDto>(createdTransaction) };
                var updateTransactionResult = await _mediator.Send(updateTransactionCommand);

                // Commit the transaction
                await transaction.CommitAsync();

                return new TransactionResult
                {
                    Success = true,
                    TransactionId = createTransactionResult.Id
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                // Re-throw the exception to be handled by the controller
                throw new Exception("Error creating and approving transaction: " + ex.Message, ex);
            }
        }
        public async Task<List<TransactionDto>> GetTransactionsByDateIntervalAsync(DateTime startDate, DateTime endDate)
        {
          return await GetTransactionsByDateIntervalAsync(startDate, endDate);
        }

        public async Task<TransactionDto> GetTransactionByReferenceNoAsync(string referenceNo)
        {
            var transaction = true; 
            if (transaction == null) return null;

            return new TransactionDto
            {
            
            };
        }


    }
}
