namespace Ops.Host.Core.Services;

/// <summary>
/// Scada 领域服务基类。
/// </summary>
public abstract class ScadaDomainService
{
    /// <summary>
    /// OK
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public ReplyResult Ok(IDictionary<string, object>? values = null)
    {
        return From(0, values);
    }

    /// <summary>
    /// Error
    /// </summary>
    /// <param name="code">错误代码</param>
    /// <returns></returns>
    public ReplyResult Error(short code = 2)
    {
        return From(code);
    }

    /// <summary>
    /// 自定义返回结果。
    /// </summary>
    /// <param name="code">代码</param>
    /// <param name="values">值</param>
    /// <returns></returns>
    public ReplyResult From(short code, IDictionary<string, object>? values = null)
    {
        var result = new ReplyResult()
        {
            Result = code,
        };

        if (values != null)
        {
            foreach (var item in values)
            {
                result.AddValue(item.Key, item.Value);
            }
        }

        return result;
    }
}
