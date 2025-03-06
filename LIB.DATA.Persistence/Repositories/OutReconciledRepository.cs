using Dapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
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


    }
}
