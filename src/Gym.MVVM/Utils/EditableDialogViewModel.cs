using Gym.MVVM.ServiceAbstractions.Navigation;

namespace Gym.MVVM.Utils;

/// <summary>
/// Base class for all editable dialogs.
/// </summary>
/// <typeparam name="TModel">Type of the model to edit.</typeparam>
public abstract class EditableDialogViewModel<TModel> : EditableViewModel<TModel>
    where TModel : EditableModel
{
    private readonly IDialogService dialogService;

    /// <summary>
    /// Initializes a new instance of the `Editable Dialog` view model.
    /// </summary>
    /// <param name="dialogService">Dialog service.</param>
    public EditableDialogViewModel(IDialogService dialogService) : base(dialogService)
    {
        this.dialogService = dialogService;
    }

    /// <inheritdoc />
    protected override void Close()
    {
        dialogService.Close();
    }
}