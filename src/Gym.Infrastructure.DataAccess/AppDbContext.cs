using Gym.Domain;
using Gym.Infrastructure.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.DataAccess;

/// <summary>
/// application db context.
/// </summary>
public class AppDbContext : DbContext, IAppDbContext
{
    /// <summary>
    /// Users set.
    /// </summary>
    public DbSet<LoggedUsers> Users { get; protected set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
