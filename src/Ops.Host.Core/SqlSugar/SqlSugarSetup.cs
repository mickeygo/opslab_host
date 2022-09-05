namespace Ops.Host.Core;

/// <summary>
/// SqlSugar 配置。
/// </summary>
public static class SqlSugarSetup
{
    /// <summary>
    /// Sqlsugar 上下文初始化
    /// </summary>
    /// <param name="services"></param>
    public static void AddSqlSugarSetup(this IServiceCollection services)
    {
        services.AddSingleton<ISqlSugarClient>(sp => MakeSqlSugarScope(sp, services)); // 单例注册
        services.AddScoped(typeof(SqlSugarRepository<>)); // 注册仓储
    }

    private static SqlSugarScope MakeSqlSugarScope(IServiceProvider sp, IServiceCollection services)
    {
        var configuration = sp.GetRequiredService<IConfiguration>();
        var dbOptions = configuration.GetSection("DbConnection").Get<DbConnectionOptions>();

        var configureExternalServices = new ConfigureExternalServices
        {
            EntityService = (type, column) => // 修改列可空 1、带?问号；2、String类型若没有Required
            {
                if ((type.PropertyType.IsGenericType && type.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    || (type.PropertyType == typeof(string) && type.GetCustomAttribute<RequiredAttribute>() == null))
                {
                    column.IsNullable = true;
                }
            }
        };
        dbOptions.ConnectionConfigs?.ForEach(config =>
        {
            config.ConfigureExternalServices = configureExternalServices;
        });

        return new SqlSugarScope(dbOptions.ConnectionConfigs, db =>
        {
            dbOptions.ConnectionConfigs?.ForEach(config =>
            {
                var dbProvider = db.GetConnectionScope((string)config.ConfigId);

                // 设置超时时间
                dbProvider.Ado.CommandTimeOut = 30;

#if DEBUG
                // 打印SQL语句
                dbProvider.Aop.OnLogExecuting = (sql, pars) =>
                {

                    if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                        Console.ForegroundColor = ConsoleColor.White;
                    if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                        Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("【" + DateTime.Now + "——执行SQL】\r\n" + UtilMethods.GetSqlString(config.DbType, sql, pars) + "\r\n");
                };

                dbProvider.Aop.OnError = (ex) =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    var pars = db.Utilities.SerializeObject(((SugarParameter[])ex.Parametres).ToDictionary(it => it.ParameterName, it => it.Value));
                    Console.WriteLine("【" + DateTime.Now + "——错误SQL】\r\n" + UtilMethods.GetSqlString(config.DbType, ex.Sql, (SugarParameter[])ex.Parametres) + "\r\n");
                };
#endif

                // 数据审计
                dbProvider.Aop.DataExecuting = (oldValue, entityInfo) =>
                {
                    // 新增操作
                    if (entityInfo.OperationType == DataFilterType.InsertByObject)
                    {
                        // 主键(long类型)且没有值的---赋值雪花Id
                        if (entityInfo.EntityColumnInfo.IsPrimarykey && entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long))
                        {
                            var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                            if (id == null || (long)id == 0)
                            {
                                entityInfo.SetValue(Yitter.IdGenerator.YitIdHelper.NextId());
                            }
                        }

                        if (entityInfo.PropertyName == nameof(BaseEntity.CreateTime))
                            entityInfo.SetValue(DateTime.Now);
                    }

                    // 更新操作
                    if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                    {
                        if (entityInfo.PropertyName == nameof(BaseEntity.UpdateTime))
                            entityInfo.SetValue(DateTime.Now);
                    }
                };
            });
        });
    }
}
