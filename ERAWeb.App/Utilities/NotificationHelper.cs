using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ERAWeb.App.Utilities
{
    public class NotificationHelper
    {

        //private async void SendEmailNotification(string userEmail, string userName, int userID)
        //{
        //    using (var smtpClient = HttpContext.RequestServices.GetRequiredService<SmtpClient>())
        //    {
        //        var fromEmail = config.GetValue<string>("AppSettings:ActivationEmailFrom");
        //        var emailSubject = config.GetValue<string>("AppSettings:ActivationEmailSubject");
        //        var actionURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/{"Registration"}/{"Activate"}?{"UserID="}{userID}";

        //        var cts = new CancellationTokenSource();
        //        cts.CancelAfter(TimeSpan.FromSeconds(2));

        //        await smtpClient.SendMailAsync(new MailMessage(from: fromEmail,
        //               to: userEmail,
        //               subject: emailSubject,
        //               body: "<a href='" + actionURL + "'>Click to Activate Account</a>"
        //               ));

        //    }
        //}

        public async static Task<bool> SendRegisterNotification(string userEmail, string userName, string activationURL,IConfiguration config)
        {
            var mailSent = false;
            var fromEmail = config.GetValue<string>("AppSettings:ActivationEmailFrom");
            var emailSubject = config.GetValue<string>("AppSettings:ActivationEmailSubject");
            var host = config.GetValue<String>("Email:Smtp:Host");
            var port = config.GetValue<int>("Email:Smtp:Port");
            var credentials = new NetworkCredential(config.GetValue<String>("Email:Smtp:Username"), config.GetValue<String>("Email:Smtp:Password"));

            var client = new SmtpClient()
            {
                Port = port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = host,
                EnableSsl = true,
                Credentials = credentials
            };

            var mail = new MailMessage(new MailAddress(fromEmail), new MailAddress(userEmail))
            {
                Subject = emailSubject,
                Body = "<a href='" + activationURL + "'>Click to Activate Account</a>",
                IsBodyHtml = true,
            };
            mailSent = await EmailHelper.SendEmail(client, mail);
            return mailSent;
        }

    }
}
