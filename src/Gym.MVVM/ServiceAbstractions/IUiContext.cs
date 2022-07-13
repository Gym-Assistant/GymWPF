using Gym.MVVM.Utils;

namespace Gym.MVVM.ServiceAbstractions;

/// <summary>
/// Service for working with UI context.
/// </summary>
public interface IUiContext
{
    /// <summary>
    /// Switch to the UI thread.
    /// </summary>
    IAwaitable SwitchToUi();

    /// <summary>
    /// Synchronization context of the UI thread.
    /// </summary>
    SynchronizationContext UiSynchronizationContext { get; }
}