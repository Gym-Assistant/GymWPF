using Gym.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Application abstraction for unit of work.
/// </summary>
public interface IAppDbContext
{
    #region Users

    /// <summary>
    /// Users.
    /// </summary>
    DbSet<LoggedUsers> Users { get; }

    #endregion

    /// <summary>
    /// Save pending changes.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
