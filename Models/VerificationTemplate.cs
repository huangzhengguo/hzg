namespace Hzg.Models;

public class VerificationTemplate
{
    /// <summary>
    /// 验证码邮件模板主题
    /// </summary>
    /// <value></value>
    public String Subject { get; set; }

    /// <summary>
    /// 验证码邮件模板内容格式
    /// </summary>
    /// <value></value>
    public String Content { get; set; }
}