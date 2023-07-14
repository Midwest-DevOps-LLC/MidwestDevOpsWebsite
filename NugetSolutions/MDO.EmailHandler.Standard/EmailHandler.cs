using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace MDO.EmailHandler.Standard
{
    public static class EmailHandler
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public enum EmailAddresses
        {
            alerts,
            deals,
            receipt,
            webmaster,
            noreply
        }

        private static string GetEmailFromEnum(EmailAddresses emailAddresses)
        {
            var ret = "";

            switch (emailAddresses)
            {
                case EmailAddresses.alerts:
                    return "alerts";
                case EmailAddresses.deals:
                    return "deals";
                case EmailAddresses.receipt:
                    return "receipt";
                case EmailAddresses.webmaster:
                    return "webmaster";
                case EmailAddresses.noreply:
                    return "noreply";
            }

            return ret;
        }

        public static bool SendEmail(string fromAddress, EmailAddresses fromAddressName, string toAddress, string fromPassword, string subject, string body, bool isHtml)
        {
            try
            {
                //string fromAddresss = "mail.midwestdevops@gmail.com";
                //var toAddresss = new MailAddress(toAddress);


                var smtpClient = new SmtpClient("smtp-relay.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromAddress, fromPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(GetEmailFromEnum(fromAddressName) + "@midwestdevops.com", GetEmailFromEnum(fromAddressName)),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml,
                };


                mailMessage.To.Add(toAddress);

                smtpClient.Send(mailMessage);




                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress(fromAddresss);
                //    mail.To.Add(toAddress);
                //    mail.Subject = subject;
                //    mail.Body = body;
                //    mail.IsBodyHtml = isHtml;
                //    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                //    {
                //        smtp.UseDefaultCredentials = false;
                //        smtp.Credentials = new NetworkCredential(fromAddresss, fromPassword);
                //        smtp.EnableSsl = true;
                //        smtp.Send(mail);
                //    }
                //}

                return true;
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        string error = "";
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                        }

                        MDO.Utility.Standard.LogHandler.SaveLog(new MDO.Utility.Standard.LogHandler.Log() { text = "Recieved bad code from email api", exception = new Exception(error) });
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
                Console.WriteLine(e.Message);
                throw;
            }

            return false;
        }
    }
}
