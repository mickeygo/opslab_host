namespace Ops.Host.App.ViewModels;

public sealed class DictDataViewModel : AsyncSinglePagedViewModelBase<SysDictData, SysDictDataFilter>, IViewModel
{
    protected override Task<PagedList<SysDictData>> OnSearchAsync(int pageIndex, int pageSize)
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> SaveAsync(SysDictData data)
    {
        return base.SaveAsync(data);
    }

    protected override Task<bool> DeleteAsync(SysDictData data)
    {
        return base.DeleteAsync(data);
    }
}
