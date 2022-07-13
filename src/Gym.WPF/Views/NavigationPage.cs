using System.Windows;
using System.Windows.Controls;
using Gym.MVVM.ViewModels;

namespace Gym.WPF.Views;

/// <summary>
/// Base class for navigation page.
/// </summary>
public class NavigationPage : Page
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public NavigationPage()
    {
        Loaded += NavigationPage_Loaded;
    }

    private async void NavigationPage_Loaded(object sender, RoutedEventArgs e)
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

        Loaded -= NavigationPage_Loaded;
    }
}