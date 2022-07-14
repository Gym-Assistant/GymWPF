using MediatR;

namespace Gym.UseCases.User.RestoreToken;

/// <summary>
/// Restore token command.
/// </summary>
public record RestoreTokenCommand() : IRequest<bool>;
