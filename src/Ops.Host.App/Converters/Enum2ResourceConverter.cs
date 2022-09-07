namespace Ops.Host.App.Converters;

/// <summary>
/// 根据枚举类型的值（数字）转换为对应的资源。
/// </summary>
/// <remarks>枚举值（int）与分隔参数对应，参数起始地址为 0</remarks>
public sealed class Enum2ResourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        FrameworkElement element = new();

        if (value is Enum @enum)
        {
            int v = (int)value;
            if (parameter is string text)
            {
                string resourceKey = "";
                string[] array = text.Split(';');
                if (array.Length > 0 && v < array.Length)
                {
                    resourceKey = array[v];
                }

                return element.TryFindResource(resourceKey);
            }

            return new();
        }

        return new();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
