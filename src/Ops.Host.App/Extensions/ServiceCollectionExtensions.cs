using Ops.Host.App.Forwarders;
using Ops.Host.App.ViewModels;

namespace Ops.Host.App.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHostApp(this IServiceCollection services)
    {
        // forwarders
        services.AddScoped<INoticeForwarder, OpsLocalNoticeForwarder>();
        services.AddScoped<IReplyForwarder, OpsLocalReplyForwarder>();
        services.AddScoped<IUnderlyForwarder, OpsLocalUnderlyForwarder>();

        // 注册 Host.Core 服务。
        services.AddHostCore();

        // 注册 ViewModels
        services.AddViewModel();

        return services;
    }

    private static IServiceCollection AddViewModel(this IServiceCollection services)
    {
        var types = typeof(IViewModel).Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && typeof(IViewModel).IsAssignableFrom(t));
        foreach (var type in types)
        {
            services.AddTransient(type);
        }

        return services;
    }
}
