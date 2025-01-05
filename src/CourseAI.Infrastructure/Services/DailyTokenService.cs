using CourseAI.Core.Security;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class DailyTokenService(
    IServiceProvider serviceProvider,
    ILogger<DailyTokenService> logger
) : IHostedService, IDisposable
{
    private const int TokensToAdd = 10;

    private Timer _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Daily Token Service is starting.");

        // Calculate the time until the next desired run time (e.g., midnight)
        var now = DateTime.Now;
        var nextRunTime = DateTime.Today.AddDays(1).AddHours(0); // Next midnight
        // var nextRunTime = now + TimeSpan.FromMinutes(1);
        var initialDelay = nextRunTime - now;

        if (initialDelay < TimeSpan.Zero)
        {
            initialDelay = TimeSpan.Zero;
        }

        // Set the timer to trigger the DoWork method every 24 hours
        _timer = new Timer(DoWork, null, initialDelay, TimeSpan.FromHours(24));
        // _timer = new Timer(DoWork, null, initialDelay, TimeSpan.FromMinutes(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        logger.LogInformation("Daily Token Service is working.");

        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        
        try
        {
            var users = await dbContext.Users.ToListAsync();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                if (!roles.Contains(Roles.Standard) && !roles.Contains(Roles.Enterprise) && roles.Contains(Roles.User))
                {
                    user.Tokens += TokensToAdd;
                }

                await dbContext.SaveChangesAsync();

                logger.LogInformation($"Successfully added {TokensToAdd} tokens to {users.Count} users.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding tokens to users.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Daily Token Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}