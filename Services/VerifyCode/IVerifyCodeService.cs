using Hzg.Tool;
using Hzg.Consts;

namespace Hzg.Services;

public interface IVerifyCodeService
{
    /// <summary>
    /// 生成验证码并保存到 Redis
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    ResponseData<string> GenerateEmailRegisterVerifycode(String email);

    /// <summary>
    /// 生成验证码并保存到 Redis
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    ResponseData<string> GenerateEmailResetPasswordVerifycode(String email);
}