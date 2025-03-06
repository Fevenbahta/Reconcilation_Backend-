using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class LotteryProcessingService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public LotteryProcessingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Schedule the task to run immediately, then every hour
        //_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var lotteryService = scope.ServiceProvider.GetRequiredService<LotteryService>();
            try
            {
                await lotteryService.RunLottery();
                Console.WriteLine($"Lottery processed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error processing lottery: {ex.Message}");
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
