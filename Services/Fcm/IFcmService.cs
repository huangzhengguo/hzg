using Hzg.Tool;

namespace Hzg.Services;

public interface IFcmService
{
    /// <summary>
    /// 发送通知
    /// </summary>
    /// <param name="deviceToken"></param>
    /// <param name="brand"></param>
    /// <param name="type"></param>
    /// <param name="title"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    Task<ResponseData<string>> PushNotification(string brand, string deviceToken, NotificationType type, string title, string body);
}