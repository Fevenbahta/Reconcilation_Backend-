
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.Contracts.Persistent
{
    public interface ITransactionRepository : IGenericRepositoryOracle<Transactions>
    {
        Task<AccountInfos> GetUserDetailsByAccountNumberAsync(string accountNumber);
        Task<UserData> GetUserDetailAsync(string branch, string userName, string role);
        Task<UserData> GetUserDetailAsyncByUserName( string userName);
  
 
        Task<string> CheckAccountBalanceAsync(string branch, string account, decimal amount);

    }
}
