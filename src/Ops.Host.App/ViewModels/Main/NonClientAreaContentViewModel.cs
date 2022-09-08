﻿using HandyControl.Controls;
using HandyControl.Data;

namespace Ops.Host.App.ViewModels;

public sealed class NonClientAreaContentViewModel : ObservableObject, IViewModel
{
    private readonly StationCacheManager _stationManager;

    public NonClientAreaContentViewModel(StationCacheManager stationManager)
    {
        _stationManager = stationManager;

        RefreshStationCacheCommand = new AsyncRelayCommand(RefreshStationCacheAsync);
    }

    /// <summary>
    /// 刷新工站缓存信息。
    /// </summary>
    public ICommand RefreshStationCacheCommand { get; }

    #region 属性绑定

    private string _appVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";
    public string AppVersion
    {
        get => _appVersion;
        set => SetProperty(ref _appVersion, value);
    }

    #endregion

    private async Task RefreshStationCacheAsync()
    {
        await _stationManager.RefreshAsync();
        Growl.Info(new GrowlInfo
        {
            Message = "数据加载完成",
            WaitTime = 1,
        });
    }
}
