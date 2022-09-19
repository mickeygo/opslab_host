using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class WoScheduleCtl : UserControl
{
    public WoScheduleCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<WoScheduleViewModel>();
    }
}
