using System.Threading.Tasks;
using Gym.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Gym.WPF.Infrastructure.Startup;

/// <summary>
/// Contains database migration helper methods.
/// </summary>
internal sealed class DatabaseInitializer
{
    private readonly AppDbContext appDbContext;

    /// <summary>
    /// Database initializer. Performs migration and data seed.
    /// </summary>
    /// <param name="appDbContext">Data context.</param>
    public DatabaseInitializer(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        appDbContext.Database.EnsureCreated();
        await appDbContext.Database.MigrateAsync();
    }
}
