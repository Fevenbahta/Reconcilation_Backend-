using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using LIB.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.Contracts.Persistence
{
    public interface IOutRtgsAtsRepository : IGenericRepository<OutRtgsAtss>
    {
        Task<OutRtgsAtss> GetByRefNoAmountAndDate(string reference, string amount, DateTime businessDate);
        Task<List<OutRtgsAtsDto>> GetOutRtgsAtsDByDateIntervalAsync(DateTime startDate, DateTime endDate);


    }
}
