using Gym.MVVM.ViewModels.Common;

namespace Gym.MVVM.ServiceAbstractions.Navigation;

/// <summary>
/// Contains presets for different dialogs.
/// </summary>
public static class DialogStylePresets
{
    /// <summary>
    /// Get a dialog which just displays a text to user in a "danger" style.
    /// </summary>
    /// <param name="dialogService">Dialog service reference.</param>
    /// <param name="errorText">Message to be shown to user.</param>
    /// <param name="title">Dialog title.</param>
    /// <returns>Resulting dialog builder instance.</returns>
    public static DialogBuilder GetErrorDialog(this IDialogService dialogService, string errorText, string title = "Error")
    {
        return dialogService.GetDialogBuilder(title)
            .AsAlert()
            .WithMessage(errorText)
            .WithCancelButton(new ConfirmationButtonStyle
            {
                IsVisible = false
            });
    }
}