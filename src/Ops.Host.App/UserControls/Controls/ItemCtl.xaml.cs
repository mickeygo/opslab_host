using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class ItemCtl : UserControl
{
    public ItemCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<ItemViewModel>();
    }
}
