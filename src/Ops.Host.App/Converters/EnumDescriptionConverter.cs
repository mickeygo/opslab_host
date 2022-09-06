namespace Ops.Host.App.Converters;

/// <summary>
/// 显示枚举字段的 <see cref="DescriptionAttribute"/> 描述。
/// </summary>
public sealed class EnumDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // 枚举
        if (value is Enum @enum)
        {
            return @enum.Desc() ?? value;
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
