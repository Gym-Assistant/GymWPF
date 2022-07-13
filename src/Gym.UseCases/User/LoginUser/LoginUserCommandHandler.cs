using MediatR;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.Domain.Exceptions;

namespace Gym.UseCases.User.LoginUser
{
    /// <summary>
    /// Handler for <see cref="LoginUserCommand" />.
    /// </summary>
    internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResult>
    {
        private const string InvalidCredentialsErrorMessage = "Email or password is incorrect.";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="signInManager">Sign in manager.</param>
        /// <param name="tokenService">Token service.</param>
        /// <param name="logger">Logger.</param>
        public LoginUserCommandHandler()
        {
        }

        /// <inheritdoc />
        public async Task<LoginUserCommandResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("asdas");
            return new LoginUserCommandResult
            {
                UserId = 0,
            };
        }
    }
}
