using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class ArchiveCtl : UserControl
{
    public ArchiveCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<ArchiveViewModel>();
    }
}
