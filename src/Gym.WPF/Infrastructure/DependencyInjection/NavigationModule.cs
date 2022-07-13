using System.Windows;
using Gym.MVVM.ServiceAbstractions;
using Gym.MVVM.ServiceAbstractions.Navigation;
using Gym.MVVM.Utils;
using Gym.WPF.Infrastructure.Navigation;
using Gym.WPF.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.WPF.Infrastructure.DependencyInjection;

/// <summary>
/// Register navigation dependencies.
/// </summary>
internal static class NavigationModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddTransient<ViewModelFactory>();
        services.AddSingleton<NavigationStack>();
        services.AddTransient<ICurrentViewService, CurrentViewService>();

        services.AddSingleton<IDialogService>(provider => new DialogService(
            ((MainWindow)Application.Current.MainWindow).FrameDialog,
            provider.GetRequiredService<ViewModelFactory>(),
            provider.GetRequiredService<NavigationStack>(),
            provider.GetRequiredService<ICurrentViewService>()));

        services.AddSingleton<INavigationService>(provider =>
        {
            // We should initialize `IDialogService` before.
            // Navigation page should be top most on the application start.
            provider.GetRequiredService<IDialogService>();

            return new NavigationService(
                ((MainWindow)Application.Current.MainWindow).FrameRoot,
                provider.GetRequiredService<ViewModelFactory>(),
                provider.GetRequiredService<NavigationStack>());
        });
    }
}