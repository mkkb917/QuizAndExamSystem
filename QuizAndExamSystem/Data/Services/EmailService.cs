using ExamSystem.Data.Interface;
using System.Net.Mail;

namespace ExamSystem.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmailAddress;

        public EmailService(string smtpServer, int smtpPort, string fromEmailAddress)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _fromEmailAddress = fromEmailAddress;
        }
        public Task SendEmailAsync(string userEmail, string subject, string htmlMessage)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(_fromEmailAddress),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(userEmail));
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Send(message);
            };
            return Task.CompletedTask;
        }
        //public bool SendEmailPasswordReset(string userEmail, string link)
        //{
        //    MailMessage mailMessage = new MailMessage()
        //    {
        //        From = new MailAddress(_fromEmailAddress),
        //        Subject = "Password Reset",
        //        Body = link,
        //        IsBodyHtml = true
        //    };
        //    mailMessage.To.Add(new MailAddress(userEmail));

        //    using (var client = new SmtpClient(_smtpServer, _smtpPort))
        //    {
        //        client.Credentials = new System.Net.NetworkCredential("804b4a498b67d7e41496e084c56d1639", "6d7446bde04fbd5550e8d825626a7683");
        //        client.EnableSsl = true;
        //        client.Send(mailMessage);
        //        return true;
        //    };

        //SmtpClient client = new SmtpClient();

        //client.Credentials = new System.Net.NetworkCredential("mkkb917@yahoo.com", "c5fj!B6mA@r");
        //client.Credentials = new System.Net.NetworkCredential("kamran.9002@gmail.com", "orvnqqlaakmugjug");
        //client.Host = "smtp.gmail.com";
        //client.Port = 587;    // google 587  , 465
        //client.EnableSsl = true;

        //// mailjet api configurations
        //client.Credentials = new System.Net.NetworkCredential("804b4a498b67d7e41496e084c56d1639", "6d7446bde04fbd5550e8d825626a7683");
        //client.Host = "in-v3.mailjet.com";
        //client.EnableSsl = true;
        //client.Port = 587;


        //// Ethereal api configurations (temporary email account for smtp)
        //client.Credentials = new System.Net.NetworkCredential("rachelle.jerde52@ethereal.email", "bVWmz5nbn5MGFsSvyV");
        //client.Host = "smtp.ethereal.email";
        //client.EnableSsl = true;
        //client.Port = 587;



        //return false;
        //}
    }
}
