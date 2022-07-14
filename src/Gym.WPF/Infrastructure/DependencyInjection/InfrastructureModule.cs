using Gym.Infrastructure.Abstractions.Interfaces;
using Gym.Infrastructure.Authorization;
using Gym.Infrastructure.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.WPF.Infrastructure.DependencyInjection;

/// <summary>
/// Register Infrastructure dependencies.
/// </summary>
public class InfrastructureModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiSettings>(configuration.GetSection("AppSettings").GetSection("Api"));
        services.AddSingleton<IAuthorizationService, AuthorizationService>();
    }
}
