using System;
using System.Threading.Tasks;
using System.Windows;
using Gym.MVVM.ServiceAbstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.Domain.Exceptions;

namespace Gym.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    #region Error Handling

    private void Current_DispatcherUnhandledException(object sender,
        System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        if (e.Exception is DomainException domainException)
        {
            HandleDomainException(domainException);
            e.Handled = true;
            return;
        }

        if (e.Exception is TaskCanceledException || e.Exception is OperationCanceledException)
        {
            e.Handled = true;
            return;
        }

        LogException(e.Exception);
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = e.ExceptionObject as Exception;
        if (e.ExceptionObject is DomainException domainException)
        {
            HandleDomainException(domainException);
            return;
        }

        if (e.ExceptionObject is TaskCanceledException || e.ExceptionObject is OperationCanceledException)
        {
            return;
        }

        LogException(exception);
        FreeResources();
    }

    private void HandleDomainException(DomainException exception)
    {
        var compositionRoot = CompositionRoot.GetInstance();
        var notificationService = compositionRoot.ServiceProvider.GetRequiredService<INotificationService>();
        notificationService.ShowMessage(exception.Message, MessageType.Error);
    }

    private void LogException(Exception exception)
    {
        var compositionRoot = CompositionRoot.GetInstance();
        var logger = compositionRoot.ServiceProvider.GetRequiredService<ILogger<App>>();
        logger.LogCritical(exception, "Unexpected error occurred.");
        MessageBox.Show("Unexpected error occurred.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    #endregion

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        FreeResources();
    }

    private void FreeResources()
    {
        var compositionRoot = CompositionRoot.GetInstance();
        try
        {
            compositionRoot.Dispose();
        }
        catch (Exception ex)
        {
            var logger = compositionRoot.ServiceProvider.GetRequiredService<ILogger<App>>();
            logger.LogCritical(ex, "Error disposing the service provider");
        }
    }
}
