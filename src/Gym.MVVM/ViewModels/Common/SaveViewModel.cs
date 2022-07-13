using Gym.MVVM.ServiceAbstractions.Navigation;
using Microsoft.Toolkit.Mvvm.Input;

namespace Gym.MVVM.ViewModels.Common;

/// <summary>
/// A view model for a saving dialog.
/// </summary>
public class SaveViewModel : BaseViewModel, IWithResult<SaveDialogResult>
{
    private readonly IDialogService dialogService;

    /// <summary>
    /// Is the data model valid.
    /// </summary>
    public bool IsModelValid { get; }

    /// <summary>
    /// Save command.
    /// </summary>
    public RelayCommand SaveCommand { get; }

    /// <summary>
    /// Don't save command.
    /// </summary>
    public RelayCommand DontSaveCommand { get; }

    /// <summary>
    /// Cancel command.
    /// </summary>
    public RelayCommand CancelCommand { get; }

    /// <inheritdoc />
    public SaveDialogResult Result { get; private set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public SaveViewModel(bool isModelvalid, IDialogService dialogService)
    {
        IsModelValid = isModelvalid;
        this.dialogService = dialogService;
        SaveCommand = new RelayCommand(() => CloseWithResult(SaveDialogResult.Save));
        DontSaveCommand = new RelayCommand(() => CloseWithResult(SaveDialogResult.Discard));
        CancelCommand = new RelayCommand(() => CloseWithResult(SaveDialogResult.Cancel));
    }

    private void CloseWithResult(SaveDialogResult result)
    {
        Result = result;
        dialogService.Close();
    }
}

/// <summary>
/// Saving dialog result.
/// </summary>
public enum SaveDialogResult
{
    /// <summary>
    /// Save and close.
    /// </summary>
    Save,

    /// <summary>
    /// Don't save but close.
    /// </summary>
    Discard,

    /// <summary>
    /// Close without saving.
    /// </summary>
    Cancel,
}