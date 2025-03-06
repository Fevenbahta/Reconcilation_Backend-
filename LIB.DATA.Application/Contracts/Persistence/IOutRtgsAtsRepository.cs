using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
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


    }
}
