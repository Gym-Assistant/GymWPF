using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Gym.WPF.Infrastructure.Behaviors;

/// <summary>
/// When associated label control is clicked, the <see cref="Label.Target"/> control automatically receives focus.
/// </summary>
public class LabelFocusBehavior : Behavior<Label>
{
    /// <inheritdoc />
    protected override void OnAttached()
    {
        AssociatedObject.MouseLeftButtonDown += (s, e) =>
        {
            e.Handled = true;
            AssociatedObject.CaptureMouse();
        };
        AssociatedObject.MouseLeftButtonUp += (s, e) =>
        {
            if (!AssociatedObject.IsMouseCaptured)
            {
                return;
            }

            e.Handled = true;
            AssociatedObject.ReleaseMouseCapture();

            if (AssociatedObject.Target != null && AssociatedObject.Target.Focusable)
            {
                AssociatedObject.Target.Focus();
            }
        };
    }
}