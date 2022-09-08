using Microsoft.Win32;
using HandyControl.Controls;
using HandyControl.Data;

namespace Ops.Host.Shared.ViewModel;

/// <summary>
/// 单数据源分页 ViewModel 基类。
/// </summary>
/// <typeparam name="TDataSource">数据源类型</typeparam>
public abstract class AsyncSinglePagedViewModelBase<TDataSource> : AsyncSinglePagedViewModelBase<TDataSource, NullQueryFilter>
    where TDataSource : class, new()
{

}

/// <summary>
/// 基于异步的单数据源分页 ViewModel 基类。
/// </summary>
/// <typeparam name="TDataSource">数据源类型</typeparam>
/// <typeparam name="TQueryFilter">数据查询筛选类型</typeparam>
public abstract class AsyncSinglePagedViewModelBase<TDataSource, TQueryFilter> : PagedViewModelBase<TDataSource, TQueryFilter>
    where TDataSource : class, new()
    where TQueryFilter : class, new()
{
    protected AsyncSinglePagedViewModelBase()
    {
        QueryCommand = new AsyncRelayCommand(() => DoSearchAsync(1, PageSize));
        PageUpdatedCommand = new AsyncRelayCommand<FunctionEventArgs<int>>(PageUpdatedAsync!);

        SaveCommand = new AsyncRelayCommand(DoSaveAsync);
        DeleteCommand = new RelayCommand<TDataSource>(DoDelete);
       
        DownloadCommand = new AsyncRelayCommand(DownloadAsync!);
        PrintCommand = new AsyncRelayCommand(PrintAsync);
    }

    /// <summary>
    /// 初始化查询。
    /// </summary>
    /// <returns></returns>
    protected async Task InitSearchAsync()
    {
        await DoSearchAsync(1, PageSize);
    }

    /// <summary>
    /// 查询数据。
    /// </summary>
    /// <param name="pageIndex">页数</param>
    /// <param name="pageSize">每页数量</param>
    protected abstract Task<PagedList<TDataSource>> OnSearchAsync(int pageIndex, int pageSize);

    /// <summary>
    /// 更新/新增数据。
    /// 根据实体 Id 判断是新增还是更新的数据。
    /// </summary>
    /// <returns></returns>
    protected virtual Task<(bool ok, string? err)> OnSaveAsync(TDataSource data)
    {
        return Task.FromResult((true, (string?)""));
    }

    private async Task DoSaveAsync()
    {
        OnBeforeSave(SelectedItem!);

        bool ok;
        string? err;
        (ok, err) = OnValidateModel(SelectedItem!);
        if (ok)
        {
            (ok, err) = await OnSaveAsync(SelectedItem!);
        }
        
        InnerAfterSave(ok, err);

        OnAfterSave(SelectedItem!);
    }

    /// <summary>
    /// 删除数据、
    /// </summary>
    /// <param name="data"></param>
    protected virtual Task<(bool ok, string? err)> OnDeleteAsync(TDataSource data)
    {
        return Task.FromResult((true, (string?)""));
    }

    private void DoDelete(TDataSource? data)
    {
        InnerDelete(data, Del!);

        (bool, string?) Del(TDataSource data)
        {
            return OnDeleteAsync(data).ConfigureAwait(false).GetAwaiter().GetResult(); // Growl 中异步无效
        }
    }

    private async Task PageUpdatedAsync(FunctionEventArgs<int> e)
    {
        await DoSearchAsync(e.Info, PageSize);
    }

    private async Task DownloadAsync()
    {
        try
        {
            await DoSearchedMaxDataAsync();
            var (confirm, filename, exportData, builder) = InnerDownload();
            if (confirm)
            {
                await Excel.ExportAsync(filename!, builder.SheetName!, exportData!, builder.Settings);
            }
        }
        catch (Exception ex)
        {
            Growl.Error($"数据导出失败, 错误：{ex.Message}");
        }
    }

    private async Task PrintAsync()
    {
        try
        {
            await DoSearchedMaxDataAsync();
            InnerPrint();
        }
        catch (Exception ex)
        {
            Growl.Error($"数据打印失败, 错误：{ex.Message}");
        }
    }

    private async Task DoSearchedMaxDataAsync()
    {
        var pagedList = await OnSearchAsync(1, short.MaxValue);
        SearchedAllData = pagedList.Items;
    }

    private async Task DoSearchAsync(int pageIndex, int pageSize)
    {
        var pagedList = await OnSearchAsync(pageIndex, pageSize);

        PageCount = pagedList.TotalPages;
        DataSourceList = new ObservableCollection<TDataSource>(pagedList.Items);
    }
}
