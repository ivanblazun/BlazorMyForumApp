using System.Net.Mail;
using FirstBlazorApp.Models;
using ForumAdminPanel.Models;
using System.Net.Mail;

namespace FirstBlazorApp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        

        public void SendRegisterMail(RegisterEmail regMail)
        {
            string emailUser = _configuration.GetValue<string>("EmailUser");
            string emailPassword = _configuration.GetValue<string>("EmailPassword");

            string subject = regMail.subject;
            string body = regMail.body;
            string to = regMail.to;

            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(emailUser);
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(emailUser, emailPassword);
            smtp.Send(mailMessage);

            //return Ok();
        }
    }
}
