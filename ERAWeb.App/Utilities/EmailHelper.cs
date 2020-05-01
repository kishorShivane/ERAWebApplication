using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace ERAWeb.App.Utilities
{
    public class EmailHelper
    {
        public async static Task<bool> SendEmail(SmtpClient client, MailMessage mail)
        {
            var mailSent = false;
            try
            {
                await Task.Run(() => client.Send(mail));
                mailSent = true;
            }
            catch (System.Exception)
            {
                return mailSent;
            }
            return mailSent;
        }

    }
}
