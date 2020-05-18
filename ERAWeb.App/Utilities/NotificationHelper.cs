using ERAWeb.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ERAWeb.App.Utilities
{
    public class NotificationHelper
    {

        public async static Task<bool> SendEmailNotification(UserModel user, string email, string name, string message, IConfiguration config)
        {
            var mailSent = false;
            var fromEmail = config.GetValue<string>("AppSettings:ContactUsEmailFrom");
            var toEmail = config.GetValue<string>("AppSettings:ContactUsEmailTo");
            var emailSubject = config.GetValue<string>("AppSettings:ContactUsEmailSubject");
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

            var mail = new MailMessage(new MailAddress(fromEmail), new MailAddress(toEmail))
            {
                Subject = emailSubject,
                Body = GetBodyContent(user, email, name, message),
                IsBodyHtml = true,
            };
            mailSent = await EmailHelper.SendEmail(client, mail);
            return mailSent;
        }

        public async static Task<bool> SendRegisterNotification(string userEmail, string userName, string activationURL, IConfiguration config)
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


        private static string GetBodyContent(UserModel user, string email, string name, string message)
        {
            var body = "<html> <head> <style> table, td{ border: 1px solid lightGray; } table { border-collapse: collapse; width: 100%; } </style> </head> <body> <h2>ERA Contact US Form Details</h2> <hr> <br> <h3>Details of user</h3> <table style='width:50%'> <tr> <td width='50%'><b>First Name</b></td> <td>#FN#</td> </tr> <tr> <td width='50%'><b>Last Name</b></td> <td>#LN#</td> </tr> <tr> <td width='50%'><b>Company Name</b></td> <td>#CN#</td> </tr> <tr> <td width='50%'><b>Employee Number</b></td> <td>#EN#</td> </tr> <tr> <td width='50%'><b>Email</b></td> <td>#E#</td> </tr> </table> <br> <hr> <br> <h3>Submitted Details</h3> <table style='width:50%'> <tr> <td width='50%'><b>Name</b></td> <td>#SN#</td> </tr> <tr> <td width='50%'><b>Email</b></td> <td>#SE#</td> </tr> <tr> <td width='50%'><b>Submitted Date</b></td> <td>#DATE#</td> </tr> <tr> <td width='50%'><b>Message</b></td> <td style='padding: 15px;text-align: justify;'>#MSG#</td> </tr> </table> </body> </html>";
            body = body.Replace("#FN#", user.FirstName);
            body = body.Replace("#LN#", user.LastName);
            body = body.Replace("#CN#", user.CompanyName);
            body = body.Replace("#EN#", user.EmployeeNumber);
            body = body.Replace("#E#", user.Email);
            body = body.Replace("#SN#", name);
            body = body.Replace("#SE#", email);
            body = body.Replace("#DATE#", DateTime.Now.ToString("D"));
            body = body.Replace("#MSG#", message);
            return body;
        }
    }
}
