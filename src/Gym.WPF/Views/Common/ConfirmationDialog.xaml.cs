using Gym.MVVM.ViewModels.Common;
using Gym.WPF.Infrastructure.Navigation;

namespace Gym.WPF.Views.Common;

/// <summary>
/// Interaction logic for ConfirmationDialog.xaml.
/// </summary>
[UsesViewModel(typeof(ConfirmationViewModel))]
public partial class ConfirmationDialog : BaseDialog
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public ConfirmationDialog()
    {
        InitializeComponent();
    }
}