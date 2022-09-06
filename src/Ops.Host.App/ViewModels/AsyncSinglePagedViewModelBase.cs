﻿using System.Windows.Threading;
using Microsoft.Win32;
using HandyControl.Controls;
using HandyControl.Data;
using Ops.Host.Common.IO;
using System.Windows.Markup;

namespace Ops.Host.App.ViewModels;

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
public abstract class AsyncSinglePagedViewModelBase<TDataSource, TQueryFilter> : ObservableObject
    where TDataSource : class, new()
    where TQueryFilter : class, new()
{
    private long _pageCount;
    private ObservableCollection<TDataSource>? _dataSourceList;
    private TQueryFilter _queryFilter = new();
    private TDataSource? _selectedItem;
    private bool _isOpenSidebar = false;
    private bool _isAdding = false;

    /// <summary>
    /// 每页数量，默认 20 条。
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// 获取或设置控件来源
    /// </summary>
    public Control? Owner { get; set; }

    /// <summary>
    /// 已根据筛选条件查询的所有数据。
    /// </summary>
    protected List<TDataSource> SearchedAllData { get; private set; } = new();

    protected AsyncSinglePagedViewModelBase()
    {
        QueryCommand = new AsyncRelayCommand(() => DoSearchAsync(1, PageSize));
        PageUpdatedCommand = new AsyncRelayCommand<FunctionEventArgs<int>>(PageUpdatedAsync!);

        AddCommand = new RelayCommand(Add);
        EditCommand = new RelayCommand<TDataSource>(Edit!);
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

    #region 绑定属性

    /// <summary>
    /// 总页数。
    /// </summary>
    public long PageCount
    {
        get => _pageCount;
        set => SetProperty(ref _pageCount, value);
    }

    /// <summary>
    /// 数据源。
    /// </summary>
    public ObservableCollection<TDataSource>? DataSourceList
    {
        get => _dataSourceList;
        set => SetProperty(ref _dataSourceList, value);
    }

    /// <summary>
    /// 查询筛选器。
    /// </summary>
    public TQueryFilter QueryFilter
    {
        get => _queryFilter;
        set => SetProperty(ref _queryFilter, value);
    }

    /// <summary>
    /// 选中的数据行
    /// </summary>
    public TDataSource? SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }

    /// <summary>
    /// 是否开启侧边编辑栏
    /// </summary>
    public bool IsOpenSidebar
    {
        get => _isOpenSidebar;
        set => SetProperty(ref _isOpenSidebar, value);
    }

    /// <summary>
    /// 是否为新增动作，否则为编辑。
    /// </summary>
    public bool IsAdding
    {
        get => _isAdding;
        set => SetProperty(ref _isAdding, value);
    }

    #endregion

    #region 绑定事件

    /// <summary>
    /// 数据查询事件。
    /// </summary>
    public ICommand QueryCommand { get; }

    /// <summary>
    /// 新增
    /// </summary>
    public ICommand AddCommand { get; }

    /// <summary>
    /// 编辑
    /// </summary>
    public ICommand EditCommand { get; }

    /// <summary>
    /// 保存（新增/更新）的数据
    /// </summary>
    public ICommand SaveCommand { get; }

    /// <summary>
    /// 删除
    /// </summary>
    public ICommand DeleteCommand { get; }

    /// <summary>
    /// 数据查询分页事件。
    /// </summary>
    public ICommand PageUpdatedCommand { get; }

    /// <summary>
    /// 导出查询的数据。
    /// </summary>
    public ICommand DownloadCommand { get; }

    /// <summary>
    /// 打印查询出的数据。
    /// </summary>
    public ICommand PrintCommand { get; }

    #endregion

    /// <summary>
    /// 查询数据。
    /// </summary>
    /// <param name="pageIndex">页数</param>
    /// <param name="pageSize">每页数量</param>
    protected abstract Task<PagedList<TDataSource>> OnSearchAsync(int pageIndex, int pageSize);

    private void Add()
    {
        SelectedItem = new();
        IsOpenSidebar = true;
        IsAdding = true;
    }

    private void Edit(TDataSource data)
    {
        SelectedItem = data;
        IsOpenSidebar = true;
        IsAdding = false;
    }

    /// <summary>
    /// 更新/新增数据。
    /// 根据实体 Id 判断是新增还是更新的数据。
    /// </summary>
    /// <returns></returns>
    protected virtual Task<(bool ok, string? err)> SaveAsync(TDataSource data)
    {
        return Task.FromResult((true, (string?)""));
    }

    private async Task DoSaveAsync()
    {
        var (ok, err) = await SaveAsync(SelectedItem!);
        if (ok)
        {
            Growl.Info(new GrowlInfo
            {
                Message = "数据更新成功",
                WaitTime = 1,
            });
            
            if (IsAdding)
            {
                DataSourceList?.Add(SelectedItem!);
            }

            IsOpenSidebar = false;
        }
        else
        {
            Growl.Error($"更新失败：{err ?? ""}");
        }
    }

    /// <summary>
    /// 删除数据、
    /// </summary>
    /// <param name="data"></param>
    protected virtual Task<(bool ok, string? err)> DeleteAsync(TDataSource data)
    {
        return Task.FromResult((true, (string?)""));
    }

    private void DoDelete(TDataSource? data)
    {
        if (data == null)
        {
            Growl.Info(new GrowlInfo
            {
                Message = "没有选择要删除的数据",
                WaitTime = 1,
            });
            return;
        }

        Growl.Ask("确定要删除？", isConfirmed =>
        {
            if (isConfirmed)
            {
                var (ok, err) = DeleteAsync(data).ConfigureAwait(false).GetAwaiter().GetResult(); // Growl 中异步无效
                if (ok)
                {
                    DataSourceList?.Remove(data);
                    Growl.Info(new GrowlInfo
                    {
                        Message = "数据删除成功",
                        WaitTime = 1,
                    });
                }
                else
                {
                    Growl.Error($"更新失败：{err ?? ""}");
                }
            }

            return true;
        });
    }

    /// <summary>
    /// Excel 下载参数设置。
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void OnExcelCreating(ExcelModelBuilder builder)
    {
       
    }

    /// <summary>
    /// 打印参数设置。
    /// </summary>
    protected virtual void OnPrintCreating(PrintModelBuilder builder)
    {
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

            ExcelModelBuilder builder = new();
            OnExcelCreating(builder);
            builder.ExcelName ??= DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SheetName ??= Path.GetFileNameWithoutExtension(builder.ExcelName);

            SaveFileDialog saveFile = new()
            {
                Filter = "导出文件 （*.xlsx）|*.xlsx",
                FilterIndex = 0,
                FileName = builder.ExcelName!,
            };

            if (saveFile.ShowDialog() != true)
            {
                return;
            }

            var fileName = saveFile.FileName;
            ExcelExportData<TDataSource> exportData = new()
            {
                Header = builder.Header,
                Body = SearchedAllData,
                Footer = builder.Footer,
            };
            await Excel.ExportAsync(fileName, builder.SheetName, exportData, builder.Settings);
        }
        catch (Exception ex)
        {
            Growl.Error($"数据导出失败, 错误：{ex.Message}");
        }
    }

    private async Task PrintAsync()
    {
        PrintModelBuilder builder = new();

        try
        {
            await DoSearchedMaxDataAsync();
            OnPrintCreating(builder);

            if (builder.Mode == PrintModelBuilder.PrintMode.Preview)
            {
                // TODO：若是打开失败后，关闭主窗体应用程序依旧存在的 bug。
                PrintPreviewWindow? previewWnd = null;
                try
                {
                    previewWnd = new(builder.TemplateUrl!, builder.DataContext, builder.Render)
                    {
                        Owner = System.Windows.Application.Current.MainWindow,
                        ShowInTaskbar = false
                    };
                    previewWnd.ShowDialog();
                }
                catch
                {
                    previewWnd?.Close();
                    throw;
                }
            }
            else if (builder.Mode == PrintModelBuilder.PrintMode.Dialog)
            {
                PrintDialog pdlg = new();
                if (pdlg.ShowDialog() == true)
                {
                    FlowDocument doc = PrintPreviewWindow.LoadDocument(builder.TemplateUrl!, builder.DataContext, builder.Render);
                    Owner?.Dispatcher.BeginInvoke(new DoPrintDelegate(DoPrint), DispatcherPriority.ApplicationIdle, pdlg, ((IDocumentPaginatorSource)doc).DocumentPaginator);
                }
            }
            else if (builder.Mode == PrintModelBuilder.PrintMode.Direct)
            {
                PrintDialog pdlg = new();
                FlowDocument doc = PrintPreviewWindow.LoadDocument(builder.TemplateUrl!, builder.DataContext, builder.Render);
                Owner?.Dispatcher.BeginInvoke(new DoPrintDelegate(DoPrint), DispatcherPriority.ApplicationIdle, pdlg, ((IDocumentPaginatorSource)doc).DocumentPaginator);
            }
        }
        catch (Exception ex)
        {
            Growl.Error($"数据打印失败, 错误：{ex.Message}");
        }

        void DoPrint(PrintDialog pdlg, DocumentPaginator paginator)
        {
            pdlg.PrintDocument(paginator, builder.DocumentDescription);
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
