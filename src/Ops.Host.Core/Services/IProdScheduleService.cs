namespace Ops.Host.Core.Services;

/// <summary>
/// 生产排程服务
/// </summary>
public interface IProdScheduleService : IDomainService
{
    /// <summary>
    /// 获取所有处于下发状态的工单。
    /// </summary>
    /// <returns></returns>
    List<ProdWo> GetAllIssue();

    /// <summary>
    /// 获取所有处于下发状态的工单。
    /// </summary>
    /// <returns></returns>
    Task<List<ProdWo>> GetAllIssueAsync();

    /// <summary>
    /// 获取所有排程的工单。工单状态为 [已下发, 完工)。
    /// </summary>
    /// <returns></returns>
    List<ProdSchedule> GetAllSchedule();

    /// <summary>
    /// 获取所有排程的工单。工单状态为 [已下发, 完工)。
    /// </summary>
    /// <returns></returns>
    Task<List<ProdSchedule>> GetAllScheduleAsync();

    /// <summary>
    /// 获取待做的工单。
    /// </summary>
    /// <remarks>若没有找到满足条件的工单，返回null。</remarks>
    /// <returns></returns>
    Task<ProdSchedule?> GetToDoWoAsync();

    /// <summary>
    /// 排产
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<(bool ok, ProdSchedule? schedule, string? err)> ScheduleAsync(ProdWo input);

    /// <summary>
    /// 反排产
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<(bool ok, ProdWo? wo, string? err)> DisScheduleAsync(ProdSchedule input);

    /// <summary>
    /// 排产数据上移
    /// </summary>
    /// <param name="current"></param>
    /// <param name="prev"></param>
    /// <returns></returns>
    Task<(bool ok, string? err)> UpScheduleAsync(ProdSchedule current, ProdSchedule prev);

    /// <summary>
    /// 排产数据下移
    /// </summary>
    /// <param name="current"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    Task<(bool ok, string? err)> DownScheduleAsync(ProdSchedule current, ProdSchedule next);
}
