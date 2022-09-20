namespace Ops.Host.Core;

/// <summary>
/// 工单状态枚举
/// </summary>
public enum WoStatusEnum
{
    /// <summary>
    /// 已创建。
    /// </summary>
    [ResourceBrush("DarkPrimaryBrush")]
    [Description("已创建")]
    Created = 10,

    /// <summary>
    /// 已下发。
    /// <para>在排产前可取消下发，回滚至上一状态。</para>
    /// </summary>
    [ResourceBrush("DarkWarningBrush")]
    [Description("已下发")]
    Issued = 20,

    /// <summary>
    /// 已排产。
    /// <para>在生产前可取消排产，回滚至上一状态。</para>
    /// </summary>
    [ResourceBrush("DarkInfoBrush")]
    [Description("已排产")]
    Scheduled = 30,

    /// <summary>
    /// 生产中。
    /// </summary>
    [ResourceBrush("DarkSuccessBrush")]
    [Description("生产中")]
    Producing = 40,

    /// <summary>
    /// 暂停。
    /// <para>工单在 [生产中] 时可暂停，此过程可逆。</para>
    /// </summary>
    [ResourceBrush("DarkDangerBrush")]
    [Description("已暂停")]
    Paused = 45,

    /// <summary>
    /// 已完工。
    /// <para>自动/手动关单，要满足 产品数量＝已完工数量＋报废数量+拆解数量。</para>
    /// </summary>
    [ResourceBrush("DarkSuccessBrush")]
    [Description("已完工")]
    Completed = 50,
}
