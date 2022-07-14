namespace Gym.Infrastructure.Authorization;

/// <summary>
/// Authorization response.
/// </summary>
public record AuthorizationResponse
{
    /// <summary>
    /// Authorization token.
    /// </summary>
    public string Token { get; init; }

    /// <summary>
    /// Token life time.
    /// </summary>
    public int expiresIn { get; init; }
}
