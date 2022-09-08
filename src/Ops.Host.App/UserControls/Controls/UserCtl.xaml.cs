using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class UserCtl : UserControl
{
    public UserCtl()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<UserViewModel>();
    }
}
