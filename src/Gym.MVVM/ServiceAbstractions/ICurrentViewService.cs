using Gym.MVVM.ViewModels;

namespace Gym.MVVM.ServiceAbstractions;

/// <summary>
/// Allows interacting with the currently active view.
/// </summary>
public interface ICurrentViewService
{
    /// <summary>
    /// Get the currently active view model.
    /// </summary>
    /// <typeparam name="T">Owner type of this view model.</typeparam>
    /// <returns>View model reference.</returns>
    BaseViewModel GetCurrentViewModel<T>();

    /// <summary>
    /// Hide the topmost view.
    /// </summary>
    /// <returns>Disposable object that allows to show the view back when disposed.</returns>
    IDisposable HideTopView();

    /// <summary>
    /// Show the topmost view.
    /// </summary>
    void ShowTopView();
}