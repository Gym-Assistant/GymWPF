using Gym.MVVM.ServiceAbstractions.Navigation;

namespace Gym.MVVM.Utils;

/// <summary>
/// Base class for editable view models to be displayed as a page.
/// </summary>
/// <typeparam name="TModel">Type of model to edit.</typeparam>
public abstract class EditablePageViewModel<TModel> : EditableViewModel<TModel>
    where TModel : EditableModel
{
    private readonly INavigationService navigationService;

    /// <summary>
    /// Initializes a new instance of the `Editable Page` view model.
    /// </summary>
    /// <param name="dialogService">Dialog service.</param>
    /// <param name="navigationService">Navigation service.</param>
    public EditablePageViewModel(
        IDialogService dialogService,
        INavigationService navigationService) : base(dialogService)
    {
        this.navigationService = navigationService;
    }

    /// <inheritdoc/>
    protected override void Close()
    {
        navigationService.GoBack();
    }
}