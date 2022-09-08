using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class MaterialTraceCtl : UserControl
{
    public MaterialTraceCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<MaterialTraceViewModel>();
    }
}
