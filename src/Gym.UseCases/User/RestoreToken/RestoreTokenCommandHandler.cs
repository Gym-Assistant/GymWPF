using Gym.Infrastructure.Abstractions.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;

namespace Gym.UseCases.User.RestoreToken;

/// <summary>
/// Handler for <see cref="RestoreTokenCommand"/>.
/// </summary>
public class RestoreTokenCommandHandler : IRequestHandler<RestoreTokenCommand, bool>
{
    private readonly IAppDbContext dbContext;
    private readonly IAuthorizationService authorizationService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public RestoreTokenCommandHandler(IAppDbContext dbContext, IAuthorizationService authorizationService)
    {
        this.dbContext = dbContext;
        this.authorizationService = authorizationService;
    }

    /// <inheritdoc />
    public async Task<bool> Handle(RestoreTokenCommand request, CancellationToken cancellationToken)
    {
        var lastUserLogged = await dbContext.Users
            .Where(user => !string.IsNullOrEmpty(user.Jwt))
            .OrderByDescending(user => user.JwtUpdatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastUserLogged == null)
        {
            return false;
        }
        authorizationService.RestoreJwtToken(lastUserLogged.Jwt);
        return true;
    }
}
