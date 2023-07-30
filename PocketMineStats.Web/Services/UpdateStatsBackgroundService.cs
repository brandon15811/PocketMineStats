namespace PocketMineStats.Services;

public class UpdateStatsBackgroundService : BackgroundService
{
    private readonly ILogger<UpdateStatsBackgroundService> _logger;
    private readonly UpdateStatsService _updateStatsService;

    public UpdateStatsBackgroundService(UpdateStatsService updateStatsService, ILogger<UpdateStatsBackgroundService> logger)
    {
        _logger = logger;
        _updateStatsService = updateStatsService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Stats processor running.");
        //Yield immediately to allow the host to continue starting up
        //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio#backgroundservice-base-class
        await Task.Yield();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _updateStatsService.RunStats();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing stats.");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
            
        _logger.LogInformation("Stats processor stopping.");
    }
}