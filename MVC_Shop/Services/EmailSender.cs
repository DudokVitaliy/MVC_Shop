using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace MVC_Shop.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string host = "smtp.gmail.com";
            int port = 587;
            string emailAddress = "dudok405@gmail.com";
            string emailPassword = "ccvv jkuv mucj lnmt";
            SmtpClient client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(emailAddress, emailPassword),
                EnableSsl = true
            };

            MailMessage massage = new MailMessage();
            massage.From = new MailAddress(emailAddress);
            massage.To.Add(email);
            massage.Subject = subject;
            massage.Body = htmlMessage;
            massage.IsBodyHtml = true;

            await client.SendMailAsync(massage);
        }
    }
}
