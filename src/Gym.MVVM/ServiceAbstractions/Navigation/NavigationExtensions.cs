using Gym.MVVM.ViewModels;

namespace Gym.MVVM.ServiceAbstractions.Navigation;

/// <summary>
/// Contains extension methods for navigation in the app.
/// </summary>
public static class NavigationExtensions
{
    /// <summary>
    /// Open new page and do not wait for it to be closed.
    /// </summary>
    /// <param name="viewModel">View model.</param>
    public static void Open(this INavigationService navigationService, BaseViewModel viewModel)
    {
        navigationService.OpenAsync(viewModel);
    }

    /// <summary>
    /// Open new page with parameters and do not wait for it to be closed.
    /// </summary>
    /// <typeparam name="TViewModel">View model type.</typeparam>
    /// <param name="parameters">View model constructor parameters.</param>
    public static void Open<TViewModel>(this INavigationService navigationService, params object[] parameters) where TViewModel : BaseViewModel
    {
        navigationService.OpenAsync<TViewModel>(parameters);
    }

    /// <summary>
    /// Open a new dialog and don't wait for it to be closed.
    /// </summary>
    /// <param name="viewModel">View model.</param>
    public static void Open(this IDialogService dialogService, BaseViewModel viewModel)
    {
        dialogService.OpenAsync(viewModel);
    }

    /// <summary>
    /// Open new dialog with parameters and don't wait for it to be closed.
    /// </summary>
    /// <typeparam name="TViewModel">View model type.</typeparam>
    /// <param name="parameters">View model constructor parameters.</param>
    public static void Open<TViewModel>(this IDialogService dialogService, params object[] parameters) where TViewModel : BaseViewModel
    {
        dialogService.OpenAsync<TViewModel>(parameters);
    }
}