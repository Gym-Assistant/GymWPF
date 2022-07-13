using Gym.MVVM.ViewModels;

namespace Gym.MVVM.ServiceAbstractions.Navigation;

/// <summary>
/// Service for opening / closing the application dialogs.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Open new dialog with parameters.
    /// </summary>
    /// <typeparam name="TViewModel">View model type.</typeparam>
    /// <param name="parameters">View model constructor parameters.</param>
    Task OpenAsync<TViewModel>(params object[] parameters) where TViewModel : BaseViewModel;

    /// <summary>
    /// Open a new dialog.
    /// </summary>
    /// <param name="viewModel">View model instance.</param>
    Task OpenAsync(BaseViewModel viewModel);

    /// <summary>
    /// Open a new dialog with parameters.
    /// </summary>
    /// <typeparam name="TViewModel">View model type.</typeparam>
    /// <typeparam name="TResult">Type of the dialog result.</typeparam>
    /// <param name="parameters">View model constructor parameters.</param>
    /// <returns>Dialog result.</returns>
    Task<TResult> OpenAsync<TViewModel, TResult>(params object[] parameters) where TViewModel : BaseViewModel, IWithResult<TResult>;

    /// <summary>
    /// Open new dialog.
    /// </summary>
    /// <param name="viewModel">View model.</param>
    /// <returns>Dialog result.</returns>
    Task<TResult> OpenAsync<TViewModel, TResult>(TViewModel viewModel) where TViewModel : BaseViewModel, IWithResult<TResult>;

    /// <summary>
    /// Close the topmost opened dialog.
    /// </summary>
    void Close();

    /// <summary>
    /// Get a dialog builder.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    DialogBuilder GetDialogBuilder(string title);
}