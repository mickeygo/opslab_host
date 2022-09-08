using Ops.Host.App.ViewModels;

namespace Ops.Host.App.UserControls;

public partial class NonClientAreaContent
{
    public NonClientAreaContent()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<NonClientAreaContentViewModel>();
    }

    private void ButtonConfig_OnClick(object sender, RoutedEventArgs e) => PopupConfig.IsOpen = true;
}
