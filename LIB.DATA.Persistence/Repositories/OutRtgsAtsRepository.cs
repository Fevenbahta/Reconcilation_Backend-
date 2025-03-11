using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using LIB.API.Application.DTOs.OutRtgsCbc;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using System.Threading.Tasks;

namespace LIB.API.Persistence.Repositories
{
    public class OutRtgsAtsRepository : GenericRepository<OutRtgsAtss>, IOutRtgsAtsRepository
    {
        public readonly LIBAPIDbSQLContext _context;

        public OutRtgsAtsRepository(LIBAPIDbSQLContext context) : base(context)
        {
            _context = context;
        }
        public async Task<OutRtgsAtss> GetByRefNoAmountAndDate(string reference, string amount, DateTime businessDate)
        {
            return await _context.OutRtgsAtss
                .FirstOrDefaultAsync(x => x.Reference == reference
                                       && x.Amount == amount
                                       && x.BusinessDate == businessDate);
        }



        public async Task<List<OutRtgsAtsDto>> GetOutRtgsAtsDByDateIntervalAsync(DateTime startDate, DateTime endDate)
        {


            return await _context.OutRtgsAtss
                .Where(t =>
                // Filter by BusinessDate or EntryDate using the same formatted logic
                t.BusinessDate.Date >= startDate.Date && t.BusinessDate.Date <= endDate.Date)
                .Select(t => new OutRtgsAtsDto
                {
                    No = t.No,
                    Type = t.Type,
                    Reference = t.Reference,
                    Debitor = t.Debitor,
                    Creditor = t.Creditor,
                    OrderingAccount = t.OrderingAccount,
                    BeneficiaryAccount = t.BeneficiaryAccount,
                    BusinessDate = t.BusinessDate,
                    EntryDate = t.EntryDate,
                    Currency = t.Currency,
                    Amount = t.Amount,
                    ProcessingStatus = t.ProcessingStatus,
                    Status = t.Status
                })
                .ToListAsync();
        }
    }
}
