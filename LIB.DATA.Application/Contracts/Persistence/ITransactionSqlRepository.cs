
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.DTOs.Transaction;
using LIB.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.Contracts.Persistent
{
    public interface ITransactionSqlRepository : IGenericRepository<Transactions>
    {
        Task<TransactionResult> CreateAndApproveTransactionAsync(TransactionDto request);
        Task<List<TransactionDto>> GetTransactionsByDateIntervalAsync(DateTime startDate, DateTime endDate);
        Task<TransactionDto> GetTransactionByReferenceNoAsync(string referenceNo);
    }
}
