using Gym.Domain;
using Gym.Infrastructure.Abstractions.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;

namespace Gym.UseCases.User.LoginUser;

/// <summary>
/// Handler for <see cref="LoginUserCommand" />.
/// </summary>
internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResult>
{
    private const string InvalidCredentialsErrorMessage = "Email or password is incorrect.";
    private readonly IAuthorizationService authorizationService;
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    public LoginUserCommandHandler(IAuthorizationService authorizationService, IAppDbContext dbContext)
    {
        this.authorizationService = authorizationService;
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<LoginUserCommandResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = null;
        try
        {
            string token = await authorizationService.AuthenticateAsync(request.Email, request.Password);
            userId = await authorizationService.GetMeAsync();
            var user = new LoggedUsers()
            {
                Id = userId.Value,
                Email = request.Email,
                Jwt = token,
                JwtUpdatedAt = DateTime.UtcNow
            };
            bool userExist = await dbContext.Users.AnyAsync(user => user.Id == userId, cancellationToken);
            if (!userExist)
            {
                dbContext.Users.Add(user);
            }
            else
            {
                dbContext.Users.Update(user);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (InfrastructureException exception)
        {
            throw new ValidationException(InvalidCredentialsErrorMessage);
        }

        return new LoginUserCommandResult
        {
            UserId = userId.Value,
        };
    }
}
