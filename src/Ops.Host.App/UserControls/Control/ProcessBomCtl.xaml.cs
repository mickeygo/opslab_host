using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class ProcessBomCtl : UserControl
{
    public ProcessBomCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<ProcessBomViewModel>();
    }
}
