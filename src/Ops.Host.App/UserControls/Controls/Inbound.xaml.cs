using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class Inbound : UserControl
{
    public Inbound()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<InboundViewModel>();
    }
}
