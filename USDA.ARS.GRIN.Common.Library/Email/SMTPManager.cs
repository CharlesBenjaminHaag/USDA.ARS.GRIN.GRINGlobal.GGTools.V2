using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace USDA.ARS.GRIN.Common.Library.Email
{
    public class SMTPManager
    {
        protected static string SMTP_SERVER = "mailproxy1.usda.gov";

        public bool SendMessage(SMTPMailMessage sMTPMailMessage)
        {
            string[] recipientList;
            MailMessage mailMessage = new MailMessage();

            try
            {
                mailMessage.From = new MailAddress("noreply@usda.gov");
                mailMessage.ReplyToList.Add(new MailAddress("gringlobal-feedback@usda.gov"));
                recipientList = sMTPMailMessage.To.Split(';');

                // If running in test mode, add only test email address(es) to
                // list.
                string runInTestMode = ConfigurationManager.AppSettings["RunInTestMode"];

                if (runInTestMode == "Y")
                {
                    string testModeRecipientEmailAddress = ConfigurationManager.AppSettings["TestModeRecipientEmailAddress"];
                    mailMessage.To.Add(testModeRecipientEmailAddress);
                }
                else
                {
                    foreach (var recipient in recipientList)
                    {
                        if (!String.IsNullOrEmpty(recipient))
                        {
                            mailMessage.To.Add(recipient);
                        }
                    }
                }
                mailMessage.Subject = sMTPMailMessage.Subject;
                mailMessage.Body = sMTPMailMessage.Body;

                if (!String.IsNullOrEmpty(sMTPMailMessage.CC))
                {
                    mailMessage.Bcc.Add(sMTPMailMessage.CC);
                }

                mailMessage.IsBodyHtml = true;
                SmtpClient client = new SmtpClient(SMTP_SERVER);
                client.Send(mailMessage);
            }
            catch (SmtpException smex)
            {
                throw smex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
