


using LIB.API.Application.CQRS.InReconciled.Handler.Command;
using LIB.API.Application.CQRS.InReconciled.Request.Command;
using LIB.API.Application.DTOs.InReconciled;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class InReconciledScheduledService : BackgroundService
{
    private readonly ILogger<InReconciledScheduledService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public InReconciledScheduledService(ILogger<InReconciledScheduledService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Check the time and execute tasks accordingly
            var now = DateTime.Now;
            if (now.Hour >= 6 && now.Hour < 17) // 17 corresponds to 5 PM
            {
                await RunTaskAsync();
            }
            // Sleep for an hour before the next check
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task RunTaskAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<CreateInReconciledCommandHandler>();

            // Create the command object with necessary properties if needed
            var command = new CreateInReconciledCommand
            {
                // Ensure properties are set if required
                InReconciledDto = new InReconciledDto
                {
                    No = 0,
                    BRANCH = "",
                    ACCOUNT = "",
                    TRANSACTION_DATE = DateTime.Now,
                    DISCRIPTION = "",
                    INPUTING_BRANCH = "",
                    AMOUNT = 0,
                    Type = "",
                    Reference = "",
                    Debitor = "",
                    Creditor = "",
                    OrderingAccount = "",
                    BeneficiaryAccount = "",
                    BusinessDate = DateTime.Now,
                    EntryDate = DateTime.Now,
                    Currency = "",
                    ProcessingStatus = "",
                    Status = "",

                }
            };

            // Execute the command
            var response = await handler.Handle(command, CancellationToken.None);
            _logger.LogInformation(response.Message);
        }
    }
}