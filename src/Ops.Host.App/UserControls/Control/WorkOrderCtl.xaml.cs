using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class WorkOrderCtl : UserControl
{
    public WorkOrderCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<WorkOrderViewModel>();
    }
}
