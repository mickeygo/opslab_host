using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class Archive : UserControl
{
    public Archive()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<ArchiveViewModel>();
    }
}
