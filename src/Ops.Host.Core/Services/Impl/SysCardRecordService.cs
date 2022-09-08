namespace Ops.Host.Core.Services.Impl
{
    internal sealed class SysCardRecordService : ISysCardRecordService
    {
        private readonly SqlSugarRepository<SysCardRecord> _cardRecordRep;

        public SysCardRecordService(SqlSugarRepository<SysCardRecord> cardRecordRep)
        {
            _cardRecordRep = cardRecordRep;
        }

        public async Task<PagedList<SysCardRecord>> GetPagedListAsync(SysCardRecordFilter filter, int pageIndex, int pageSize)
        {
            return await _cardRecordRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(filter.CardNo), s => s.CardNo!.Contains(filter.CardNo!))
                .WhereIF(!string.IsNullOrWhiteSpace(filter.Owner), s => s.Owner!.Contains(filter.Owner!))
                .WhereIF(!string.IsNullOrWhiteSpace(filter.CardDeviceName), s => s.CardDeviceName!.Contains(filter.CardDeviceName!))
                .WhereIF(filter.CreateTimeStart != null, s => s.CreateTime >= filter.CreateTimeStart.ToDayMin())
                .WhereIF(filter.CreateTimeEnd != null, s => s.CreateTime <= filter.CreateTimeEnd.ToDayMax())
                .OrderBy(s => s.CreateTime, OrderByType.Desc)
                .ToPagedListAsync(pageIndex, pageSize);
        }
    }
}
