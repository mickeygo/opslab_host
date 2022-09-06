namespace Ops.Host.App.Converters;

/// <summary>
/// 将枚举转换为下拉框，若枚举可空，下拉框也可空。
/// </summary>
public sealed class EnumDropdownListConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum)
        {
            var type = value.GetType();
            var canEmpty = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            return EnumExtensions.ToDropdownList(type, canEmpty);
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
