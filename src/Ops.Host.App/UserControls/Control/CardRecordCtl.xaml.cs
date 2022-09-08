using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class CardRecordCtl : UserControl
{
    public CardRecordCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<CardRecordViewModel>();
    }
}
