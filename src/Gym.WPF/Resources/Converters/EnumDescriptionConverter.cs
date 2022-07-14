using System;
using System.Globalization;
using System.Windows.Data;
using Saritasa.Tools.Common.Utils;

namespace Gym.WPF.Resources.Converters;

/// <summary>
/// Convert enum value to string description.
/// </summary>
internal class EnumDescriptionConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum e)
        {
            return EnumUtils.GetDescription(e);
        }

        return value;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}