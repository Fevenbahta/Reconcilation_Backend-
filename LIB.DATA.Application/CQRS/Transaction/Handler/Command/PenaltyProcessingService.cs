using System;
using System.Threading;
using System.Threading.Tasks;
using LIB.API.Application.CQRS.Transaction.Handler.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class PenaltyProcessingService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public PenaltyProcessingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Schedule the task to run immediately, then every 24 hours
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(24));
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var penaltyService = scope.ServiceProvider.GetRequiredService<PenaltyService>();
            try
            {
                await penaltyService.UpdatePenalties();
                await penaltyService.UpdateEqubTypeStatusIfCompleted();
                Console.WriteLine($"success updating penalties: ");

            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error updating penalties: {ex.Message}");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
