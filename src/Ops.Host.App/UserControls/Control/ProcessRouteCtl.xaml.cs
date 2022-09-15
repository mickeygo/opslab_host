using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class ProcessRouteCtl : UserControl
{
    public ProcessRouteCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<ProcessRouteViewModel>();
    }
}
