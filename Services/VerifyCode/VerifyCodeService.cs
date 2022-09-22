using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Hzg.Const;
using Hzg.Tool;

using System.Diagnostics;

namespace Hzg.Services;

public class VerifyCodeService : IVerifyCodeService
{
    private int EMAIL_CODE_INTERVAL = 300;

    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public VerifyCodeService(IConfiguration configuration, IEmailService emailService)
    {
        _configuration = configuration;
        _emailService = emailService;
    }

    /// <summary>
    /// 生成验证码并保存到 Redis
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public (bool valid, String result) GenerateEmailRegisterVerifycode(String email)
    {
        return GenerateEmailVerifycode(CommonConstant.EMAIL_REGISTER_CODE_KEY, email);
    }

    /// <summary>
    /// 生成验证码并保存到 Redis
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public (bool valid, String result) GenerateEmailResetPasswordVerifycode(String email)
    {
        return GenerateEmailVerifycode(CommonConstant.EMAIL_RESETPSW_CODE_KEY, email);
    }

    /// <summary>
    /// 生成验证码
    /// </summary>
    /// <param name="typePrefix"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    private (bool valid, String result) GenerateEmailVerifycode(string typePrefix, string email)
    {
        // 验证邮箱格式
        if (EmailTool.ValidateEmail(email) == false)
        {
            return (false, "The mailbox format is invalid");
        }

        // 存储到 Redis
        String key = typePrefix + email;
        string exitCode = RedisTool.GetStringValue(key);
        if (string.IsNullOrWhiteSpace(exitCode) == false)
        {
            long leftSeconds = RedisTool.GetRemainingSeconds(key);
            long usedSeconds = CommonConstant.CODE_TIME - leftSeconds;

            if (usedSeconds < EMAIL_CODE_INTERVAL)
            {
                return (false, "Verification codes are sent too frequency! Please wait for " + (EMAIL_CODE_INTERVAL - usedSeconds) + " Seconds.");
            }
        }

        String code = RandomTool.GenerateDigitalCode();

        RedisTool.SetStringValue(key, code, CommonConstant.CODE_TIME);

        return (true, code);
    }

}