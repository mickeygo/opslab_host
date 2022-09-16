using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class ProcessParamCtl : UserControl
{
    public ProcessParamCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<ProcessParamViewModel>();
    }
}
