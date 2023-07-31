using System.Text;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Hzg.Models;
using Microsoft.Extensions.Logging;
using Hzg.Consts;

namespace Hzg.Services;

/// <summary>
/// 邮件服务
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    private Action<bool> SendEmailCallback = null;

    public EmailService(IConfiguration configuration, ILogger<string> logger)
    {
        _configuration = configuration;
        _logger = logger;
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
    public bool SendMail(MailProperties mailProperties, String to, String subject, String content)
    {
        if (mailProperties.Protocol == "SMTP")
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(mailProperties.Email, mailProperties.Email));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = content
            };

            using(var client = new SmtpClient())
            {
                if (mailProperties.EncryptionMode == EncryptionMode.None)
                {
                    client.Connect(mailProperties.Host, mailProperties.Port, useSsl: false);
                }
                else if (mailProperties.EncryptionMode == EncryptionMode.SSL)
                {
                    client.Connect(mailProperties.Host, mailProperties.Port, useSsl: true);
                }
                else if (mailProperties.EncryptionMode == EncryptionMode.STARTTLS)
                {
                    client.Connect(mailProperties.Host, mailProperties.Port, options: MailKit.Security.SecureSocketOptions.StartTls);
                }
                
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(mailProperties.UserName, mailProperties.Password);

                client.Send(message);
                client.Disconnect(true);
            }
        }
        else if (mailProperties.Protocol == "Exchange")
        {
            // ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);

            // service.Credentials = new WebCredentials(mailProperties.Email, mailProperties.Password);

            // if (mailProperties.Properties.Keys.Contains("mail.smtp.ssl.enable") == true && mailProperties.Properties["mail.smtp.ssl.enable"].ToString() == "true")
            // {
            //     service.Url = new Uri("https://" + mailProperties.Host + ":" + mailProperties.Port);
            // }
            
            // EmailMessage email = new EmailMessage(service);
            // email.Subject = subject;
            // email.Body = new MessageBody(content);
            // email.ToRecipients.Add(to);
            // email.Send();
        }

        return true;
    }
}