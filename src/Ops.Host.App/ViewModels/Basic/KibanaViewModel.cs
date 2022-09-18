namespace Ops.Host.App.ViewModels;

public sealed class KibanaViewModel : ObservableObject, IViewModel, IDisposable
{
    private readonly CancellationTokenSource _cts = new();

    private readonly DeviceInfoManager _deviceInfoManager;
    private readonly DeviceHealthManager _deviceHealthManager;

    /// <summary>
    /// 定时器处理对象
    /// </summary>
    public EventHandler? TimerHandler { get; set; }

    public KibanaViewModel(
        DeviceInfoManager deviceInfoManager,
        DeviceHealthManager deviceHealthManager)
    {
        _deviceInfoManager = deviceInfoManager;
        _deviceHealthManager = deviceHealthManager;

        Init();
    }

    void Init()
    {
        var deviceInfos = _deviceInfoManager.GetAll();
        foreach (var deviceInfo in deviceInfos)
        {
            DeviceSourceList.Add(new KibanaModel
            {
                Line = deviceInfo.Schema.Line,
                Station = deviceInfo.Schema.Station,
                ConnectedState = false,
            });
        }

        // 状态检测，定时器可考虑与 DispatcherTimer 有什么差异
        _deviceHealthManager.Check();
        _ = Task.Factory.StartNew(async () =>
        {
            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(2000);
                ChangeDeviceConnState();
            }
                
        }, default, TaskCreationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());

        // 测试数据
        _ = Task.Factory.StartNew(async () =>
        {
            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(new Random().Next(2, 10) * 1000);

                if (AlarmSourceList.Count >= 32)
                {
                    AlarmSourceList.RemoveAt(AlarmSourceList.Count - 1);
                }

                AlarmSourceList.Insert(0, new()
                {
                    Station = "OP10",
                    Name = "焊接设备电压过高",
                });
            }

        }, default, TaskCreationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());

        _ = Task.Factory.StartNew(async () =>
        {
            int n = 1;
            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(new Random().Next(3, 15) * 1000);

                if (ProductionSourceList.Count >= 32)
                {
                    ProductionSourceList.RemoveAt(AlarmSourceList.Count - 1);
                }

                ProductionSourceList.Insert(0, new()
                {
                    Station = "OP10",
                    SN = $"SN{DateTime.Now:yyyyMMddHHmmss}",
                    Shift = "早班",
                    Pass = n++ % 4 != 0,
                    InboundTime = DateTime.Now,
                    OutboundTime = DateTime.Now,
                });
            }

        }, default, TaskCreationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());
    }

    #region 绑定属性

    private ObservableCollection<KibanaModel> _deviceSourceList = new();

    public ObservableCollection<KibanaModel> DeviceSourceList
    {
        get => _deviceSourceList;
        set => SetProperty(ref _deviceSourceList, value);
    }

    // 生产（测试）
    private ObservableCollection<ProductionModel> _productionSourceList = new();

    public ObservableCollection<ProductionModel> ProductionSourceList
    {
        get => _productionSourceList;
        set => SetProperty(ref _productionSourceList, value);
    }

    // 警报（测试）
    private ObservableCollection<AlarmModel> _alarmSourceList = new();

    public ObservableCollection<AlarmModel> AlarmSourceList
    {
        get => _alarmSourceList;
        set => SetProperty(ref _alarmSourceList, value);
    }

    #endregion

    #region privates

    private void ChangeDeviceConnState()
    {
        foreach (var device in DeviceSourceList)
        {
            var state = _deviceHealthManager.CanConnect(device.Line, device.Station);
            if (device.ConnectedState != state)
            {
                device.ConnectedState = state;
            }
        }
    }

    #endregion

    public void Dispose()
    {
        _cts.Cancel();
    }
}

public class ProductionModel : ObservableObject
{
    public string? Station { get; set; }

    public string? SN { get; set; }

    public bool Pass { get; set; }

    public string? Shift { get; set; }

    public DateTime InboundTime { get; set; }

    public DateTime? OutboundTime { get; set; }
}

public class AlarmModel
{
    public string? Station { get; set; }

    public string? Name { get; set; }

    public DateTime CreateTime { get; set; } = DateTime.Now;
}
