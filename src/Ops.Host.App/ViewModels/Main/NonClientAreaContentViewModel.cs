namespace Ops.Host.App.ViewModels;

public sealed class NonClientAreaContentViewModel : ObservableObject, IViewModel
{
    public NonClientAreaContentViewModel()
    {

    }

    #region 属性绑定

    private string _appVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";
    public string AppVersion
    {
        get => _appVersion;
        set => SetProperty(ref _appVersion, value);
    }

    #endregion
}
