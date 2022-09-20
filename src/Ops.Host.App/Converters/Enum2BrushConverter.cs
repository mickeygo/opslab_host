using HandyControl.Tools;

namespace Ops.Host.App.Converters;

/// <summary>
/// 将枚举类型标识的特性 ResourceBrushAttribute 转换成画刷。
/// </summary>
public sealed class Enum2BrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum @enum)
        {
            var attr = @enum.GetAttr<ResourceBrushAttribute>();
            if (attr != null)
            {
                return ResourceHelper.GetResource<Brush>(attr.BrushName); 
            }
        }

        return new();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
