namespace Hzg.Services;

public interface IVerifyCodeService
{
    /// <summary>
    /// 生成验证码并保存到 Redis
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    (bool valid, String result) GenerateEmailRegisterVerifycode(String email);

    /// <summary>
    /// 生成验证码并保存到 Redis
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public (bool valid, String result) GenerateEmailResetPasswordVerifycode(String email);
}