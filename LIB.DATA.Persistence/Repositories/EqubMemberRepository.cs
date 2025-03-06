using LIB.API.Application.Contracts.Persistence;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace LIB.API.Persistence.Repositories
{
    public class EqubMemberRepository : GenericRepository<EqubMembers>, IEqubMemberRepository
    {
        private readonly LIBAPIDbSQLContext _context;

        public EqubMemberRepository(LIBAPIDbSQLContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string> GenerateNextIdAsync()
        {
            // Get the last used ID from the database
          /*  var lastEqubMember = await _context.EqubMembers
                .OrderByDescending(e => e.Id)
                .FirstOrDefaultAsync();

       */     // Extract the numeric part of the ID and increment it
            int nextIdNumber = 1;
          /*  if (lastEqubMember != null)
            {
                if (int.TryParse(lastEqubMember.Id, out int lastIdNumber))
                {
                    nextIdNumber = lastIdNumber + 1;
                }
            }
*/
            // Format the next ID with leading zeros
            return nextIdNumber.ToString("D8"); // "D8" means 8 digits with leading zeros
        }


    }
}
