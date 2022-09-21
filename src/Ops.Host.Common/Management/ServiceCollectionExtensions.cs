using MediatR;

namespace Ops.Host.Common;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册基础组件。
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddHostCommon(this IServiceCollection services)
    {
        // LiteDB
        services.AddSingleton<ILiteDBMananger, LiteDBMananger>();

        return services;
    }

    /// <summary>
    /// 注册 Mediator。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddHostMediatR(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddSingleton<IMediator, Mediator>();
        services.AddMediatR(assemblies);

        return services;
    }
}
