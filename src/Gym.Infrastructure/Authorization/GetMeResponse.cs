namespace Gym.Infrastructure.Authorization;

/// <summary>
/// User dto.
/// </summary>
public record GetMeResponse
{
    /// <summary>
    /// User email.
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// Last login.
    /// </summary>
    public DateTime LastLogin { get; init; }

    /// <summary>
    /// User id.
    /// </summary>
    public Guid Id { get; init; }
}
