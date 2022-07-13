using System.Windows.Controls;
using Gym.MVVM.ViewModels;

namespace Gym.WPF.Views;

/// <summary>
/// Base class for dialogs.
/// </summary>
public abstract class BaseDialog : Page
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public BaseDialog()
    {
        Loaded += BaseDialog_Loaded;
    }

    private async void BaseDialog_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        var vm = DataContext as BaseViewModel;
        if (vm == null)
        {
            return;
        }

        try
        {
            vm.IsBusy = true;
            await vm.LoadAsync();
        }
        finally
        {
            vm.IsBusy = false;
        }

        Loaded -= BaseDialog_Loaded;
    }
}