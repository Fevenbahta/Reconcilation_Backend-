using LIB.API.Application.Contracts.Persistent;
using LIB.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.Contracts.Persistence
{
    public interface ILotteryRepository : IGenericRepository<Lotteries>
    {
        Task<DateTime?> GetLastLotteryDateByEqubType(string equbType);
        Task<int> GetNextLotteryRound(string equbType);
        Task AddLot(Lotteries lottery);
        Task<List<Lotteries>> GetWinnersByEqubType(string equbType);
        Task<Lotteries> GetLatestLotteryByEqubType(string equbTypeName);

    }
}
