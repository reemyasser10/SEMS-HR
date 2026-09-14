using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace Utilities.EmailServices
{
    public class EmailSender 
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmail(EmailMessage message)
        {
            MimeMessage mailMessage = CreateEmailMessage(message);
            await Send(mailMessage);
        }

        public async Task SendHtmlEmail(EmailMessage message)
        {
            MimeMessage mailMessage = CreateHtmlEmailMessage(message);
            await Send(mailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            MimeMessage emailMessage = new();

            emailMessage.From.Add(new MailboxAddress(_emailConfig.FromName ?? _emailConfig.From, _emailConfig.From));

            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        private MimeMessage CreateHtmlEmailMessage(EmailMessage message)
        {
            MimeMessage emailMessage = new();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.FromName ?? _emailConfig.From, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            string text = message.HtmlContent.Replace("{0}", message.Content);

            if (message.OtherContent != null && message.OtherContent.Any())
            {
                int index = 1;
                foreach (string content in message.OtherContent)
                {
                    text = text.Replace($"{{{index}}}", content);
                    index++;
                }
            }

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = text
            };
            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            using SmtpClient client = new();
            try
            {
#if DEBUG
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
#endif
                
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                _ = client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                await client.SendAsync(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception, or both.
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }

}
