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
        return From((short)0, values);
    }

    /// <summary>
    /// 返回错误结果。
    /// </summary>
    /// <param name="code">错误编码</param>
    /// <returns></returns>
    public ReplyResult Error(ErrorCodeEnum code)
    {
        return Error((short)code);
    }

    /// <summary>
    /// 返回错误结果。
    /// </summary>
    /// <param name="code">错误编码</param>
    /// <returns></returns>
    public ReplyResult Error(short code = 2)
    {
        return From(code);
    }

    /// <summary>
    /// 自定义返回结果。
    /// </summary>
    /// <param name="code">编码枚举</param>
    /// <param name="values">值</param>
    /// <returns></returns>
    public ReplyResult From(ErrorCodeEnum code, IDictionary<string, object>? values = null)
    {
        return From((short)code, values);
    }

    /// <summary>
    /// 自定义返回结果。
    /// </summary>
    /// <param name="code">编码</param>
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
