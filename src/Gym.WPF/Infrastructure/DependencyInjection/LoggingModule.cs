using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gym.WPF.Infrastructure.DependencyInjection;

/// <summary>
/// Register logging dependencies.
/// </summary>
internal static class LoggingModule
{
    /// <summary>
    /// Indicates if application is running in a console mode.
    /// </summary>
    public static bool IsConsole { get; set; }

    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(builder =>
        {
            if (IsConsole)
            {
                builder.AddConsole();
            }
#if DEBUG
            builder.AddDebug();
#endif
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddConfiguration(configuration);
        });
    }
}