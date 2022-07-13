using Gym.MVVM.ServiceAbstractions.Navigation;
using Microsoft.Toolkit.Mvvm.Input;

namespace Gym.MVVM.ViewModels.Common;

/// <summary>
/// Alert dialog view model.
/// </summary>
public class ConfirmationViewModel : BaseViewModel, IWithResult<bool>
{
    private readonly IDialogService dialogService;

    /// <summary>
    /// Dialog title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Message text.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Styles for Ok button.
    /// </summary>
    public ConfirmationButtonStyle OkButton { get; set; } = new ConfirmationButtonStyle
    {
        Text = "Ok"
    };

    /// <summary>
    /// Styles for Cancel button.
    /// </summary>
    public ConfirmationButtonStyle CancelButton { get; set; } = new ConfirmationButtonStyle
    {
        Text = "Cancel"
    };

    /// <summary>
    /// OK command.
    /// </summary>
    public RelayCommand OkCommand { get; }

    /// <summary>
    /// Cancell command.
    /// </summary>
    public RelayCommand CancelCommand { get; }

    /// <summary>
    /// Is destructive action (e.g. removing).
    /// </summary>
    public bool IsAlert { get; set; }

    /// <inheritdoc />
    public bool Result { get; private set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public ConfirmationViewModel(string title, IDialogService dialogService)
    {
        Title = title;
        this.dialogService = dialogService;
        OkCommand = new RelayCommand(() => CloseWithResult(true));
        CancelCommand = new RelayCommand(() => CloseWithResult(false));
    }

    private void CloseWithResult(bool result)
    {
        Result = result;
        dialogService.Close();
    }
}

/// <summary>
/// Contains style information of a button.
/// </summary>
public class ConfirmationButtonStyle
{
    /// <summary>
    /// Button text.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Whether or not the button should be visible.
    /// </summary>
    public bool IsVisible { get; set; } = true;
}