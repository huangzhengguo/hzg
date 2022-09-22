namespace Hzg.Dto;

public class EnterpriseDto
{
    /// <summary>
    /// 企业标识
    /// </summary>
    /// <value></value>
    public Guid Id { get; set; }

    /// <summary>
    /// 企业 Id
    /// </summary>
    /// <value></value>
    public string CorpId { get; set; }

    /// <summary>
    /// 企业名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 邮箱host
    /// </summary>
    public string MailHost { get; set; }

    /// <summary>
    /// 邮箱port
    /// </summary>
    public string MailPort { get; set; }

    /// <summary>
    /// 邮箱encoding
    /// </summary>
    public string MailEncoding { get; set; }

    /// <summary>
    /// 邮箱protocol
    /// </summary>
    public string MailProtocol { get; set; }

    /// <summary>
    /// 邮箱username
    /// </summary>
    public string MailUsername { get; set; }

    /// <summary>
    /// 邮箱password
    /// </summary>
    public string MailPassword { get; set; }

    /// <summary>
    /// 邮箱properties_ssl_enable
    /// </summary>
    public string MailPropertiesSslEnable { get; set; }

    /// <summary>
    /// 邮箱注册标题
    /// </summary>
    public string MailRegistrationSubject { get; set; }

    /// <summary>
    /// 邮箱注册内容
    /// </summary>
    public string MailRegistrationContent { get; set; }

    /// <summary>
    /// 邮箱重置标题
    /// </summary>
    public string MailResetSubject { get; set; }

    /// <summary>
    /// 邮箱重置内容
    /// </summary>
    public string MailResetContent { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; } 
}