using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Hzg.Tool;
using Hzg.Consts;
using Hzg.Dto;

namespace Hzg.Services;

public enum NotificationType: int
{
    Alert = 0,
    Sound = 1,
    Badge = 2,
    Silent = 3
}

/// <summary>
/// APNs 生成 JWT token，添加服务的时候，使用单利
/// </summary>
public class APNsService : IAPNsService
{
    static string token = null;

    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    public APNsService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        this._configuration = configuration;
        this._httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// 生成 APNs JWT token
    /// </summary>
    /// <returns></returns>
    public string GetnerateAPNsJWTToken(string brand)
    {
        return this.GetnerateAPNsJWTToken(brand, APNsService.token);
    }

    /// <summary>
    /// 生成 APNs JWT token
    /// </summary>
    /// <returns></returns>
    private string GetnerateAPNsJWTToken(string brand, string oldToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var iat = DateTimeTool.UtcNowUnixTimeSeconds();

        // 判断原 token 是否超过 50 分钟，如果未超过，直接返回
        if (string.IsNullOrWhiteSpace(oldToken) == false)
        {
            JwtPayload oldPayload = tokenHandler.ReadJwtToken(oldToken).Payload;
            var oldIat = oldPayload.Claims.FirstOrDefault(c => c.Type == "iat");
            if (oldIat != null)
            {
                if (long.TryParse(oldIat.Value, out long oldIatValue) == true)
                {
                    // 两次间隔小于 50 分钟，使用原 token
                    if ((iat - oldIatValue) < (50 * 60))
                    {
                        return oldToken;
                    }
                }
            }
        }

        var kid = _configuration["apple:" + brand + ":kid"];
        var securityKey = _configuration["apple:" + brand + ":securityKey"].Replace("\n", "");
        var iss = _configuration["apple:" + brand + ":iss"];
        
        var claims = new Claim[]
        {
            new Claim("iss", iss),
            new Claim("iat", iat.ToString())
        };

        var eCDsa = ECDsa.Create();

        eCDsa.ImportPkcs8PrivateKey(Convert.FromBase64String(securityKey), out _);

        var key = new ECDsaSecurityKey(eCDsa);

        key.KeyId = kid;

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.EcdsaSha256);
        var jwtHeader = new JwtHeader(signingCredentials);
        var jwtPayload = new JwtPayload(claims);

        var jwtSecurityToken = new JwtSecurityToken(jwtHeader, jwtPayload);

        APNsService.token = tokenHandler.WriteToken(jwtSecurityToken);

        return APNsService.token;
    }

    /// <summary>
    /// 发送推送通知
    /// </summary>
    /// <param name="apnsTopic">APP Id</param>
    /// <param name="deviceToken">设备标识</param>
    /// <param name="dto">通知数据</param>
    /// <returns></returns>
    public async Task<ResponseData<string>> PushNotification(string brand, string apnsTopic, string deviceToken, APNSNotificationDto dto)
    {
        var responseData = ResponseTool.FailedResponseData<string>();
        var token = this.GetnerateAPNsJWTToken(brand);
        var server = this._configuration["apple:" + brand + ":pushNotificationServer"];
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, server + deviceToken)
        {
            Headers = 
            {
                { HeaderNames.Authorization, "bearer " +  token },
                { "apns-topic", apnsTopic },
                { "apns-expiration", "0" }
            },
            Version = new Version(2, 0)
        };

        var notContent = new
        {
            aps = new
            {
                alert = new
                {
                    title = dto.Title,
                    subtitle = dto.Subtitle,
                    body = dto.Body
                }
            },
            data = dto.Data
        };
        var content = new StringContent(JsonSerializerTool.SerializeDefault(notContent),  System.Text.Encoding.UTF8, Application.Json);

        httpRequestMessage.Content = content;

        var httpClient = _httpClientFactory.CreateClient();
        try
        {
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                responseData.Code = ErrorCode.Success;
                return responseData;
            }
            else
            {
                responseData.Data = httpResponseMessage.StatusCode.ToString();
                return responseData;
            }
        }
        catch (Exception e)
        {
            responseData.Data = e.Message;
            return responseData;
        }
    }
}