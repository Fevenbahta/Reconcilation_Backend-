using LIB.API.Application.CQRS.OutRtgsCbc.Handler.Command;
using LIB.API.Application.CQRS.OutRtgsCbc.Request.Command;
using LIB.API.Application.DTOs.OutRtgsCbc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class OutRtgsCbcScheduledService : BackgroundService
{
    private readonly ILogger<OutRtgsCbcScheduledService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public OutRtgsCbcScheduledService(ILogger<OutRtgsCbcScheduledService> logger, IServiceProvider serviceProvider)
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
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private async Task RunTaskAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<CreateOutRtgsCbcCommandHandler>();

            // Create the command object with necessary properties if needed
            var command = new CreateOutRtgsCbcCommand
            {
                // Ensure properties are set if required
                OutRtgsCbcDto = new OutRtgsCbcDto
                {
                    DESCRIPTION = "",
                    ACCOUNT = "",
                    REFNO = "",
                    DATET = "",
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