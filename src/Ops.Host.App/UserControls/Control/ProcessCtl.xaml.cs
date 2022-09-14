using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class ProcessCtl : UserControl
{
    public ProcessCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<ProcessViewModel>();
    }
}
