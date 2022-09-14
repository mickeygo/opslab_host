using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class ProductBomCtl : UserControl
{
    public ProductBomCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<ProductBomViewModel>();
    }
}
