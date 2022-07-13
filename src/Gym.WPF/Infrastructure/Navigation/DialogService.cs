using System.Threading.Tasks;
using System.Windows.Controls;
using Gym.MVVM.ServiceAbstractions;
using Gym.MVVM.ServiceAbstractions.Navigation;
using Gym.MVVM.Utils;
using Gym.MVVM.ViewModels;
using Gym.WPF.Views;

namespace Gym.WPF.Infrastructure.Navigation;

/// <summary>
/// Service for opening / closing the dialogs.
/// </summary>
public class DialogService : IDialogService
{
    private readonly FrameBasedNavigation<BaseDialog> frameNavigation;
    private readonly ViewModelFactory viewModelFactory;

    /// <summary>
    /// Constructor.
    /// </summary>
    public DialogService(Frame dialogFrame,
        ViewModelFactory viewModelFactory,
        NavigationStack navigationStack,
        ICurrentViewService currentViewService)
    {
        this.viewModelFactory = viewModelFactory;
        frameNavigation = new FrameBasedNavigation<BaseDialog>(dialogFrame, navigationStack, true);

        // Add a dummy dialog view model to hide the dialog overlay
        frameNavigation.Open(new BackgroundDialogViewModel());
        currentViewService.HideTopView();
    }

    /// <inheritdoc />
    public void Close()
    {
        frameNavigation.Close();
    }

    /// <inheritdoc />
    public Task OpenAsync<TViewModel>(params object[] parameters) where TViewModel : BaseViewModel
    {
        var viewModel = viewModelFactory.Create<TViewModel>(parameters);
        return OpenAsync(viewModel);
    }

    /// <inheritdoc />
    public async Task OpenAsync(BaseViewModel viewModel)
    {
        await frameNavigation.Open(viewModel);
    }

    /// <inheritdoc />
    public Task<TResult> OpenAsync<TViewModel, TResult>(params object[] parameters) where TViewModel : BaseViewModel, IWithResult<TResult>
    {
        var viewModel = viewModelFactory.Create<TViewModel>(parameters);
        return OpenAsync<TViewModel, TResult>(viewModel);
    }

    /// <inheritdoc />
    public async Task<TResult> OpenAsync<TViewModel, TResult>(TViewModel viewModel) where TViewModel : BaseViewModel, IWithResult<TResult>
    {
        return await frameNavigation.Open<TViewModel, TResult>(viewModel);
    }

    /// <inheritdoc/>
    public DialogBuilder GetDialogBuilder(string title)
        => new DialogBuilder(title, this);
}

/// <summary>
/// A dummy class that is used to indicate that there are no more views left in the dialog stack to show.
/// Instance of this class if always the bottom most element in view stack.
/// </summary>
internal sealed class BackgroundDialogViewModel : BaseViewModel
{
}

/// <summary>
/// A dummy class that is used to indicate that there are no more views left in the dialog stack to show.
/// Instance of this class if always the bottom most element in view stack.
/// </summary>
[UsesViewModel(typeof(BackgroundDialogViewModel))]
internal sealed class BackgroundDialog : BaseDialog
{
}