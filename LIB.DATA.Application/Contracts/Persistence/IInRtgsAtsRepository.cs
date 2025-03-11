
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.Contracts.Persistence
{
    public interface IInRtgsAtsRepository : IGenericRepository<InRtgsAtss>
    {
        Task<InRtgsAtss> GetByName(string name);
        // Check if the ID already exists
        Task<List<InRtgsAtsDto>> GetInRtgsAtsDByDateIntervalAsync(DateTime startDate, DateTime endDate);

    }
}
