using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Gym.WPF.Resources.Controls
{
    /// <summary>
    /// Interaction logic for DropDownControl.xaml.
    /// </summary>
    public partial class DropDownControl : UserControl
    {
        /// <summary>
        /// Selected item changed command.
        /// </summary>
        public ICommand SelectedItemChangedCommand
        {
            get { return (ICommand)GetValue(SelectedItemChangedCommandProperty); }
            set { SetValue(SelectedItemChangedCommandProperty, value); }
        }

        /// <see cref="SelectedItemChangedCommand"/>
        public static readonly DependencyProperty SelectedItemChangedCommandProperty =
            DependencyProperty.Register(nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(DropDownControl), new PropertyMetadata(default));

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
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(DropDownControl), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Selected value.
        /// </summary>
        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        /// <see cref="SelectedValue"/>
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register(nameof(SelectedValue), typeof(object), typeof(DropDownControl), new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            });

        /// <summary>
        /// Selected item.
        /// </summary>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <see cref="SelectedItem"/>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(DropDownControl), new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            });

        /// <summary>
        /// Items source.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <see cref="ItemsSource"/>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(DropDownControl), new PropertyMetadata(null));

        /// <summary>
        /// Display member path.
        /// </summary>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        /// <see cref="DisplayMemberPath"/>
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(DropDownControl), new PropertyMetadata(string.Empty, OnDisplayMemberPathChanged));

        private static void OnDisplayMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (string)e.NewValue;
            var combobox = (d as DropDownControl).ComboBox;
            if (!string.IsNullOrEmpty(newValue))
            {
                // HACK: We cant use together DisplayMemberPath and ItemTemplate.
                combobox.ItemTemplate = null;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DropDownControl()
        {
            InitializeComponent();
            Loaded += DropDownControlLoaded;
            Unloaded += DropDownControlUnloaded;
        }

        private void DropDownControlUnloaded(object sender, RoutedEventArgs e)
        {
            BindingOperations.ClearBinding(this, SelectedValueProperty);
            BindingOperations.ClearBinding(this, SelectedItemProperty);
        }

        private void DropDownControlLoaded(object sender, RoutedEventArgs e)
        {
            // This is a hack to fix validation in the control. It binds the properties directly to view model.
            var selectedValueBinding = BindingOperations.GetBinding(this, SelectedValueProperty);
            if (selectedValueBinding != null)
            {
                ComboBox.SetBinding(ComboBox.SelectedValueProperty, selectedValueBinding);
            }
            var selectedItemBinding = BindingOperations.GetBinding(this, SelectedItemProperty);
            if (selectedItemBinding != null)
            {
                ComboBox.SetBinding(ComboBox.SelectedItemProperty, selectedItemBinding);
            }
        }
    }
}
