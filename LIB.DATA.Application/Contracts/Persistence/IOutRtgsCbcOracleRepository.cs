
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
    public interface IOutRtgsCbcOracleRepository : IGenericRepositoryOracle<OutRtgsCbcs>
    {
        Task<string> GenerateNextIdAsync();
        Task<List<OutRtgsCbcsOracle>> GetOutRtgsCbcsByDateAsync(DateTime transactionDate);
        Task<UserData> GetUserDetailAsyncByUserName(string userName);
    }
}
