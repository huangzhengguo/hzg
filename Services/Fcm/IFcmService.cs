using Hzg.Tool;
using Hzg.Dto;

namespace Hzg.Services;

/// <summary>
/// FCM 通知
/// </summary>
public interface IFcmService
{
    /// <summary>
    /// 发送通知
    /// </summary>
    /// <param name="deviceToken">设备标识</param>
    /// <param name="brand">品牌</param>
    /// <param name="dto">通知数据</param>
    /// <returns></returns>
    Task<ResponseData<string>> PushNotification(string brand, string deviceToken, FCMNotificationDto dto);
}