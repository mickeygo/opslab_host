namespace Ops.Host.Core.Services;

internal sealed class AlarmService : ScadaDomainService, IAlarmService
{
    private readonly SqlSugarRepository<SysDictData> _dictDataRep;
    private readonly SqlSugarRepository<PtAlarmRecord> _alarmRecordRep;
    private readonly ILogger _logger;

    public AlarmService(SqlSugarRepository<SysDictData> dictDataRep, 
        SqlSugarRepository<PtAlarmRecord> alarmRecordRep,
        ILogger<AlarmService> logger)
    {
        _dictDataRep = dictDataRep;
        _alarmRecordRep = alarmRecordRep;
        _logger = logger;
    }

    public async Task HandleAsync(ForwardData data)
    {
        if (data.Values.Length == 0)
        {
            return;
        }

        // 从字典中查找对应的警报信息，若字典中没有设置，不会存储对应警报信息。
        var dicts = await _dictDataRep.GetListAsync(s => s.Code == DictCodeEnum.Alarm.ToString());
        if (!dicts.Any())
        {
            return;
        }

        var alarmValues = data.Values[0].GetBitArray(); // 警报数据
        try
        {
            var alarmRecords = new List<PtAlarmRecord>();
            for (int i = 0; i < alarmValues!.Length; i++)
            {
                if (alarmValues[i])
                {
                    var alarmDict = dicts.FirstOrDefault(s => s.Value == (i + 1).ToString()); // 字典基数为 1.
                    if (alarmDict != null)
                    {
                        var alarm = new PtAlarmRecord
                        {
                            LineCode = data.Schema.Line,
                            StationCode = data.Schema.Station,
                            Descirption = alarmDict.Name,
                        };
                        alarmRecords.Add(alarm);

                        // 推送消息
                        // await MessageTaskQueueManager.Default.QueueAsync(new Message(alarm.LineCode, alarm.StationCode, MessageClassify.Alarm, alarm.Descirption ?? ""));
                    }
                }
            }

            if (alarmRecords.Any())
            {
                await _alarmRecordRep.InsertRangeAsync(alarmRecords);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[AlarmService] 存储警报信息异常");
        }
    }
}
