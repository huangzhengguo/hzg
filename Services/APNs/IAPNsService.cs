using Hzg.Tool;

namespace Hzg.Services;

public interface IAPNsService
{
    /// <summary>
    /// 生成 APNs JWT token
    /// </summary>
    /// <returns></returns>
    string GetnerateAPNsJWTToken();

    /// <summary>
    /// 发送推送通知
    /// </summary>
    /// <param name="apnsTopic">APP Id</param>
    /// <param name="deviceToken">设备标识</param>
    /// <param name="type">通知类型</param>
    /// <param name="title">标题</param>
    /// <param name="subtitle">子标题</param>
    /// <param name="body">通知内容</param>
    /// <returns></returns>
    Task<ResponseData<string>> PushNotification(string apnsTopic, string deviceToken, NotificationType type, string title, string subtitle, string body);
}