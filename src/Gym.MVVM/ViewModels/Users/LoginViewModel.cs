using Gym.MVVM.ServiceAbstractions.Navigation;
using Gym.MVVM.ViewModels.Users.Models;
using Gym.UseCases.User.LoginUser;
using MediatR;
using Microsoft.Toolkit.Mvvm.Input;
using Saritasa.Tools.Domain.Exceptions;

namespace Gym.MVVM.ViewModels.Users
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IMediator mediator;

        /// <inheritdoc/>
        public LoginModel Model { get; private set; }

        /// <summary>
        /// Login command.
        /// </summary>
        public AsyncRelayCommand LoginCommand { get; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LoginViewModel(
            INavigationService navigationService,
            IMediator mediator)
        {
            this.navigationService = navigationService;
            this.mediator = mediator;

            LoginCommand = new AsyncRelayCommand(LoginCommandExecute);
            Model = new();
            Model.PropertyChanged += ModelPropertyChanged;
        }

        /// <inheritdoc/>
        public override async Task LoadAsync()
        {
            await base.LoadAsync();
        }

        private void ModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ErrorMessage = null;
        }

        private async Task LoginCommandExecute()
        {
            Model.Touch();

            if (!Model.IsValid)
            {
                ErrorMessage = "Please enter user name and password.";
                return;
            }

            try
            {
                IsBusy = true;
                var res = await mediator.Send(new LoginUserCommand()
                {
                    Email = Model.Username,
                    Password = Model.Password,
                });
                await Task.Delay(5000);
            }
            catch (ValidationException e)
            {
                ErrorMessage = e.Message;
                return;
            }
            finally
            {
                IsBusy = false;
            }

            // var discardTask = navigationService.OpenAsFirstAsync<ProductListViewModel>();
        }
    }
}
