using Ops.Host.Agent.ViewModels;

namespace Ops.Host.Agent.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHostApp(this IServiceCollection services)
    {
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
