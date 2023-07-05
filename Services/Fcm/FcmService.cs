using System.Diagnostics;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Hzg.Tool;
using Hzg.Consts;
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
    /// <param name="deviceToken"></param>
    /// <param name="brand"></param>
    /// <param name="type"></param>
    /// <param name="title"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public async Task<ResponseData<string>> PushNotification(string brand, string deviceToken, NotificationType type, string title, string body)
    {
        var responseData = ResponseTool.FailedResponseData<string>();
        var message = new Message()
        {
            Notification = new Notification()
            {
                Title = title,
                Body = body
            },
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