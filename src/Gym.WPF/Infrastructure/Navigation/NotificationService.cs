using System;
using System.Windows;
using Gym.MVVM.ServiceAbstractions;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Lifetime.Clear;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Gym.WPF.Infrastructure.Navigation;

/// <summary>
/// In-app notification service (Popup).
/// </summary>
internal class NotificationService : INotificationService, IDisposable
{
    private readonly Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.MainWindow,
            corner: Corner.BottomRight,
            offsetX: 10,
            offsetY: 10);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(5));

        cfg.Dispatcher = Application.Current.Dispatcher;
    });

    /// <inheritdoc/>
    public void CloseAll()
    {
        notifier.ClearMessages(new ClearAll());
    }

    /// <inheritdoc/>
    public void ShowMessage(string message, MessageType type = MessageType.Information)
    {
        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
        {
            switch (type)
            {
                case MessageType.Information:
                    notifier.ShowInformation(message);
                    break;
                case MessageType.Success:
                    notifier.ShowSuccess(message);
                    break;
                case MessageType.Warning:
                    notifier.ShowWarning(message);
                    break;
                case MessageType.Error:
                    notifier.ShowError(message);
                    break;
            }
        }));
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        notifier.Dispose();
    }
}