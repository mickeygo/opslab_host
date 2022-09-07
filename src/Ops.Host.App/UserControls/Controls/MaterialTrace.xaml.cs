using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class MaterialTrace : UserControl
{
    public MaterialTrace()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<MaterialTraceViewModel>();
    }
}
