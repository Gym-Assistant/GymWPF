namespace Gym.MVVM.ServiceAbstractions;

/// <summary>
/// In-app notification service abstraction.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Show message.
    /// </summary>
    /// <param name="message">Message text.</param>
    /// <param name="type">Message type.</param>
    void ShowMessage(string message, MessageType type = MessageType.Information);

    /// <summary>
    /// Force clearing messages stack (Close all messages).
    /// </summary>
    void CloseAll();
}

/// <summary>
/// Message types.
/// </summary>
public enum MessageType
{
    /// <summary>
    /// Information (Blue).
    /// </summary>
    Information,

    /// <summary>
    /// Success (Green).
    /// </summary>
    Success,

    /// <summary>
    /// Warning (Yellow).
    /// </summary>
    Warning,

    /// <summary>
    /// Error (Red).
    /// </summary>
    Error
}