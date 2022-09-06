using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class DictData : UserControl
{
    public DictData()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<DictDataViewModel>();
    }
}
