using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class StationCtl : UserControl
{
    public StationCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<StationViewModel>();
    }
}
