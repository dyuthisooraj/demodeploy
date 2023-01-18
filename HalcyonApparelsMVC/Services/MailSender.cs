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

        //private bool SendEmail(string recepientEmail)
         private bool SendEmail(MarketingList mlist)
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
                
                mailMessage.IsBodyHtml = true;

                if (mlist.Product_Type__c == "shirt")
                {
                    mailMessage.Body = System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}/wwwroot/emails/Mixed.cshtml");
                    //mailMessage.Body = System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}/wwwroot/watches/Shoes.cshtml");
                   
                    {
                        mailMessage.To.Add(new MailAddress(mlist.Email));
                    }

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = HostAdd;
                    smtp.EnableSsl = true;
                    smtp.Port = Convert.ToInt32(SMTPPort);
                    smtp.Credentials = new NetworkCredential(FromEmailid, gmailPassword);
                    smtp.Send(mailMessage);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
     
       

        //public async void SendBulkMail(IEnumerable<string> recepientEmails)
        public async void SendBulkMail(List<MarketingList> mlist)
        {
            foreach (var mailid in mlist)
            {
                SendEmail(mailid);

            }
            //SendEmail(mlist);

        }
    }
}