using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace BeautySaloon.Models
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Beauty Saloon 'Nika'", "BeautycenterNika@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 25, false);
               await client.AuthenticateAsync("BeautyCenterNika@gmail.com", "beautycenterNika2020");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
        public async Task GetEmailAsync(string email, string subject, string message, string emailsend, string Name)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(Name, emailsend));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 25, false);
                await client.AuthenticateAsync("BeautyCenterNika@gmail.com", "beautycenterNika2020");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
