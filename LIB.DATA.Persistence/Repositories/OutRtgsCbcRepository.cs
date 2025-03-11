using Dapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.DTOs.OutRtgsCbc;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using System.Threading.Tasks;

namespace LIB.API.Persistence.Repositories
{
    public class OutRtgsCbcRepository : GenericRepository<OutRtgsCbcs>, IOutRtgsCbcRepository
    {
        public readonly LIBAPIDbSQLContext _context;
        private readonly LIBAPIDbContext _context1;
        private readonly string _connectionString;
        public OutRtgsCbcRepository(LIBAPIDbSQLContext context, LIBAPIDbContext context1,IConfiguration configuration) : base(context)
        {
            _context = context;
           _context1 = context1;
            _connectionString = configuration.GetConnectionString("LIBAPIConnectionString");
        }

        public async Task<string> GenerateNextIdAsync()
        {
            // Get the last used ID from the database
            var lastOutRtgsCbc = await _context.OutRtgsCbcs
             
                .FirstOrDefaultAsync();

            // Extract the numeric part of the ID and increment it
            int nextIdNumber = 1;
            if (lastOutRtgsCbc != null)
            {
                if (int.TryParse(lastOutRtgsCbc.REFNO, out int lastIdNumber))
                {
                    nextIdNumber = lastIdNumber + 1;
                }
            }

            // Format the next ID with leading zeros
            return nextIdNumber.ToString("D8"); // "D8" means 8 digits with leading zeros
        }
        public async Task<List<OutRtgsCbcDto>> GetOutRtgsCbcDByDateIntervalAsync(DateTime startDate, DateTime endDate)
        {
            // Use raw SQL to perform the comparison at the database level
            string formattedStartDate = startDate.ToString("dd-MMM-yy", CultureInfo.InvariantCulture);
            string formattedEndDate = endDate.ToString("dd-MMM-yy", CultureInfo.InvariantCulture);

            return await _context.OutRtgsCbcs
                .FromSqlRaw("SELECT * FROM OutRtgsCbcs WHERE CONVERT(DATE, DATET, 3) >= {0} AND CONVERT(DATE, DATET, 3) <= {1}", formattedStartDate, formattedEndDate)
                .Select(t => new OutRtgsCbcDto
                {
                    REFNO = t.REFNO,
                    DATET = t.DATET,
                    INPUTING_BRANCH = t.INPUTING_BRANCH,
                    AMOUNT = t.AMOUNT,
                    DISCRIPTION = t.DISCRIPTION,
                    DEBITOR_NAME = t.DEBITOR_NAME,
                    ACCOUNT = t.ACCOUNT,
                    BRANCH = t.BRANCH
                })
                .ToListAsync();
        }




        public async Task<string?> GetLastProcessedDateAsync()
        {
            var lastRecord = await _context.OutRtgsCbcs
                .OrderByDescending(l => l.DATET)  // Assuming DATET is the date field
                .Select(l => l.DATET)  // Select only the date field
                .FirstOrDefaultAsync();

            return lastRecord;
        }

    }
}
