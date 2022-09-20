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
        DeleteCommand = new AsyncRelayCommand<TDataSource>(DoDeleteAsync!);
       
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
        
        OnAfterSave(ok, err);
    }

    /// <summary>
    /// 删除数据、
    /// </summary>
    /// <param name="data"></param>
    protected virtual Task<(bool ok, string? err)> OnDeleteAsync(TDataSource data)
    {
        return Task.FromResult((true, (string?)""));
    }

    private async Task DoDeleteAsync(TDataSource data)
    {
        // 采用 MessageBox 对话框形式确认删除。
        // 部分情况下，Growl.Ask 中采用异步转同步会出现界面阻塞（假死）情况。
        // 异步使用 TaskFunc.ConfigureAwait(false).GetAwaiter().GetResult() 可转为同步操作，但在使用 SqlSugarCore 导航属性删除时，在弹出确认框确定时会出现阻塞情况。
        // 另外一点，弹出的确认框不是 Modal 形式，会导致有多个弹出框时删除的都是最后一条数据。
        if (HandyControl.Controls.MessageBox.Show("确认要删除？", "删除", MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK)
        {
            return;
        }

        var (ok, err) = await OnDeleteAsync(data!);
        AfterDelete(data, ok, err);
    }
   
    private async Task PageUpdatedAsync(FunctionEventArgs<int> e)
    {
        await DoSearchAsync(e.Info, PageSize);
    }

    private async Task DownloadAsync()
    {
        try
        {
            var (confirm, filename, exportData, builder) = InnerDownload();
            if (confirm)
            {
                await DoSearchedMaxDataAsync();
                exportData!.Body = SearchedAllData;
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
