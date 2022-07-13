using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Gym.MVVM.ServiceAbstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.WPF.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var compositionRoot = CompositionRoot.GetInstance();
        Task.Run(async () =>
        {
            await compositionRoot.RunAsync();
            var uiContext = compositionRoot.ServiceProvider.GetRequiredService<IUiContext>();
            await uiContext.SwitchToUi();
            LoadingLayout.Visibility = Visibility.Collapsed;
        });

        DisableNavigationHotkeys();
    }

    private void FrameRoot_Navigating(object sender, NavigatingCancelEventArgs e)
    {
        var animation = new ThicknessAnimation
        {
            Duration = TimeSpan.FromSeconds(0.3),
            DecelerationRatio = 0.7,
            To = new Thickness(0, 0, 0, 0)
        };

        if (e.NavigationMode == NavigationMode.New)
        {
            animation.From = new Thickness(500, 0, 0, 0);
        }
        else if (e.NavigationMode == NavigationMode.Back)
        {
            animation.From = new Thickness(0, 0, 500, 0);
        }

        if (e.Content is NavigationPage page)
        {
            page.BeginAnimation(MarginProperty, animation);
        }
    }

    private void FrameDialog_Navigating(object sender, NavigatingCancelEventArgs e)
    {
        var animation = new ThicknessAnimation
        {
            Duration = TimeSpan.FromSeconds(0.3),
            DecelerationRatio = 0.7,
            To = new Thickness(0, 0, 0, 0)
        };

        animation.From = new Thickness(0, 300, 0, 0);

        ((BaseDialog)e.Content).BeginAnimation(MarginProperty, animation);
    }

    private void DisableNavigationHotkeys()
    {
        // TODO: integrate NavigationCommands with NavigationService.
        NavigationCommands.BrowseForward.InputGestures.Clear();
        NavigationCommands.BrowseHome.InputGestures.Clear();
        NavigationCommands.BrowseStop.InputGestures.Clear();
        NavigationCommands.Favorites.InputGestures.Clear();
        NavigationCommands.FirstPage.InputGestures.Clear();
        NavigationCommands.GoToPage.InputGestures.Clear();
        NavigationCommands.LastPage.InputGestures.Clear();
        NavigationCommands.NextPage.InputGestures.Clear();
        NavigationCommands.PreviousPage.InputGestures.Clear();
        NavigationCommands.Refresh.InputGestures.Clear();
        NavigationCommands.Search.InputGestures.Clear();
        NavigationCommands.Zoom.InputGestures.Clear();
    }
}
