using Microsoft.Extensions.Configuration;
using Hzg.Consts;
using Hzg.Tool;

namespace Hzg.Services;

public class VerifyCodeService : IVerifyCodeService
{
    private int EMAIL_CODE_INTERVAL = 300;

    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IRedisService _redisService;

    public VerifyCodeService(IConfiguration configuration, IEmailService emailService, IRedisService redisService)
    {
        _configuration = configuration;
        _emailService = emailService;
        _redisService = redisService;
    }

    /// <summary>
    /// 生成验证码并保存到 Redis
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public ResponseData<string> GenerateEmailRegisterVerifycode(String email)
    {
        return GenerateEmailVerifycode(CommonConstant.EMAIL_REGISTER_CODE_KEY, email);
    }

    /// <summary>
    /// 生成验证码并保存到 Redis
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public ResponseData<string> GenerateEmailResetPasswordVerifycode(String email)
    {
        return GenerateEmailVerifycode(CommonConstant.EMAIL_RESETPSW_CODE_KEY, email);
    }

    /// <summary>
    /// 生成验证码
    /// </summary>
    /// <param name="typePrefix"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    private ResponseData<string> GenerateEmailVerifycode(string typePrefix, string email)
    {
        var responseData = ResponseTool.FailedResponseData<string>();
        // 验证邮箱格式
        if (EmailTool.ValidateEmail(email) == false)
        {
            responseData.Code = ErrorCode.Email_Format_Error;
        }

        // 存储到 Redis
        String key = typePrefix + email;
        string exitCode = _redisService.GetStringValue(key);
        if (string.IsNullOrWhiteSpace(exitCode) == false)
        {
            long leftSeconds = _redisService.GetRemainingSeconds(key);
            long usedSeconds = CommonConstant.CODE_TIME - leftSeconds;

            if (usedSeconds < EMAIL_CODE_INTERVAL)
            {
                responseData.Message = string.Format(ErrorCodeTool.GetErrorMessage(ErrorCode.Verify_Code_Too_Frequency_Tepmlete), EMAIL_CODE_INTERVAL - usedSeconds);

                return responseData;
            }
        }

        String code = RandomTool.GenerateDigitalCode();

        _redisService.SetStringValue(key, code, CommonConstant.CODE_TIME);

        responseData.Data = code;
        responseData.Code = ErrorCode.Success;

        return responseData;
    }

}