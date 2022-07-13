using Gym.MVVM.ViewModels.Common;

namespace Gym.MVVM.ServiceAbstractions.Navigation;

/// <summary>
/// Generic dialog builder.
/// </summary>
public sealed class DialogBuilder : IDisposable
{
    private readonly ConfirmationViewModel viewModel;
    private readonly IDialogService dialogService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    /// <param name="dialogService">Dialog service.</param>
    public DialogBuilder(string title, IDialogService dialogService)
    {
        this.dialogService = dialogService;
        viewModel = new ConfirmationViewModel(title, dialogService);
    }

    /// <summary>
    /// Body message.
    /// </summary>
    public DialogBuilder WithMessage(string message)
    {
        viewModel.Message = message;
        return this;
    }

    /// <summary>
    /// "Ok" button text.
    /// </summary>
    public DialogBuilder WithOkButton(string buttonText)
    {
        viewModel.OkButton.Text = buttonText;
        return this;
    }

    /// <summary>
    /// "Cancel" button text.
    /// </summary>
    public DialogBuilder WithCancelButton(string buttonText)
    {
        viewModel.CancelButton.Text = buttonText;
        return this;
    }

    /// <summary>
    /// Specify styles for Ok button.
    /// </summary>
    /// <param name="okButtonStyle">Style information.</param>
    public DialogBuilder WithOkButton(ConfirmationButtonStyle okButtonStyle)
    {
        viewModel.OkButton = okButtonStyle;
        return this;
    }

    /// <summary>
    /// Specify styles for Cancel button.
    /// </summary>
    /// <param name="cancelButtonStyle">Style information.</param>
    public DialogBuilder WithCancelButton(ConfirmationButtonStyle cancelButtonStyle)
    {
        viewModel.CancelButton = cancelButtonStyle;
        return this;
    }

    /// <summary>
    /// As destructive action (e.g. removing).
    /// </summary>
    public DialogBuilder AsAlert()
    {
        viewModel.IsAlert = true;
        return this;
    }

    /// <summary>
    /// Show a dialog.
    /// </summary>
    public async Task<bool> Show()
    {
        return await dialogService.OpenAsync<ConfirmationViewModel, bool>(viewModel);
    }

    /// <inheritdoc />
    public void Dispose() => ((IDisposable)viewModel).Dispose();
}