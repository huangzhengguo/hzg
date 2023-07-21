using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Hzg.Models;

namespace Hzg.Services;

/// <summary>
/// 邮件服务
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="mailProperties"></param>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public bool SendMail(MailProperties mailProperties, String to, String subject, String content);
}