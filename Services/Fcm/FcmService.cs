using System.Diagnostics;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Hzg.Tool;
using Hzg.Consts;
using Hzg.Dto;
using Microsoft.Extensions.Configuration;

namespace Hzg.Services;

public class FcmService : IFcmService
{
    private readonly IConfiguration _configuration;
    public FcmService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 发送通知
    /// </summary>
    /// <param name="deviceToken">设备标识</param>
    /// <param name="brand">品牌</param>
    /// <param name="dto">通知数据</param>
    /// <returns></returns>
    public async Task<ResponseData<string>> PushNotification(string brand, string deviceToken, FCMNotificationDto dto)
    {
        var responseData = ResponseTool.FailedResponseData<string>();
        var message = new Message()
        {
            Notification = new Notification()
            {
                Title = dto.Title,
                Body = dto.Body
            },
            Data = dto.Data,
            Token = deviceToken
        };

        var lowerBrand = brand.ToLower();

        var firebaseApp = FirebaseAdmin.FirebaseApp.GetInstance(lowerBrand);
        if (firebaseApp == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(_configuration["google:" + lowerBrand + ":secretKeyPath"])
            }, lowerBrand);
        }

        string response = await FirebaseMessaging.GetMessaging(firebaseApp).SendAsync(message);

        responseData.Code = Hzg.Consts.ErrorCode.Success;
        responseData.Data = response;

        return responseData;
    }
}