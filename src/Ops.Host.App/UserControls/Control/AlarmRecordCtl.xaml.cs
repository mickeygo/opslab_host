using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class AlarmRecordCtl : UserControl
{
    public AlarmRecordCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<AlarmRecordViewModel>();
    }
}
