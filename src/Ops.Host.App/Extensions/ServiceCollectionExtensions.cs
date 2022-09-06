﻿using Ops.Host.App.Forwarders;
using Ops.Host.App.ViewModels;
using Yitter.IdGenerator;

namespace Ops.Host.App.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHostApp(this IServiceCollection services)
    {
        // forwarders
        services.AddSingleton<INoticeForwarder, OpsLocalNoticeForwarder>();
        services.AddSingleton<IReplyForwarder, OpsLocalReplyForwarder>();

        // services
        services.AddSqlSugarSetup();
        services.AddHostAppServices();

        // viewmodels
        services.AddViewModel();

        // 设置雪花Id算法机器码
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions(5));

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
