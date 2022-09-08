using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class CardCtl : UserControl
{
    public CardCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<CardViewModel>();
    }
}
