using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using System.Threading.Tasks;

namespace LIB.API.Persistence.Repositories
{
    public class InRtgsAtsRepository : GenericRepository<InRtgsAtss>, IInRtgsAtsRepository
    {
        public readonly LIBAPIDbSQLContext _context;

        public InRtgsAtsRepository(LIBAPIDbSQLContext context) : base(context)
        {
            _context = context;
        }

        public async Task<InRtgsAtss> GetByName(string name)
        {
            // Assuming you're using Entity Framework or similar ORM
            return await _context.InRtgsAtss
                                 .FirstOrDefaultAsync(e => e.Reference == name && e.Status!="1");
        }

        public async Task<List<InRtgsAtsDto>> GetInRtgsAtsDByDateIntervalAsync(DateTime startDate, DateTime endDate)
        {
            // Format the startDate and endDate into the "dd-MMM-yy" format


            return await _context.InRtgsAtss
                .Where(t =>
                  // Filter by BusinessDate or EntryDate using the same formatted logic
                  t.BusinessDate.Date >= startDate.Date && t.BusinessDate.Date <= endDate.Date)
                .Select(t => new InRtgsAtsDto
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
