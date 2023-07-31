using Hzg.Tool;
using Hzg.Dto;

namespace Hzg.Services;

public interface IAPNsService
{
    /// <summary>
    /// 生成 APNs JWT token
    /// </summary>
    /// <param name="brand">品牌</param>
    /// <returns></returns>
    string GetnerateAPNsJWTToken(string brand);

    /// <summary>
    /// 发送推送通知
    /// </summary>
    /// <param name="apnsTopic">APP Id</param>
    /// <param name="deviceToken">设备标识</param>
    /// <param name="dto">通知数据</param>
    /// <returns></returns>
    Task<ResponseData<string>> PushNotification(string brand, string apnsTopic, string deviceToken, APNSNotificationDto dto);
}