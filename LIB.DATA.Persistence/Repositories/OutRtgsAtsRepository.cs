using LIB.API.Application.Contracts.Persistence;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
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

    }
}
