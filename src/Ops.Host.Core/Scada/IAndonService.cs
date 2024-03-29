﻿namespace Ops.Host.Core.Services;

/// <summary>
/// SCADA 安灯信息服务。
/// </summary>
public interface IAndonService : IDomainService
{
    Task HandleAsync(ForwardData data);
}
