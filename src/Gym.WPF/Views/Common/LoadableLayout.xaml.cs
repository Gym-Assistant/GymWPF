using System.Windows;
using System.Windows.Controls;

namespace Gym.WPF.Views.Common;

/// <summary>
/// A layout for presenting a loadable content.
/// </summary>
public class LoadableLayout : ContentControl
{
    /// <summary>
    /// Is data loading.
    /// </summary>
    public bool IsBusy
    {
        get { return (bool)GetValue(IsBusyProperty); }
        set { SetValue(IsBusyProperty, value); }
    }

    /// <inheritdoc cref="IsBusy"/>
    public static readonly DependencyProperty IsBusyProperty =
        DependencyProperty.Register("IsBusy", typeof(bool), typeof(LoadableLayout), new PropertyMetadata(false));
}