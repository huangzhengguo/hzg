namespace Hzg.Models;

/// <summary>
/// 邮件属性
/// </summary>
public class MailProperties
{
    /**
     * 公司ID
     */
    public String CorpId { get; set; }

    /**
     * 公司名称
     */
    public String CorpName { get; set; }

    /**
     * 邮箱服务器
     */
    public String Host { get; set; }

    /**
     * 端口
     */
    public int Port { get; set; }

    /**
     * 编码
     */
    public String Encoding { get; set; }

    /**
     * 协议
     */
    public String Protocol { get; set; }

    public Dictionary<object, object> Properties { get; set; }

    /**
     * 发送邮箱
     */
    public String UserName { get; set; }

    /**
     * 邮箱密码
     */
    public String Password { get; set; }

    /**
     * 验证码注册邮件模板
     */
    public VerificationTemplate VerificationRegistrationTemplate { get; set; }

    /**
     * 验证码重置密码邮件模板
     */
    public VerificationTemplate VerificationResetTemplate { get; set; }
}