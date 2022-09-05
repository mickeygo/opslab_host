using Ops.Host.Core.Services;

namespace Ops.Host.Core.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加领域服务对象
    /// </summary>

    public static IServiceCollection AddHostAppServices(this IServiceCollection services)
    {
        // 添加 SCADA 服务
        services.AddSingleton<IAlarmService, AlarmService>();
        services.AddSingleton<IAndonService, AndonService>();
        services.AddSingleton<INoticeService, NoticeService>();
        services.AddSingleton<IInboundService, InboundService>();
        services.AddSingleton<IArchiveService, ArchiveService>();
        services.AddSingleton<IMaterialService, MaterialService>();
        services.AddSingleton<ICustomService, CustomService>();

        // 添加自定义服务
        var types = typeof(IAlarmService).Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && typeof(IDomainService).IsAssignableFrom(t));
        foreach (var type in types)
        {
            var interfaceType = type.GetInterfaces().FirstOrDefault(t => typeof(IDomainService).IsAssignableFrom(t) && t != typeof(IDomainService));
            if (interfaceType != null)
            {
                services.AddTransient(interfaceType, type);
            }
        }

        return services;
    }
}
