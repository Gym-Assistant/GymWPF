using System.Threading.Tasks;
using System.Windows.Controls;
using Gym.MVVM.ViewModels;

namespace Gym.WPF.Infrastructure.Navigation;

/// <summary>
/// Contains information about a view.
/// </summary>
public class ViewState
{
    private readonly Frame frame;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="frame">Frame containing the view.</param>
    public ViewState(Frame frame)
    {
        this.frame = frame;
        IsVisible = true;
    }

    /// <summary>
    /// Associated view model.
    /// </summary>
    public BaseViewModel ViewModel { get; init; }

    /// <summary>
    /// View model navigation result.
    /// </summary>
    public TaskCompletionSource NavigationResult { get; init; }

    /// <summary>
    /// Indicates if the view is visible.
    /// </summary>
    public bool IsVisible { get; private set; }

    /// <summary>
    /// Change visibility of the view.
    /// </summary>
    /// <param name="isVisible">Whether the view should be visible or not.</param>
    /// <param name="updateFrameVisibility">Indicates if associated frame should also change its visibility accordingly.</param>
    public void ToggleVisibility(bool isVisible, bool updateFrameVisibility = true)
    {
        IsVisible = isVisible;
        if (updateFrameVisibility)
        {
            EnsureFrameVisibility();
        }
    }

    /// <summary>
    /// Make sure associated frame's visibility matches the current view's visibility (<see cref="IsVisible"/>).
    /// </summary>
    public void EnsureFrameVisibility()
    {
        frame.Visibility = IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
    }
}