﻿namespace Ops.Host.App.Forwarders;

/// <summary>
/// 本地处理请求/响应事件。
/// </summary>
internal sealed class OpsLocalReplyForwarder : IReplyForwarder
{
    private readonly IInboundService _inboundService;
    private readonly IArchiveService _archiveService;
    private readonly IMaterialService _materialService;
    private readonly ICustomService _customService;
    private readonly ILogger _logger;

    public OpsLocalReplyForwarder(
        IInboundService inboundService,
        IArchiveService archiveService,
        IMaterialService materialService,
        ICustomService customService,
        ILogger<OpsLocalReplyForwarder> logger)
    {
        _inboundService = inboundService;
        _archiveService = archiveService;
        _materialService = materialService;
        _customService = customService;
        _logger = logger;
    }

    public async Task<ReplyResult> ExecuteAsync(ForwardData data, CancellationToken cancellationToken = default)
    {
        try
        {
            return data.Tag switch
            {
                PlcSymbolTag.PLC_Sign_Inbound => await _inboundService.HandleAsync(data),
                PlcSymbolTag.PLC_Sign_Archive => await _archiveService.HandleAsync(data),
                PlcSymbolTag.PLC_Sign_Critical_Material => await _materialService.HandleCriticalMaterialAsync(data),
                PlcSymbolTag.PLC_Sign_Batch_Material => await _materialService.HandleBactchMaterialAsync(data),
                _ => await _customService.HandleAsync(data),
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[OpsReplyForwarder] Handle Error");

            return new()
            {
                Result = ExStatusCode.HandlerException,
            };
        }
    }
}
