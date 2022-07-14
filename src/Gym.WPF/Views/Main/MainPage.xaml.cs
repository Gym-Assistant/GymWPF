using System.Windows.Controls;
using Gym.MVVM.ViewModels.Main;
using Gym.WPF.Infrastructure.Navigation;

namespace Gym.WPF.Views.Main;

/// <summary>
/// Interaction logic for Login.xaml.
/// </summary>
[UsesViewModel(typeof(MainPageViewModel))]
public partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }
}

