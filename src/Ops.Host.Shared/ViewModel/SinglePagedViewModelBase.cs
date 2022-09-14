using HandyControl.Controls;
using HandyControl.Data;

namespace Ops.Host.Shared.ViewModel;

/// <summary>
/// 单数据源分页 ViewModel 基类。
/// </summary>
/// <typeparam name="TDataSource">数据源类型</typeparam>
public abstract class SinglePagedViewModelBase<TDataSource> : SinglePagedViewModelBase<TDataSource, NullQueryFilter>
    where TDataSource : class, new()
{

}

/// <summary>
/// 单数据源分页 ViewModel 基类。
/// </summary>
/// <typeparam name="TDataSource">数据源类型</typeparam>
/// <typeparam name="TQueryFilter">数据查询筛选类型</typeparam>
public abstract class SinglePagedViewModelBase<TDataSource, TQueryFilter> : PagedViewModelBase<TDataSource, TQueryFilter>
    where TDataSource : class, new()
    where TQueryFilter : class, new()
{
    protected SinglePagedViewModelBase()
    {
        QueryCommand = new RelayCommand(() => DoSearch(1, PageSize));
        PageUpdatedCommand = new RelayCommand<FunctionEventArgs<int>>((e) => PageUpdated(e!));

        SaveCommand = new RelayCommand(DoSave);
        DeleteCommand = new RelayCommand<TDataSource>(DoDelete);

        DownloadCommand = new RelayCommand(Download);
        PrintCommand = new RelayCommand(Print);
    }

    /// <summary>
    /// 初始化查询。
    /// </summary>
    protected void InitSearch()
    {
        DoSearch(1, PageSize);
    }

    /// <summary>
    /// 查询数据。
    /// </summary>
    /// <param name="pageIndex">页数</param>
    /// <param name="pageSize">每页数量</param>
    protected abstract PagedList<TDataSource> OnSearch(int pageIndex, int pageSize);

    /// <summary>
    /// 更新/新增数据。
    /// 根据实体 Id 判断是新增还是更新的数据。
    /// </summary>
    /// <returns></returns>
    protected virtual (bool ok, string? err) OnSave(TDataSource data)
    {
        return (true, default);
    }

    private void DoSave()
    {
        OnBeforeSave(SelectedItem!);

        bool ok;
        string? err;
        (ok, err) = OnValidateModel(SelectedItem!);
        if (ok)
        {
            (ok, err) = OnSave(SelectedItem!);
        }

        OnAfterSave(ok, err);
    }

    /// <summary>
    /// 删除数据、
    /// </summary>
    /// <param name="data"></param>
    protected virtual (bool ok, string? err) OnDelete(TDataSource data)
    {
        return (true, default);
    }

    private void DoDelete(TDataSource? data)
    {
        InnerDelete(data, OnDelete!);
    }

    private void PageUpdated(FunctionEventArgs<int> e)
    {
        DoSearch(e.Info, PageSize);
    }

    private void Download()
    {
        try
        {
            var (confirm, filename, exportData, builder) = InnerDownload(); // 确认导出后再查询数据
            if (confirm)
            {
                DoSearchedMaxData();
                exportData!.Body = SearchedAllData;
                Excel.Export(filename!, builder.SheetName!, exportData!, builder.Settings);
            }
        }
        catch (Exception ex)
        {
            Growl.Error($"数据导出失败, 错误：{ex.Message}");
        }
    }

    private void Print()
    {
        try
        {
            DoSearchedMaxData();
            InnerPrint();
        }
        catch (Exception ex)
        {
            Growl.Error($"数据打印失败, 错误：{ex.Message}");
        }
    }

    private void DoSearchedMaxData()
    {
        var pagedList = OnSearch(1, short.MaxValue);
        SearchedAllData = pagedList.Items;
    }

    private void DoSearch(int pageIndex, int pageSize)
    {
        var pagedList = OnSearch(pageIndex, pageSize);

        PageCount = pagedList.TotalPages;
        DataSourceList = new ObservableCollection<TDataSource>(pagedList.Items);
    }
}
