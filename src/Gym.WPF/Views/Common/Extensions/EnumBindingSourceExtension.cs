using System;
using System.Windows.Markup;

namespace Gym.WPF.Views.Common.Extensions;

/// <summary>
/// XAML markup extension for binding to enums.
/// </summary>
internal class EnumBindingSourceExtension : MarkupExtension
{
    /// <summary>
    /// Enum type.
    /// </summary>
    public Type EnumType { get; private set; }

    /// <summary>
    /// Initializes a new instance of the enum binding source extension.
    /// </summary>
    /// <param name="enumType">Enum type.</param>
    public EnumBindingSourceExtension(Type enumType)
    {
        if (enumType == null || !enumType.IsEnum)
        {
            throw new ArgumentException("Enum type must not be null and of type Enum", nameof(enumType));
        }
        EnumType = enumType;
    }

    /// <inheritdoc/>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Enum.GetValues(EnumType);
    }
}