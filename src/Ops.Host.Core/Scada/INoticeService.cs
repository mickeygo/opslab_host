﻿namespace Ops.Host.Core.Services;

public interface INoticeService
{
    Task HandleAsync(ForwardData data);
}
