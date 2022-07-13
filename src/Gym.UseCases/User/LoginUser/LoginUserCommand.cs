using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gym.UseCases.User.LoginUser
{
    /// <summary>
    /// Login user command.
    /// </summary>
    public record LoginUserCommand : IRequest<LoginUserCommandResult>
    {
        /// <summary>
        /// Email.
        /// </summary>
        [EmailAddress]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; init; }

        /// <summary>
        /// Password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }
    }
}
