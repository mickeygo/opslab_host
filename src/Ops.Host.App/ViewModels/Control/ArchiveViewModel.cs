using Microsoft.Win32;
using Ops.Host.Common.IO;

namespace Ops.Host.App.ViewModels;

public sealed class ArchiveViewModel : AsyncSinglePagedViewModelBase<PtArchive, PtArchiveFilter>, IViewModel
{
    private readonly IPtArchiveService _archiveService;
    private readonly StationCacheManager _stationManager;
    private readonly ILogger _logger;

    public ArchiveViewModel(IPtArchiveService archiveService, StationCacheManager stationManager, ILogger<ArchiveViewModel> logger)
    {
        _archiveService = archiveService;
        _stationManager = stationManager;
        _logger = logger;

        MyDownloadCommand = new AsyncRelayCommand(MyExportTableAsync);
    }

    public List<NameValue> LineDropdownList => _stationManager.Lines;

    public List<NameValue> StationDropdownList => _stationManager.Stations;

    public ICommand MyDownloadCommand { get; }

    protected override async Task<PagedList<PtArchive>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _archiveService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }

    private async Task MyExportTableAsync()
    {
        if (string.IsNullOrWhiteSpace(QueryFilter.LineCode))
        {
            NoticeWarning("请先选择 [产线]");
            return;
        }

        if (string.IsNullOrWhiteSpace(QueryFilter.StationCode))
        {
            NoticeWarning("请先选择 [工站]");
            return;
        }

        try
        {
            SaveFileDialog saveFile = new()
            {
                Filter = "导出文件 （*.xlsx）|*.xlsx",
                FilterIndex = 0,
                FileName = $"{QueryFilter.LineCode}-{QueryFilter.StationCode}",
            };

            if (saveFile.ShowDialog() != true)
            {
                return;
            }

            var dataTable = await _archiveService.GetDataTable(QueryFilter);
            await Excel.ExportAsync(saveFile.FileName!, "过站信息", dataTable);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Archive] 导出数据异常");
            NoticeWarning($"导出数据错误，原因：{ex.Message}");
        }
    }
}
