using HandyControl.Tools;

namespace Ops.Host.App.Converters;

public sealed class String2BrushConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string str ? ResourceHelper.GetResource<Brush>(str) : default;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
