using LIB.API.Application.Contracts.Persistence;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
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


 
    }
}
