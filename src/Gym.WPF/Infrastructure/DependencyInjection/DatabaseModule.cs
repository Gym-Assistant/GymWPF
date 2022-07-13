using System;
using Gym.Infrastructure.Abstractions.Interfaces;
using Gym.Infrastructure.DataAccess;
using Gym.WPF.Infrastructure.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.WPF.Infrastructure.DependencyInjection;

/// <summary>
/// Register database dependencies.
/// </summary>
internal static class DatabaseModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        var migrationAssembly = typeof(AppDbContext).Assembly.GetName().Name;
        var x = configuration.GetConnectionString("AppDatabase");
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(
            configuration.GetConnectionString("AppDatabase") ?? throw new ArgumentNullException("AppDatabase"),
            sqlOptions => sqlOptions.MigrationsAssembly(migrationAssembly)
        ));
        services.AddTransient<DatabaseInitializer>();
        services.AddScoped<IAppDbContext, AppDbContext>();
    }
}
