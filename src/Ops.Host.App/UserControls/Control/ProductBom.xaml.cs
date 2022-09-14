using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class ProductBom : UserControl
{
    public ProductBom()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<ProductBomViewModel>();
    }
}
