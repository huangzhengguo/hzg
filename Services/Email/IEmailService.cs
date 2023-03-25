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
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public bool SendMail(MailProperties mailProperties, String from, String to, String subject, String content);
    
    /// <summary>
    /// 异步发送邮件
    /// </summary>
    /// <param name="email"></param>
    /// <param name="body"></param>
    public void SendEmailAsync(string email, string body, Action<bool> callback);
}