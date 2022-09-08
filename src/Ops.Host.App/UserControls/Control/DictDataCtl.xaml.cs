using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class DictDataCtl : UserControl
{
    public DictDataCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<DictDataViewModel>();
    }
}
