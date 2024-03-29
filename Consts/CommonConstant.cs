namespace Hzg.Consts;

/// <summary>
/// 常用常量
/// </summary>
public static class CommonConstant
{
    /// <summary>
    /// 注册验证码前缀
    /// </summary>
    static public String EMAIL_REGISTER_CODE_KEY = "EMAIL_REGISTER_CODE:";

    /// <summary>
    /// 注册验证码前缀
    /// </summary>
    static public String EMAIL_RESETPSW_CODE_KEY = "EMAIL_RESETPSW_CODE:";
    
    /// <summary>
    /// 验证码过期时间，单位秒
    /// </summary>
    static public long CODE_TIME = 3600;

    /// <summary>
    /// Redis 服务器
    /// </summary>
    public static string RedisServer = "localhost";
}