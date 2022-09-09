using Microsoft.Win32;
using HandyControl.Controls;
using HandyControl.Data;

namespace Ops.Host.Shared.ViewModel;

/// <summary>
/// 分页 ViewModel 基类。
/// </summary>
public abstract class PagedViewModelBase<TDataSource, TQueryFilter> : ObservableObject
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
    /// 已根据筛选条件查询的所有数据（用于导出和打印）。
    /// </summary>
    protected List<TDataSource> SearchedAllData { get; set; } = new();

    public PagedViewModelBase()
    {
        AddCommand = new RelayCommand(Add);
        EditCommand = new RelayCommand<TDataSource>(Edit!);
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
    public ICommand? QueryCommand { get; protected set; }

    /// <summary>
    /// 新增
    /// </summary>
    public ICommand? AddCommand { get; protected set; }

    /// <summary>
    /// 编辑
    /// </summary>
    public ICommand? EditCommand { get; protected set; }

    /// <summary>
    /// 保存（新增/更新）的数据
    /// </summary>
    public ICommand? SaveCommand { get; protected set; }

    /// <summary>
    /// 删除
    /// </summary>
    public ICommand? DeleteCommand { get; protected set; }

    /// <summary>
    /// 数据查询分页事件。
    /// </summary>
    public ICommand? PageUpdatedCommand { get; protected set; }

    /// <summary>
    /// 导出查询的数据。
    /// </summary>
    public ICommand? DownloadCommand { get; protected set; }

    /// <summary>
    /// 打印查询出的数据。
    /// </summary>
    public ICommand? PrintCommand { get; protected set; }

    #endregion

    /// <summary>
    /// 构建Excel参数模型。
    /// </summary>
    /// <param name="builder"></param>
    /// <remarks>导出 Excel 的 Header 优先使用导出类型的 <see cref="DisplayNameAttribute"/> 名称，若没有会使用类型的属性名。Excel 列顺序与属性顺序一致。</remarks>
    protected virtual void OnExcelModelCreating(ExcelModelBuilder builder)
    {

    }

    /// <summary>
    /// 构建打印参数模型。
    /// </summary>
    protected virtual void OnPrintModelCreating(PrintModelBuilder builder)
    {
    }

    /// <summary>
    /// 数据保存前（包括验证）处理方法。
    /// </summary>
    /// <param name="data">要保存的数据</param>
    protected virtual void OnBeforeSave(TDataSource data)
    {

    }

    /// <summary>
    /// 数据保存后处理方法。
    /// </summary>
    /// <param name="data">保存后的数据</param>
    protected virtual void OnAfterSave(TDataSource data)
    {

    }

    /// <summary>
    /// 验证要新增或更新的 TDataSource 对象是否满足要求。
    /// </summary>
    /// <param name="trim">是否对非空的字符串进行 Trim() 操作，默认为 true。</param>
    /// <returns></returns>
    protected virtual (bool ok, string? err) OnValidateModel(TDataSource data, bool trim = true)
    {
        var props = typeof(TDataSource).GetProperties();
        foreach (var prop in props)
        {
            // string 类型数据。
            if (prop.PropertyType == typeof(string))
            {
                var v = (string?)prop.GetValue(SelectedItem);

                // 设置了 RequiredAttribute 属性（此处表示不为 null 或 空白）。
                if (string.IsNullOrWhiteSpace(v) && prop.GetCustomAttribute<RequiredAttribute>() is not null)
                {
                    var name = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? prop.Name;
                    return (false, $"[{name}] 字段不能为空");
                }

                // 允许 Required 后，null 字符串直接返回。
                if (v is null)
                {
                    return (true, "");
                }

                // 先移除两端空格。
                if (trim)
                {
                    v = v.Trim();
                }

                // 设置了 MaxLengthAttribute 属性，判断字符长度。
                var maxLenAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
                if (maxLenAttr is not null && v!.Length > maxLenAttr.Length)
                {
                    var name = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? prop.Name;
                    return (false, $"[{name}] 字段长度（{v?.Length}）超过了允许的最大长度 {maxLenAttr.Length}");
                }

                // 重置属性值，这里在检测最大长度后设置。
                if (trim)
                {
                    prop.SetValue(SelectedItem, v!.Trim());
                }
            }
            else if (prop.PropertyType.IsConstructedGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) // 可空值类型
            {
                var v = prop.GetValue(SelectedItem);

                // 设置了 RequiredAttribute 属性。
                if (v is null && prop.GetCustomAttribute<RequiredAttribute>() is not null)
                {
                    var name = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? prop.Name;
                    return (false, $"[{name}] 字段不能为空");
                }
            }
        }

        return (true, "");
    }

    internal protected void InnerDelete(TDataSource? data, Func<TDataSource, (bool, string)> deleteAction)
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
                var (ok, err) = deleteAction(data); // Growl 中异步无效
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
                    Growl.Error($"删除失败: {err}");
                }
            }

            return true;
        });
    }

    internal protected (bool, string?, ExcelExportData<TDataSource>?, ExcelModelBuilder) InnerDownload()
    {
        ExcelModelBuilder builder = new();
        OnExcelModelCreating(builder);

        if (!string.IsNullOrEmpty(builder.ExcelName))
        {
            builder.SheetName ??= builder.ExcelName;
            if (builder.HasExcelNameDatePostfix)
            {
                builder.ExcelName += DateTime.Now.ToString("yyyyMMdd");
            }
        }
        else
        {
            builder.ExcelName ??= DateTime.Now.ToString("yyyyMMddHHmm");
            builder.SheetName ??= "Sheet1";
        }

        // 弹出保存对话框
        SaveFileDialog saveFile = new()
        {
            Filter = "导出文件 （*.xlsx）|*.xlsx",
            FilterIndex = 0,
            FileName = builder.ExcelName!,
        };

        if (saveFile.ShowDialog() != true)
        {
            return (false, default, default, builder);
        }

        // 设置要排除的列名
        if (builder.IsOnlyDisplayNameColomn)
        {
            foreach (var prop in typeof(TDataSource).GetProperties())
            {
                if (prop.GetCustomAttribute<DisplayNameAttribute>() == null 
                    && !builder.Settings.Excludes.Contains(prop.Name))
                {
                    builder.Settings.Excludes.Add(prop.Name);
                }
            }
        }

        ExcelExportData<TDataSource> exportData = new()
        {
            Header = builder.Header,
            Body = SearchedAllData,
            Footer = builder.Footer,
        };

        return (true, saveFile.FileName, exportData, builder);
    }

    internal protected void InnerPrint()
    {
        PrintModelBuilder builder = new();

        OnPrintModelCreating(builder);

        if (builder.Mode == PrintModelBuilder.PrintMode.Preview)
        {
            // TODO：若是打开失败后，关闭主窗体应用程序依旧存在的 bug。
            PrintPreviewWindow? previewWnd = null;
            try
            {
                previewWnd = new(builder.TemplateUrl!, builder.DataContext, builder.Render)
                {
                    Owner = Application.Current.MainWindow,
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
                (Owner ?? Application.Current.MainWindow).Dispatcher.BeginInvoke(
                    new PrintDelegate(PrintDocument),
                    DispatcherPriority.ApplicationIdle,
                    pdlg,
                    ((IDocumentPaginatorSource)doc).DocumentPaginator);
            }
        }
        else if (builder.Mode == PrintModelBuilder.PrintMode.Direct)
        {
            PrintDialog pdlg = new();
            FlowDocument doc = PrintPreviewWindow.LoadDocument(builder.TemplateUrl!, builder.DataContext, builder.Render);
            (Owner ?? Application.Current.MainWindow)?.Dispatcher.BeginInvoke(
                new PrintDelegate(PrintDocument),
                DispatcherPriority.ApplicationIdle,
                pdlg,
                ((IDocumentPaginatorSource)doc).DocumentPaginator);
        }

        void PrintDocument(PrintDialog pdlg, DocumentPaginator paginator)
        {
            pdlg.PrintDocument(paginator, builder.DocumentDescription);
        }
    }

    internal protected void InnerAfterSave(bool ok, string? err)
    {
        var desc = IsAdding ? "新增" : "更新";

        if (ok)
        {
            Growl.Info(new GrowlInfo
            {
                Message = $"数据{desc}成功",
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
            Growl.Error($"{desc}失败：{err ?? ""}");
        }
    }

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
}
