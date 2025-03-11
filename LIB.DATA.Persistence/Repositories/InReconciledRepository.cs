using Dapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.DTOs.InReconciled;
using LIB.API.Application.DTOs.OutRtgsCbc;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using System.Threading.Tasks;

namespace LIB.API.Persistence.Repositories
{
    public class InReconciledRepository : GenericRepository<InReconcileds>, IInReconciledRepository
    {
        public readonly LIBAPIDbSQLContext _context;
  
        private readonly string _connectionString;
        public InReconciledRepository(LIBAPIDbSQLContext context,IConfiguration configuration) : base(context)
        {
            _context = context;
         
        
        }


        public async Task<List<InReconciledDto>> GetInReconciledByDateIntervalAsync(DateTime startDate, DateTime endDate)
        {
            // Format the startDate and endDate into the "dd-MMM-yy" format
       
            return await _context.InReconcileds
                .Where(t =>
                t.BusinessDate.Date >= startDate.Date && t.BusinessDate.Date <= endDate.Date)
                .Select(t => new InReconciledDto
                {
                    No = t.No,
                    BRANCH = t.BRANCH,
                    ACCOUNT = t.ACCOUNT,
                    DISCRIPTION = t.DISCRIPTION,
                    AMOUNT = t.AMOUNT,
                    INPUTING_BRANCH = t.INPUTING_BRANCH,
                    TRANSACTION_DATE = t.TRANSACTION_DATE,
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
