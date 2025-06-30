using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models.Enums;

namespace server.Services.Background
{
    public class SessionActivationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SessionActivationService> _logger;
        private readonly short _ActivationHourInterval = 2; // Reasonable -> can be adjusted later

        public SessionActivationService(IServiceProvider serviceProvider, ILogger<SessionActivationService> logger) {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                try {
                    await ActivatePlannedSessions();
                }
                catch (Exception ex) {
                    _logger.LogError(ex, "An error occurred while activating planned sessions");
                }

                await Task.Delay(TimeSpan.FromHours(_ActivationHourInterval), stoppingToken);
            }
        }

        private async Task ActivatePlannedSessions() {
            using (var scope = _serviceProvider.CreateScope()) {
                _logger.LogInformation("Checking for planned sessions to activate...");
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();

                // Get planned sessions that are due for activation
                var sessionsToActivate = await dbContext.Sessions
                    .Where(s => s.Status == SessionStatus.Planned && s.ActivationDate.HasValue && s.ActivationDate.Value <= DateTime.UtcNow)
                    .Include(s => s.Template)
                    .ToListAsync();

                if (sessionsToActivate.Any()) {
                    _logger.LogInformation($"Found {sessionsToActivate.Count} sessions to activate");

                    foreach (var session in sessionsToActivate) {
                        try {
                            await sessionService.StartSessionAsync(session.Id, session.Template.OwnerId);
                        }
                        catch (Exception ex) {
                            _logger.LogError(ex, $"Failed to activate session {session.Id}");
                        }
                    }
                }
            }
        }
    }
}