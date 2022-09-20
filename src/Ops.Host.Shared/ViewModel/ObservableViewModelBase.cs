using HandyControl.Controls;
using HandyControl.Data;

namespace Ops.Host.Shared.ViewModel;

/// <summary>
/// 基于 ObservableObject 的 ViewModel 抽象类。
/// </summary>
public abstract class ObservableViewModelBase : ObservableObject
{
    /// <summary>
    /// 通知成功消息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="waitTime"></param>
    protected void NoticeSuccess(string? message, int waitTime = 1)
    {
        Growl.Success(new GrowlInfo
        {
            Message = message ?? "成功",
            WaitTime = waitTime,
        });
    }

    /// <summary>
    /// 通知消息
    /// </summary>
    /// <param name="message">消息内容</param>
    /// <param name="waitTime">延迟关闭时间，默认1s</param>
    protected void NoticeInfo(string? message, int waitTime = 1)
    {
        Growl.Info(new GrowlInfo
        {
            Message = message ?? "消息",
            WaitTime = waitTime,
        });
    }

    /// <summary>
    /// 通知警告消息
    /// </summary>
    /// <param name="message">消息内容</param>
    /// <param name="waitTime">延迟关闭时间，默认3s</param>
    protected void NoticeWarning(string? message, int waitTime = 3)
    {
        Growl.Warning(new GrowlInfo
        {
            Message = message ?? "警告" ,
            WaitTime = waitTime,
        });
    }

    /// <summary>
    /// 通知错误消息
    /// </summary>
    /// <param name="message">消息内容</param>
    /// <param name="waitTime">延迟关闭时间，小于 0 时不设置关闭时间，默认 5s。</param>
    protected void NoticeError(string? message, int waitTime = 5)
    {
        if (waitTime <= 0)
        {
            Growl.Error(message ?? "错误");
        }
        else
        {
            Growl.Error(new GrowlInfo
            {
                Message = message ?? "错误",
                WaitTime = waitTime,
                StaysOpen = false,
            });
        }
    }

    protected void NoticeAsk(string? message, Action action)
    {
        Growl.Ask(message, isConfirm =>
        {
            if (isConfirm)
            {
                action();
            }

            return true;
        });
    }
}
