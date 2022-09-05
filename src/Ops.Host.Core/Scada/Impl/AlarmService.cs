namespace Ops.Host.Core.Services;

internal sealed class AlarmService : IAlarmService
{
    public Task HandleAsync(ForwardData data)
    {
        if (data.Values.Length == 0)
        {
            return Task.CompletedTask;
        }

        var alarmValues = data.Values[0].GetBitArray(); // 警报数据
        try
        {
            for (int i = 0; i < alarmValues!.Length; i++)
            {
                if (alarmValues[i])
                {
                    // DoSomething...
                }
            }
        }
        catch (Exception)
        {
            
        }

        return Task.CompletedTask;
    }
}
