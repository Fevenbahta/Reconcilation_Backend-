using LIB.API.Application.Contracts.Persistence;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace LIB.API.Persistence.Repositories
{
    public class LotteryRepository : GenericRepository<Lotteries>, ILotteryRepository
    {
        private readonly LIBAPIDbSQLContext _context;

        public LotteryRepository(LIBAPIDbSQLContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DateTime?> GetLastLotteryDateByEqubType(string equbType)
        {
            var lastRecord = DateTime.Now;

            return lastRecord;
        }

        public async Task<int> GetNextLotteryRound(string equbType)
        {
            var lastRound = 1;
            return lastRound + 1;
        }

        public async Task AddLot(Lotteries lottery)
        {
            var lastRecord = DateTime.Now;

           
        }

        public async Task<List<Lotteries>> GetWinnersByEqubType(string equbType)
        {
            var lastRecord =new List<Lotteries>();

            return lastRecord;
        }
        public async Task<Lotteries> GetLatestLotteryByEqubType(string equbTypeName)
        {
            var lastRecord = new Lotteries();

            return lastRecord;
        }

    }
}
