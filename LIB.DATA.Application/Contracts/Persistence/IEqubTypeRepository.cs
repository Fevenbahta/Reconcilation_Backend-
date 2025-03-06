using LIB.API.Application.Contracts.Persistent;
using LIB.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.Contracts.Persistence
{
    public interface IEqubTypeRepository : IGenericRepository<EqubTypes>
    {
        Task<EqubTypes> GetByName(string name);
        Task<string> GetLastIdAsync();
        Task UpdateLastIdAsync(string newId);
        Task<bool> ExistsString(string id); // Check if the ID already exists

    }
}
