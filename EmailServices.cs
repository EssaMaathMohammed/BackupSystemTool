using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystemTool
{
    internal class EmailServices
    {
        // send email method
        public void SendEmail(string receiver, string subject, string body)
        {
            var fromAddress = new MailAddress("mysqlbackupsystem@gmail.com", "MysqlBackupSystem");
            var toAddress = new MailAddress(receiver, "Customer");
            const string fromPassword = "ckvhqqhomwrcssjj";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
