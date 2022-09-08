using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class InboundCtl : UserControl
{
    public InboundCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<InboundViewModel>();
    }
}
