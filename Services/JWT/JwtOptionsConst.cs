namespace Hzg.Services;

/// <summary>
/// 配置路径常量
/// </summary>
public static class JwtOptionsConst
{
    /// <summary>
    /// 签发者 issuer
    /// </summary>
    public readonly static string IssuerSettingPath = "Authentication:JwtBearer:Issuer";

    /// <summary>
    /// 受众 audience
    /// </summary>
    public readonly static string AudienceSettingPath = "Authentication:JwtBearer:Audience";

    /// <summary>
    /// 有效时长小时
    /// </summary>
    public readonly static string ExpiresHourSettingPath = "Authentication:JwtBearer:ExpiresHour";

    /// <summary>
    /// 密钥路径
    /// </summary>
    public readonly static string SecurityKeySettingPath = "Authentication:JwtBearer:SecurityKey";
}