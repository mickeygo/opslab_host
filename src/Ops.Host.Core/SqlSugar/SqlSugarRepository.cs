namespace Ops.Host.Core;

/// <summary>
/// SqlSugar仓储类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SqlSugarRepository<T> : SimpleClient<T> where T : class, new()
{
    public SqlSugarRepository(ISqlSugarClient? context = null) : base(context) // 默认值等于null不能少
    {
        base.Context = context; // ioc注入的对象
    }
}
