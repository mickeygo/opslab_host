using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class KibanaCtl : UserControl
{
    public KibanaCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<KibanaViewModel>();
    }
}
