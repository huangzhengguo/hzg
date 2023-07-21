namespace Hzg.Models;
using Hzg.Consts;

/// <summary>
/// 邮件属性
/// </summary>
public class MailProperties
{
    /// <summary>
    /// 品牌
    /// </summary>
    /// <value></value>
    public String Brand { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    /// <value></value>
    public String CorpName { get; set; }

    /// <summary>
    /// 邮箱服务器
    /// </summary>
    /// <value></value>
    public String Host { get; set; }

    /// <summary>
    /// 邮箱服务器端口
    /// </summary>
    /// <value></value>
    public int Port { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    /// <value></value>
    public String Encoding { get; set; }

    /// <summary>
    /// 协议
    /// </summary>
    /// <value></value>
    public String Protocol { get; set; }

    /// <summary>
    /// 加密方式
    /// </summary>
    public EncryptionMode EncryptionMode { get; set; }

    /// <summary>
    /// 属性
    /// </summary>
    /// <value></value>
    public Dictionary<object, object> Properties { get; set; }

    /// <summary>
    /// 发件箱
    /// </summary>
    /// <value></value>
    public String Email { get; set; }

    /// <summary>
    /// 认证用户名
    /// </summary>
    /// <value></value>
    public String UserName { get; set; }

    /// <summary>
    /// 发件箱密码
    /// </summary>
    /// <value></value>
    public String Password { get; set; }

    /// <summary>
    /// 验证码注册邮件模板
    /// </summary>
    /// <value></value>
    public VerificationTemplate VerificationRegistrationTemplate { get; set; }

    /// <summary>
    /// 验证码重置密码邮件模板
    /// </summary>
    /// <value></value>
    public VerificationTemplate VerificationResetTemplate { get; set; }
}