using LIB.API.Application.CQRS.InRtgsCbc.Handler.Command;
using LIB.API.Application.CQRS.InRtgsCbc.Request.Command;
using LIB.API.Application.DTOs.InRtgsCbc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class InRtgsCbcScheduledService : BackgroundService
{
    private readonly ILogger<InRtgsCbcScheduledService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public InRtgsCbcScheduledService(ILogger<InRtgsCbcScheduledService> logger, IServiceProvider serviceProvider)
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
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task RunTaskAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<CreateInRtgsCbcCommandHandler>();

            // Create the command object with necessary properties if needed
            var command = new CreateInRtgsCbcCommand
            {
                // Ensure properties are set if required
                InRtgsCbcDto = new InRtgsCbcDto
                {
                    DISCRIPTION = "",
                    ACCOUNT = "",
                    REFNO = "",
                    TRANSACTION_DATE = DateTime.Now,
                    DEBITOR_NAME = "",
                    BRANCH = "",
                    INPUTING_BRANCH = ""

                }
            };

            // Execute the command
            var response = await handler.Handle(command, CancellationToken.None);
            _logger.LogInformation(response.Message);
        }
    }
}