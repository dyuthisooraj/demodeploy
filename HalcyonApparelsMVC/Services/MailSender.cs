using FluentEmail.Core;
using HalcyonApparelsMVC.Interfaces;
using HalcyonApparelsMVC.Models;
using System.Net.Mail;
using System.Net;

namespace HalcyonApparelsMVC.Services
{
    public class MailSender : IMailSender
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        public MailSender(IServiceProvider serviceProvider, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _config = config;
        }

        private bool SendEmail(string recepientEmail)
        {
            try
            {
                string HostAdd = _config.GetSection("Gmail")["ServerName"];
                string FromEmailid = _config.GetSection("Gmail")["Sender"];
                var gmailPassword = _config.GetSection("Gmail")["Password"];
                string SMTPPort = _config.GetSection("Gmail")["Port"];

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FromEmailid);
                mailMessage.Subject = "Test Subject";
                //mailMessage.Body = "Test Message";
                mailMessage.IsBodyHtml = true;

                mailMessage.Body = System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}/wwwroot/emails/Mixed.cshtml");
                //foreach (string ToEMailId in recepientEmails)
                {
                    mailMessage.To.Add(new MailAddress(recepientEmail));
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = HostAdd;
                smtp.EnableSsl = true;
                smtp.Port = Convert.ToInt32(SMTPPort);
                smtp.Credentials = new NetworkCredential(FromEmailid, gmailPassword);
                smtp.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
     
       

        public async void SendBulkMail(IEnumerable<string> recepientEmails)
        {
            foreach (var mailid in recepientEmails)
            {
                SendEmail(mailid);

            }
        }
    }
}