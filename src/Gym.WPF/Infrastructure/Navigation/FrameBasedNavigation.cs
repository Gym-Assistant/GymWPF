using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using Gym.MVVM.ServiceAbstractions.Navigation;
using Gym.MVVM.ViewModels;

namespace Gym.WPF.Infrastructure.Navigation;

/// <summary>
/// Implements a navigation using a single frame for displaying the current view.
/// </summary>
/// <typeparam name="T">
/// Type of view that are allowed to be navigated.
/// Only views that have a <see cref="UsesViewModelAttribute"/> attribute specified are allowed to be uesd with this service.
/// </typeparam>
internal class FrameBasedNavigation<T>
{
    private readonly Frame frame;
    private readonly Dictionary<Type, Type> viewModelToViewAssociation;
    private readonly NavigationStack navigationStack;

    private TaskCompletionSource navigatingFinishTaskCompletionSource;
    private TaskCompletionSource frameNavigationClearingTaskCompletionSource;

    /// <summary>
    /// Auto hide.
    /// </summary>
    public bool AutoHide { get; }

    /// <summary>
    /// Initializes a new frame based navigation service.
    /// </summary>
    /// <param name="frame">Frame control.</param>
    /// <param name="navigationStack">Navigation stack.</param>
    /// <param name="autoHide">Auto hide flag.</param>
    public FrameBasedNavigation(Frame frame, NavigationStack navigationStack, bool autoHide)
    {
        this.frame = frame;
        this.frame.Navigating += FrameNavigating;
        this.frame.Navigated += FrameOnNavigated;
        this.navigationStack = navigationStack;
        AutoHide = autoHide;

        // Find all existing pages and generate a viewmodel - page mapping
        viewModelToViewAssociation = new Dictionary<Type, Type>();
        var pageTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(T)));
        foreach (var pageType in pageTypes)
        {
            var viewModelAttribute = pageType.GetCustomAttribute<UsesViewModelAttribute>();
            if (viewModelAttribute == null)
            {
                continue;
            }

            viewModelToViewAssociation.Add(viewModelAttribute.ViewModelType, pageType);
        }
    }

    /// <summary>
    /// Open the specified view model and close previous pages if any exists.
    /// </summary>
    /// <typeparam name="TViewModel">Type of the view model.</typeparam>
    /// <param name="viewModel">View model instance.</param>
    public async Task OpenAsFirstAsync<TViewModel>(TViewModel viewModel) where TViewModel : BaseViewModel
    {
        frameNavigationClearingTaskCompletionSource?.TrySetCanceled();
        frameNavigationClearingTaskCompletionSource = new TaskCompletionSource();

        // We need to push clearing page to the navigation stack to clear the all real pages.
        // The last page is not in the frame history.
        frame.Navigate(new ClearingPage());

        await frameNavigationClearingTaskCompletionSource.Task;

        ClearFrameHistory();
        await OpenInternal(viewModel);
    }

    private void ClearFrameHistory()
    {
        if (!frame.CanGoBack && !frame.CanGoForward)
        {
            return;
        }

        var model = navigationStack.Pop<T>();
        var entry = frame.RemoveBackEntry();

        while (model != null && entry != null)
        {
            model.NavigationResult.SetCanceled();
            model = navigationStack.Pop<T>();
            entry = frame.RemoveBackEntry();
        }

        if (model != null || entry != null)
        {
            // We should always have identical amount of frames and view models.
            // It is an invalid state for the application if amount is not identical.
            throw new InvalidOperationException("Amount of frames and view models was not identical.");
        }
    }

    /// <summary>
    /// Open the specified view model.
    /// </summary>
    /// <param name="viewModel">View model instance.</param>
    public Task Open(BaseViewModel viewModel)
    {
        return OpenInternal(viewModel);
    }

    /// <summary>
    /// Open the specified view model and get a result from it.
    /// </summary>
    /// <typeparam name="TViewModel">Type of the view model.</typeparam>
    /// <typeparam name="TResult">Type of the result to receive from it.</typeparam>
    /// <param name="viewModel">View model instance.</param>
    /// <returns>View model result.</returns>
    public Task<TResult> Open<TViewModel, TResult>(TViewModel viewModel) where TViewModel : BaseViewModel, IWithResult<TResult>
    {
        return OpenInternal(viewModel).ContinueWith(result => viewModel.Result);
    }

    private async Task OpenInternal(BaseViewModel viewModel)
    {
        var state = new ViewState(frame)
        {
            ViewModel = viewModel,
            NavigationResult = new TaskCompletionSource(),
        };
        navigationStack.Push<T>(state);

        var page = GetPage(viewModel.GetType());
        page.DataContext = viewModel;

        await WaitForNavigatingFinish();

        frame.Navigate(page);
        await state.NavigationResult.Task;
    }

    /// <summary>
    /// Close the currently opened view.
    /// </summary>
    public void Close()
    {
        if (frame.CanGoBack)
        {
            frame.GoBack();
        }
    }

    private void FrameNavigating(object sender, NavigatingCancelEventArgs e)
    {
        switch (e.NavigationMode)
        {
            case NavigationMode.Back:
                var currentView = navigationStack.Peek<T>();

                if (!currentView.ViewModel.CanGoBack())
                {
                    e.Cancel = true;
                    return;
                }

                navigationStack.Pop<T>();
                currentView.NavigationResult.SetResult();
                currentView.ViewModel.Dispose();
                break;
            default:
                break;
        }

        navigatingFinishTaskCompletionSource?.TrySetCanceled();
        navigatingFinishTaskCompletionSource = new TaskCompletionSource();
    }

    private void FrameOnNavigated(object sender, NavigationEventArgs e)
    {
        navigatingFinishTaskCompletionSource?.TrySetResult();

        if (e.Content is ClearingPage)
        {
            frameNavigationClearingTaskCompletionSource?.TrySetResult();
        }
        else
        {
            var currentView = navigationStack.Peek<T>();
            currentView.EnsureFrameVisibility();
            frame.Focus();
        }
    }

    private Task WaitForNavigatingFinish()
    {
        if (navigatingFinishTaskCompletionSource != null)
        {
            return navigatingFinishTaskCompletionSource.Task;
        }

        return Task.CompletedTask;
    }

    private Page GetPage(Type viewModelType)
    {
        if (!viewModelToViewAssociation.TryGetValue(viewModelType, out var viewType))
        {
            throw new ArgumentException($"Cannot find a view type for the {viewModelType} type.");
        }

        return (Page)Activator.CreateInstance(viewType);
    }

    private class ClearingPage : PageFunctionBase
    {
        public ClearingPage()
        {
            // We should not add the page to the frame history.
            RemoveFromJournal = true;
        }
    }
}