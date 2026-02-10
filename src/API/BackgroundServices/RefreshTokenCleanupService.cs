using JuliGastos.Application.Interfaces.Repositories;

namespace JuliGastos.API.BackgroundServices;

public class RefreshTokenCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RefreshTokenCleanupService> _logger;

    public RefreshTokenCleanupService(
        IServiceProvider serviceProvider,
        ILogger<RefreshTokenCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RefreshTokenCleanupService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var refreshTokenRepository = scope.ServiceProvider
                        .GetRequiredService<IRefreshTokenRepository>();

                    await refreshTokenRepository.DeleteExpiredAsync();
                    _logger.LogInformation("Expired refresh tokens deleted");
                }

                // Ejecutar limpieza una vez al día
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during refresh token cleanup");
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        _logger.LogInformation("RefreshTokenCleanupService stopped");
    }
}
