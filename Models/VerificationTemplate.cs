namespace Hzg.Models;

public class VerificationTemplate {
    /**
     * 验证码邮件模板主题
     */
    public String Subject { get; set; }

    /**
     * 验证码邮件模板内容格式
     */
    public String Content { get; set; }
}