
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Persistence.Repositories;

using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PERSISTANCE.Services;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.DependencyInjection;
using LIB.API.Application.CQRS.Transaction.Handler.Command;
using LIB.API.Application.CQRS.OutRtgsCbc.Handler.Command;
using LIB.API.Application.CQRS.OutReconciled.Handler.Command;
using LIB.API.Application.CQRS.InRtgsCbc.Handler.Command;
using LIB.API.Application.CQRS.InReconciled.Handler.Command;



namespace LIB.API.Persistence
{
    public static partial class PersistenceServiceRegistrtion
    {
        public static IServiceCollection ConfigurePersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LIBAPIDbContext>(options => options.UseOracle(configuration.GetConnectionString("LIBAPIConnectionString")));

            services.AddDbContext<LIBAPIDbSQLContext>(options => options.UseSqlServer(configuration.GetConnectionString("LIBAPISQLConnectionString")));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
 
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddScoped<ITransactionSqlRepository, TransactionSqlRepository>();

            services.AddScoped<IEqubMemberRepository, EqubMemberRepository>();

            services.AddScoped<IEqubTypeRepository, EqubTypeRepository>();
            services.AddScoped<ILotteryRepository, LotteryRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<PenaltyService>();

            // Register the background service
            services.AddHostedService<PenaltyProcessingService>();

            services.AddHostedService<LotteryProcessingService>();
            services.AddScoped<LotteryService>();


            services.AddScoped<IInRtgsAtsRepository, InRtgsAtsRepository>();
            services.AddScoped<IOutRtgsAtsRepository, OutRtgsAtsRepository>();
            services.AddScoped<IOutRtgsCbcOracleRepository, OutRtgsCbcOracleRepository>();
            services.AddScoped<IOutRtgsCbcRepository, OutRtgsCbcRepository>();
            services.AddScoped<IInRtgsCbcRepository, InRtgsCbcRepository>();
            services.AddScoped<IOutReconciledRepository, OutReconciledRepository>();
            services.AddScoped<IInReconciledRepository,InReconciledRepository>();
            services.AddScoped<IInRtgsCbcOracleRepository, InRtgsCbcOracleRepository>();
            services.AddScoped<JwtService>();
            // Inside ConfigureServices method of Startup.cs
            services.AddScoped<UpdateLogService>();
            // services.AddSingleton<SoapClient>();
            services.AddHttpClient<SoapClient>();
            services.AddHttpClient<SoapClient2>();

            services.AddHttpClient();

            string connectionString = configuration.GetConnectionString("LIBAPIConnectionString");
            string connectionSqlString = configuration.GetConnectionString("LIBAPISQLConnectionString");

            // Register IDbConnection with a SqlConnection instance using the retrieved connection string
            //   services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

            services.AddScoped<IDbConnection>(_ => new OracleConnection(connectionString));
            services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionSqlString));
            services.AddScoped<CreateOutRtgsCbcCommandHandler>();

            services.AddHostedService<OutRtgsCbcScheduledService>();
            services.AddScoped<CreateOutReconciledCommandHandler>();
            services.AddHostedService<OutReconciledScheduledService>();

            services.AddScoped<CreateInRtgsCbcCommandHandler>();
            services.AddHostedService<InRtgsCbcScheduledService>();
            services.AddScoped<CreateInReconciledCommandHandler>();
            services.AddHostedService<InReconciledScheduledService>();


            return services;
        }
    }
}
