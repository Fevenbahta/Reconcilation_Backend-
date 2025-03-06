using Dapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
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
        /*        public async Task<IEnumerable<OutRtgsCbcs>> GetOutRtgsCbcsByDateAsync(DateTime transactionDate)
                {
                    var query = @"
                    SELECT BRANCH, ACCOUNT, REFNO, AMOUNT, INPUTING_BRANCH, DEBITOR_NAME, DESCRIPTION, DATET
                    FROM anbesaprod.rtgs_txn
                    WHERE transaction_date = :transactionDate";

                    using (var connection = new OracleConnection(_connectionString))
                    {
                        var transactionDateParameter = new OracleParameter
                        {
                            ParameterName = "transactionDate",
                            OracleDbType = OracleDbType.Date,
                            Value = transactionDate
                        };

                        await connection.OpenAsync();

                        return await connection.QueryAsync<OutRtgsCbcs>(
                            query,
                            new { transactionDate = transactionDateParameter });
                    }
                }
        */

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
