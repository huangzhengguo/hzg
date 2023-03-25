using System.Text;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Hzg.Models;

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
    /// <param name="mailProperties"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public bool SendMail(MailProperties mailProperties, String from, String to, String subject, String content)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(from, from));
        message.To.Add(new MailboxAddress(to, to));
        message.Subject = subject;
        message.Body = new TextPart("plain")
        {
            Text = content
        };

        using(var client = new SmtpClient())
        {
            var useSsl = false;
            if (mailProperties.Properties.Keys.Contains("mail.smtp.ssl.enable") == true && mailProperties.Properties["mail.smtp.ssl.enable"].ToString() == "true")
            {
                useSsl = true;
            }
            client.Connect(mailProperties.Host, mailProperties.Port, useSsl);

            // Note: only needed if the SMTP server requires authentication
            client.Authenticate (mailProperties.Email, mailProperties.Password);

            client.Send(message);
            client.Disconnect(true);
        }

        return true;
    }

    /// <summary>
    /// 异步发送邮件
    /// </summary>
    /// <param name="email"></param>
    /// <param name="body"></param>
    public void SendEmailAsync(string email, string body, Action<bool> callback)
    {
        // this.SendEmailCallback = callback;

        // var mailMessage = GetMailMessage(email, body);
        // var smtpClient = GetSmtpClient();

        // smtpClient.SendCompleted += SendCompletedCallback;

        // smtpClient.SendAsync(mailMessage, "Test");
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
    /// 获取邮件消息
    /// </summary>
    /// <param name="email"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    private MimeMessage GetMailMessage(string email, string body)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(this._displayName, this._fromEmail));
        message.To.Add(new MailboxAddress(email, email));
        message.Subject = this._subject;
        message.Body = new TextPart("plain")
        {
            Text = body
        };

        return message;
    }
}