using Gym.MVVM.ServiceAbstractions.Navigation;
using Gym.MVVM.ViewModels;
using Gym.MVVM.ViewModels.Common;
using Microsoft.Toolkit.Mvvm.Input;
using PropertyChanged;

namespace Gym.MVVM.Utils;

/// <summary>
/// Base class for editable view models.
/// </summary>
/// <typeparam name="TModel"></typeparam>
public abstract class EditableViewModel<TModel> : BaseViewModel, IWithResult<bool>
    where TModel : EditableModel
{
    private readonly IDialogService dialogService;

    /// <summary>
    /// Data model.
    /// </summary>
    public abstract TModel Model { get; protected set; }

    /// <summary>
    /// Was the model edited.
    /// </summary>
    [DoNotNotify]
    public bool IsDirty => Model.IsDirty;

    /// <inheritdoc/>
    public RelayCommand CancelCommand { get; }

    /// <inheritdoc/>
    public RelayCommand SaveCommand { get; }

    /// <inheritdoc/>
    public bool Result { get; private set; }

    private volatile bool isSaving;

    /// <summary>
    /// Indicates if view model is currently saving the data.
    /// </summary>
    protected bool IsSaving => isSaving;

    /// <summary>
    /// Initializes a new instance of the editable view model.
    /// </summary>
    /// <param name="dialogService">Dialog service.</param>
    public EditableViewModel(IDialogService dialogService)
    {
        this.dialogService = dialogService;
        SaveCommand = new RelayCommand(async () => await SaveCommandExecute(), () => !isSaving);
        CancelCommand = new RelayCommand(async () => await CancelCommandExecute());
    }

    /// <summary>
    /// Cancel command method.
    /// </summary>
    protected virtual async Task CancelCommandExecute()
    {
        var needToRefresh = false;
        if (Model.IsDirty)
        {
            var result = await dialogService.OpenAsync<SaveViewModel, SaveDialogResult>(Model.IsValid);
            if (result == SaveDialogResult.Save)
            {
                await SaveCommandExecute();
            }
            if (result == SaveDialogResult.Discard)
            {
                Close(needToRefresh);
            }
        }
        else
        {
            Close(needToRefresh);
        }
    }

    private async Task SaveCommandExecute()
    {
        if (isSaving)
        {
            return;
        }

        isSaving = true;
        OnPropertyChanged(nameof(IsSaving));
        SaveCommand.NotifyCanExecuteChanged();

        // Revalidate
        Model.Touch();

        try
        {
            await OnSaveCommandAsync();
        }
        finally
        {
            isSaving = false;
            OnPropertyChanged(nameof(IsSaving));
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    /// <summary>
    /// Save command method.
    /// </summary>
    protected virtual async Task OnSaveCommandAsync()
    {
        if (Model.IsValid)
        {
            await OnSave();
            var needToRefresh = true;
            Close(needToRefresh);
        }
    }

    /// <summary>
    /// Close a dialog.
    /// </summary>
    /// <param name="result">Dialog result.</param>
    protected void Close(bool result)
    {
        Result = result;
        Close();
    }

    /// <summary>
    /// Close the associated view.
    /// </summary>
    protected abstract void Close();

    /// <summary>
    /// Save action.
    /// </summary>
    /// <returns></returns>
    protected abstract Task OnSave();
}