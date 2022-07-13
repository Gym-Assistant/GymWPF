using System;
using Gym.MVVM.ServiceAbstractions;
using Gym.MVVM.ViewModels;

namespace Gym.WPF.Infrastructure.Navigation;

/// <summary>
/// Provides access to the currently active view.
/// </summary>
internal class CurrentViewService : ICurrentViewService
{
    private readonly NavigationStack navigationStack;

    /// <summary>
    /// Constructor.
    /// </summary>
    public CurrentViewService(NavigationStack navigationStack)
    {
        this.navigationStack = navigationStack;
    }

    /// <inheritdoc />
    public BaseViewModel GetCurrentViewModel<T>()
    {
        return navigationStack.Peek<T>()?.ViewModel;
    }

    /// <inheritdoc />
    public IDisposable HideTopView()
    {
        var view = navigationStack.Peek();
        if (view == null)
        {
            return System.Reactive.Disposables.Disposable.Empty;
        }

        view.ToggleVisibility(false);
        return System.Reactive.Disposables.Disposable.Create(() =>
        {
            view.ToggleVisibility(true, updateFrameVisibility: false);

            // In case if the view we are showing is not the top view at the moment
            var topView = navigationStack.Peek();
            topView?.EnsureFrameVisibility();
        });
    }

    /// <inheritdoc />
    public void ShowTopView()
    {
        var view = navigationStack.Peek();
        if (view == null)
        {
            return;
        }

        view.ToggleVisibility(true);
    }
}