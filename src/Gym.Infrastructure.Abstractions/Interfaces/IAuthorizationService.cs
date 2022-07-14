namespace Gym.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Authorization service.
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Authenticate.
    /// </summary>
    /// <param name="login">Login</param>
    /// <param name="password">Password.</param>
    /// <returns>Access token.</returns>
    Task<string> AuthenticateAsync(string login, string password);

    /// <summary>
    /// Get current user id.
    /// </summary>
    /// <returns>User id.</returns>
    Task<Guid?> GetMeAsync();

    /// <summary>
    /// Update access token.
    /// </summary>
    /// <returns>New access token.</returns>
    Task<string> RefreshTokenAsync();

    /// <summary>
    /// Restore jwt token.
    /// </summary>
    /// <param name="token">Token for restore.</param>
    void RestoreJwtToken(string token);
}
