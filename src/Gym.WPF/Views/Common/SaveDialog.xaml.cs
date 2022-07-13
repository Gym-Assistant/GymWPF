using Gym.MVVM.ViewModels.Common;
using Gym.WPF.Infrastructure.Navigation;

namespace Gym.WPF.Views.Common;

/// <summary>
/// Save dialog view.
/// </summary>
[UsesViewModel(typeof(SaveViewModel))]
public partial class SaveDialog : BaseDialog
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public SaveDialog()
    {
        InitializeComponent();
    }
}