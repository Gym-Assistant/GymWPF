using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Gym.MVVM.ViewModels.Common.Models;

namespace Gym.WPF.Views.Common;

/// <summary>
/// Interaction logic for InputControl.xaml.
/// </summary>
public partial class InputControl : UserControl
{
    /// <summary>
    /// Title content.
    /// </summary>
    public object TitleContent
    {
        get { return GetValue(TitleContentProperty); }
        set { SetValue(TitleContentProperty, value); }
    }

    /// <see cref="TitleContent"/>
    public static readonly DependencyProperty TitleContentProperty =
        DependencyProperty.Register(nameof(TitleContent), typeof(object), typeof(InputControl), new PropertyMetadata(null));

    /// <summary>
    /// Field title.
    /// </summary>
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    /// <see cref="Title"/>
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(InputControl), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Tool tip icon text.
    /// </summary>
    public string ToolTipIconText
    {
        get { return (string)GetValue(ToolTipIconTextProperty); }
        set { SetValue(ToolTipIconTextProperty, value); }
    }

    /// <see cref="Title"/>
    public static readonly DependencyProperty ToolTipIconTextProperty =
        DependencyProperty.Register(nameof(ToolTipIconText), typeof(string), typeof(InputControl), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Field value.
    /// </summary>
    public string Value
    {
        get { return (string)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    /// <see cref="Value"/>
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(string), typeof(InputControl), new FrameworkPropertyMetadata
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        });

    /// <summary>
    /// Placeholder.
    /// </summary>
    public string Placeholder
    {
        get { return (string)GetValue(PlaceholderProperty); }
        set { SetValue(PlaceholderProperty, value); }
    }

    /// <see cref="Placeholder"/>
    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register("Placeholder", typeof(string), typeof(InputControl), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Is the component multiline.
    /// </summary>
    public bool IsMultiline
    {
        get { return (bool)GetValue(IsMultilineProperty); }
        set { SetValue(IsMultilineProperty, value); }
    }

    /// <see cref="IsMultiline"/>
    public static readonly DependencyProperty IsMultilineProperty =
        DependencyProperty.Register("IsMultiline", typeof(bool), typeof(InputControl), new PropertyMetadata(false));

    /// <summary>
    /// Padding for input control.
    /// </summary>
    public Thickness InputPadding => IsMultiline
        ? (Thickness)Application.Current.Resources["MultilineInputPadding"]
        : (Thickness)Application.Current.Resources["InputPadding"];

    /// <summary>
    /// Vertical alignment of the text inside the input.
    /// </summary>
    public VerticalAlignment VerticalTextAlignment => IsMultiline
        ? VerticalAlignment.Top
        : VerticalAlignment.Center;

    /// <summary>
    /// Height property of the input.
    /// </summary>
    public double InputHeight => IsMultiline
        ? double.NaN
        : (double)Application.Current.Resources["InputHeight"];

    /// <summary>
    /// Text selection.
    /// </summary>
    public TextSelection TextSelection
    {
        get { return (TextSelection)GetValue(TextSelectionProperty); }
        set { SetValue(TextSelectionProperty, value); }
    }

    /// <inheritdoc cref="TextSelection"/>
    public static readonly DependencyProperty TextSelectionProperty =
        DependencyProperty.Register("TextSelection", typeof(TextSelection), typeof(InputControl), new FrameworkPropertyMetadata
        {
            BindsTwoWayByDefault = true,
        });

    /// <summary>
    /// Multiline min height.
    /// </summary>
    public double MultilineMinHeight
    {
        get { return (double)GetValue(MultilineMinHeightProperty); }
        set { SetValue(MultilineMinHeightProperty, value); }
    }

    /// <inheritdoc cref="MultilineMinHeight"/>
    public static readonly DependencyProperty MultilineMinHeightProperty =
        DependencyProperty.Register(nameof(MultilineMinHeight), typeof(double), typeof(InputControl), new PropertyMetadata(72d));

    /// <summary>
    /// Multiline max height.
    /// </summary>
    public double MultilineMaxHeight
    {
        get { return (double)GetValue(MultilineMaxHeightProperty); }
        set { SetValue(MultilineMaxHeightProperty, value); }
    }

    /// <inheritdoc cref="MultilineMaxHeight"/>
    public static readonly DependencyProperty MultilineMaxHeightProperty =
        DependencyProperty.Register(nameof(MultilineMaxHeight), typeof(double), typeof(InputControl), new PropertyMetadata(150d));

    /// <summary>
    /// Constructor.
    /// </summary>
    public InputControl()
    {
        InitializeComponent();
        TextBox.SelectionChanged += TextBox_SelectionChanged;
        Loaded += OnLoaded;

        Focusable = true;
        GotFocus += (o, e) => TextBox.Focus();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var valueTextBinding = BindingOperations.GetBinding(this, ValueProperty);
        if (valueTextBinding != null)
        {
            TextBox.SetBinding(TextBox.TextProperty, valueTextBinding);
        }
    }

    private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        TextSelection = new TextSelection(textBox.SelectionStart, textBox.SelectionLength);
    }
}
