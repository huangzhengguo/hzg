using System.Text;
using System.Net;
using System.Net.Mail;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace Hzg.Services;

/// <summary>
/// 邮件服务
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    // SMTP 服务器
    private string _host {
        get {
            return _configuration["email:host"];
        }
    }
    // 端口
    private int _port {
        get {
            return Convert.ToInt32(_configuration["email:port"]);
        }
    }
    // 发件人邮箱密码
    private string _password {
        get {
            return _configuration["email:password"];
        }
    }

    // 发件人邮箱
    private string _fromEmail {
        get {
            return _configuration["email:email"];
        }
    }

    // 邮件主题
    private string _subject {
        get {
            return _configuration["email:subject"];
        }
    }

    // 显示名称
    private string _displayName {
        get {
            return _configuration["email:displayName"];
        }
    }

    private Action<bool> SendEmailCallback = null;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="email">邮箱</param>
    /// <param name="body">邮件内容</param>
    public void SendEmail(string email, string body)
    {
        // 邮件消息信息
        var mailMessage = GetMailMessage(email, body);

        var smtp = GetSmtpClient();

        smtp.Send(mailMessage);
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public bool SendMail(SmtpClient sender, String from, String to, String subject, String content)
    {
        var mailMessage = new MailMessage();

        mailMessage.From = new MailAddress(from, from, Encoding.UTF8);
        mailMessage.To.Add(new MailAddress(to));
        mailMessage.SubjectEncoding = Encoding.UTF8;
        mailMessage.Subject = subject;
        mailMessage.BodyEncoding = Encoding.UTF8;
        mailMessage.Body = content;

        sender.Send(mailMessage);

        return true;
    }

    /// <summary>
    /// 异步发送邮件
    /// </summary>
    /// <param name="email"></param>
    /// <param name="body"></param>
    public void SendEmailAsync(string email, string body, Action<bool> callback)
    {
        this.SendEmailCallback = callback;

        var mailMessage = GetMailMessage(email, body);
        var smtpClient = GetSmtpClient();

        smtpClient.SendCompleted += SendCompletedCallback;

        smtpClient.SendAsync(mailMessage, "Test");
    }

    /// <summary>
    /// 异步发送邮件回调
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        var mailSent = false;
        string token = (string) e.UserState;
        if (e.Cancelled)
        {
            return;
        }

        if (e.Error != null)
        {
            mailSent = false;
        }
        else
        {
            mailSent = true;

        }

        if (this.SendEmailCallback != null)
        {
            this.SendEmailCallback(mailSent);
        }
    }

    /// <summary>
    /// 获取默认 SMTP 客户端
    /// </summary>
    /// <returns></returns>
    private SmtpClient GetSmtpClient()
    {
        var smtp = new SmtpClient();

        smtp.Host = this._host;
        smtp.Port = this._port;
        smtp.Credentials = new NetworkCredential(this._fromEmail, this._password);

        return smtp;
    }

    /// <summary>
    /// 获取邮件消息
    /// </summary>
    /// <param name="email"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    private MailMessage GetMailMessage(string email, string body)
    {
        var mailMessage = new MailMessage();

        mailMessage.From = new MailAddress(this._fromEmail, this._displayName, Encoding.UTF8);
        mailMessage.To.Add(new MailAddress(email));
        mailMessage.SubjectEncoding = Encoding.UTF8;
        mailMessage.Subject = this._subject;
        mailMessage.BodyEncoding = Encoding.UTF8;
        mailMessage.Body = body;

        return mailMessage;
    }
}