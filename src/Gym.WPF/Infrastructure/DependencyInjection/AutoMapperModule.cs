using Gym.UseCases.User;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.WPF.Infrastructure.DependencyInjection;

/// <summary>
/// Register AutoMapper dependencies.
/// </summary>
public class AutoMapperModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMappingProfile));
    }
}
