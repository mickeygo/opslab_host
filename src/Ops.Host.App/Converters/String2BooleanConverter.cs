namespace Ops.Host.App.Converters;

/// <summary>
/// string 类型转换为 bool 类型
/// </summary>
public sealed class String2BooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null || parameter is null)
        {
            return false;
        }

        return value.ToString() == parameter.ToString();
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null || parameter is null)
        {
            return false;
        }

        bool v = (bool)value;
        return v ? parameter.ToString() : null;
    }
}
