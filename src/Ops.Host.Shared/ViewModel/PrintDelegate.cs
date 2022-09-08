namespace Ops.Host.Shared.ViewModel;

/// <summary>
/// 打印委托。
/// </summary>
/// <param name="pdlg">打印对话框</param>
/// <param name="paginator">打印分页文档</param>
public delegate void PrintDelegate(PrintDialog pdlg, DocumentPaginator paginator);
