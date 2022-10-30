using System.Diagnostics;
using FirebaseAdmin.Messaging;
using Hzg.Tool;
using Hzg.Const;

namespace Hzg.Services;

public class FcmService : IFcmService
{
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
    public async Task<ResponseData<string>> PushNotification(string deviceToken, NotificationType type, string title, string subtitle, string body)
    {
        var responseData = ResponseTool.FailedResponseData<string>();
        var message = new Message()
        {
            Data = new Dictionary<string, string>()
            {
                { "score", "900" },
                { "time", "03:00" }
            },
            Token = deviceToken
        };

        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        Debug.WriteLine("Fcm 发送消息返回:" + response);

        responseData.Code = ErrorCode.Success;

        return responseData;
    }
}