namespace Gym.Domain;

/// <summary>
/// Logged user.
/// </summary>
public record LoggedUsers
{
    /// <summary>
    /// Logged user Id.
    /// </summary>
    /// <remarks>
    /// This Id equals id from server.
    /// </remarks>
    public Guid Id { get; init; }

    /// <summary>
    /// Email.
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// FirstName
    /// </summary>
    public string FirstName { get; init; }

    /// <summary>
    /// Last name.
    /// </summary>
    public string LastName { get; init; }

    /// <summary>
    /// Jwt token.
    /// </summary>
    public string Jwt { get; init; }

    /// <summary>
    /// When last jwt token was created.
    /// </summary>
    public DateTime JwtUpdatedAt { get; init; }
}
