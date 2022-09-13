namespace Ops.Host.Core;

/// <summary>
/// 状态码，需注意自定义状态码不能冲突。具体可参考 <see cref="Exchange.ExStatusCode"/>
/// <para>【0】 初始状态/成功处理状态；</para>
/// <para>【1】 数据触发状态；</para>
/// <para>【2】 内部异常统一状态；</para>
/// <para>【400~499】 请求数据异常，如参数为空、数据类型不对等；</para>
/// <para>【500~599】 内部异常，如超时、崩溃等；</para>
/// <para>【1100~1199】 进站详细状态；</para>
/// <para>【1200~1299】 出站详细状态；</para>
/// <para>【1300~1399】 扫码详细状态；</para>
/// <para>【1400~1499】 其他状态。</para>
/// </summary>
public enum ErrorCodeEnum
{
    /// <summary>
    /// 成功
    /// </summary>
    [Description("成功")]
    Success = 0,

    /// <summary>
    /// 异常
    /// </summary>
    [Description("异常")]
    Error = 2,

    E0401 = 401,

    /// <summary>
    /// 过站结果不为空
    /// </summary>
    [Description("过站结果不为空")]
    E0402,

    /// <summary>
    /// PLC程序配方号不能为空
    /// </summary>
    [Description("PLC程序配方号不能为空")]
    E0403,

    /// <summary>
    /// SN 不能为空
    /// </summary>
    [Description("SN不能为空")]
    E0404,

    /// <summary>
    /// Pass 值不正确
    /// </summary>
    [Description("Pass 值不正确")]
    E1201 = 1201,

    /// <summary>
    /// 没有进站信息
    /// </summary>
    [Description("没有进站信息")]
    E1202,

    E1300 = 1300,

    /// <summary>
    /// Barcode 为空。
    /// </summary>
    [Description("Barcode 为空")]
    E1301,

    /// <summary>
    /// 没有进站信息
    /// </summary>
    [Description("没有进站信息")]
    E1302,

    /// <summary>
    /// Barcode已绑定
    /// </summary>
    [Description("Barcode已绑定")]
    E1303,

    /// <summary>
    /// 该产品在该站没有下发 BOM。
    /// </summary>
    [Description("没有下发BOM")]
    E1304,

    /// <summary>
    /// Barcode校验不匹配
    /// </summary>
    [Description("Barcode校验不匹配")]
    E1305,

    /// <summary>
    /// 不是关键物料
    /// </summary>
    [Description("不是关键物料")]
    E1306,

    /// <summary>
    /// 物料索引超出范围
    /// </summary>
    [Description("物料索引超出范围")]
    E1307,
}
