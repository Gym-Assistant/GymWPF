namespace Gym.UseCases.User.LoginUser;

/// <summary>
/// Represents user login attempt to system.
/// </summary>
public class LoginUserCommandResult
{
    /// <summary>
    /// Logged user id (if success).
    /// </summary>
    public Guid UserId { get; set; }
}