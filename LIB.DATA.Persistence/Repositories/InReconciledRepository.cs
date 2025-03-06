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
    public class InReconciledRepository : GenericRepository<InReconcileds>, IInReconciledRepository
    {
        public readonly LIBAPIDbSQLContext _context;
  
        private readonly string _connectionString;
        public InReconciledRepository(LIBAPIDbSQLContext context,IConfiguration configuration) : base(context)
        {
            _context = context;
         
        
        }

       


    }
}
