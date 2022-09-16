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

    private void DoDelete(TDataSource? data)
    {
        InnerDelete(data, Del!);

        (bool, string?) Del(TDataSource data)
        {
            // Growl 中异步无效
            // 注意：异步可能会导致阻塞当前线程。

            // TODO: 使用 SqlSugarCore 导航属性删除时，在弹出确认框确定时会出现阻塞情况。
            // return OnDeleteAsync(data).ConfigureAwait(false).GetAwaiter().GetResult();
            AwaitAndThrowIfFailed(OnDeleteAsync(data));

            // 转换为同步后没有返回数据，目前只能返回 true。
            return (true, "");
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

    /// <summary>
    /// Awaits an input <see cref="Task"/> and throws an exception on the calling context, if the task fails.
    /// </summary>
    /// <param name="executionTask">The input <see cref="Task"/> instance to await.</param>
    internal static async void AwaitAndThrowIfFailed(Task executionTask)
    {
        // Note: this method is purposefully an async void method awaiting the input task. This is done so that
        // if an async relay command is invoked synchronously (ie. when Execute is called, eg. from a binding),
        // exceptions in the wrapped delegate will not be ignored or just become visible through the ExecutionTask
        // property, but will be rethrown in the original synchronization context by default. This makes the behavior
        // more consistent with how normal commands work (where exceptions are also just normally propagated to the
        // caller context), and avoids getting an app into an inconsistent state in case a method faults without
        // other components being notified. It is also possible to not await this task and to instead ignore exceptions
        // and then inspect them manually from the ExecutionTask property, by constructing an async command instance
        // using the AsyncRelayCommandOptions.FlowExceptionsToTaskScheduler option. That will cause this call to
        // be skipped, and exceptions will just either normally be available through that property, or will otherwise
        // flow to the static TaskScheduler.UnobservedTaskException event if otherwise unobserved (eg. for logging).
        await executionTask;
    }
}
