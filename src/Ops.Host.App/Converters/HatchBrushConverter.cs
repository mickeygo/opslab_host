using HandyControl.Data;
using HandyControl.Tools;

namespace Ops.Host.App.Converters;

public sealed class HatchBrushConverter : IValueConverter
{
    private readonly HatchBrushGenerator _brushGenerator;

    public HatchBrushConverter()
    {
        _brushGenerator = new HatchBrushGenerator();
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is HatchStyle style)
        {
            return _brushGenerator.GetHatchBrush(style, ResourceHelper.GetResource<Color>(ResourceToken.DarkPrimaryColor), Colors.Transparent);
        }
        return Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
