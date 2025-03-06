using LIB.API.Application.Contracts.Persistence;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace LIB.API.Persistence.Repositories
{
    public class EqubTypeRepository : GenericRepository<EqubTypes>, IEqubTypeRepository
    {
        private readonly LIBAPIDbSQLContext _context;

        public EqubTypeRepository(LIBAPIDbSQLContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EqubTypes> GetByName(string name)
        {
            // Assuming you're using Entity Framework or similar ORM
            var equals = new EqubTypes();
            return equals;
        }


        public async Task<string> GetLastIdAsync()
        {
            // Retrieve the last ID from the EqubTypes table
            var lastRecord = "test";

            return lastRecord ; // Return default value if no records exist
        }

        public async Task UpdateLastIdAsync(string newId)
        {
            // Update the last ID in the EqubTypes table
            var lastRecord = "test";

          
        }

        public async Task<bool> ExistsString(string id)
        {
            // Check if the ID already exists in the repository
            var lastRecord = "test";

            return true;
        }

    }
}
