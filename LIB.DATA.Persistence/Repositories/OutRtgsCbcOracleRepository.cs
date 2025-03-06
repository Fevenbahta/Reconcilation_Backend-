
using Dapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace LIB.API.Persistence.Repositories
{
    public class OutRtgsCbcOracleRepository : GenericRepositoryOracle<OutRtgsCbcs>, IOutRtgsCbcOracleRepository
    {

        private readonly LIBAPIDbContext _context;
        private readonly ILogger<OutRtgsCbcOracleRepository> _logger;
        private readonly string _connectionString;
        public OutRtgsCbcOracleRepository(LIBAPIDbContext context, IConfiguration configuration, ILogger<OutRtgsCbcOracleRepository> logger) : base(context)
        {

            _context = context;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("LIBAPIConnectionString");
        }

        public async Task<string> GenerateNextIdAsync()
        {
            // Get the last used ID from the database
            var lastOutRtgsCbc = await _context.OutRtgsCbcsOracle
                .OrderByDescending(e => e.REFNO)
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

        public async Task<List<OutRtgsCbcsOracle>> GetOutRtgsCbcsByDateAsync(DateTime transactionDate)
        {
            var formattedDate = transactionDate.ToString("dd-MMM-yy").ToUpper();
            var query = @"
           SELECT BRANCH, ACCOUNT, AMOUNT, INPUTING_BRANCH, DEBITOR_NAME, DISCRIPTION,transaction_date AS DATET,REFNO
  
            FROM anbesaprod.out_rtgs_txnr
            WHERE transaction_date = :formattedDate";

            var parameters = new[]
            {
            new OracleParameter("formattedDate",formattedDate) 
        };

            try
            {
                // Execute the query with parameters
                var result = await _context.OutRtgsCbcsOracle
                    .FromSqlRaw(query, parameters)
                    .ToListAsync();

                return result;
            }
            catch (OracleException ex)
            {
                // Log Oracle-specific exceptions
                _logger.LogError(ex, "Oracle error occurred while retrieving data for date {TransactionDate}", transactionDate);
                throw new Exception("An error occurred while retrieving data from the database. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                // Log general exceptions
                _logger.LogError(ex, "An error occurred while retrieving data for date {TransactionDate}", transactionDate);
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }

        public async Task<UserData> GetUserDetailAsyncByUserName(string userName)
        {
            var query = @"
SELECT *
FROM anbesaprod.users2
WHERE USER_NAME = :userName";

            var userNameParameter = new OracleParameter("userName", userName);

            var userDetails = await _context.userDatas
                .FromSqlRaw(query, userNameParameter)
                .FirstOrDefaultAsync();

            return userDetails;
        }
           }
    }
