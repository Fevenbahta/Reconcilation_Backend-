using Dapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.DTOs.InReconciled;
using LIB.API.Application.DTOs.OutReconciled;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using System.Threading.Tasks;

namespace LIB.API.Persistence.Repositories
{
    public class OutReconciledRepository : GenericRepository<OutReconcileds>, IOutReconciledRepository
    {
        public readonly LIBAPIDbSQLContext _context;
  
        private readonly string _connectionString;
        public OutReconciledRepository(LIBAPIDbSQLContext context,IConfiguration configuration) : base(context)
        {
            _context = context;
         
        
        }

        public async Task<OutReconcileds> GetByRefNoAmountAndDate(string reference, decimal amount, DateTime businessDate)
        {
            return await _context.outReconcileds
                .FirstOrDefaultAsync(x => x.Reference == reference
                                       && x.AMOUNT == amount
                                       && x.BusinessDate == businessDate);
        }

        public async Task<List<OutReconciledDto>> GetInReconciledByDateIntervalAsync(DateTime startDate, DateTime endDate)
        {
            
            return await _context.outReconcileds
                .Where(t =>
                     t.BusinessDate.Date >= startDate.Date && t.BusinessDate.Date <= endDate.Date)
                .Select(t => new OutReconciledDto
                {
                    No = t.No,
                    BRANCH = t.BRANCH,
                    ACCOUNT = t.ACCOUNT,
                    DISCRIPTION = t.DISCRIPTION,
                    AMOUNT = t.AMOUNT,
                    INPUTING_BRANCH = t.INPUTING_BRANCH,
                    DATET = t.DATET,
                    Type = t.Type,
                    Reference = t.Reference,
                    Debitor = t.Debitor,
                    Creditor = t.Creditor,
                    OrderingAccount = t.OrderingAccount,
                    BeneficiaryAccount = t.BeneficiaryAccount,
                    BusinessDate = t.BusinessDate,
                    EntryDate = t.EntryDate,
                    Currency = t.Currency,
                    ProcessingStatus = t.ProcessingStatus,
                    Status = t.Status
                })
                .ToListAsync();
        }



    }
}
