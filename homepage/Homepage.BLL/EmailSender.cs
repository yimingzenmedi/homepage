using System.Net;
using System.Net.Mail;
using System.Text;
using System.Configuration;
using System;
using HomepageWeb.Models;

namespace HomepageWeb.BLL
{
    public class EmailSender: ConfigurationSection
    {
        /// <summary>
        /// send the captcha by email to the given email address
        /// </summary>
        /// <param name="captcha">captcha to send</param>
        /// <param name="type">reset or register</param>
        /// <param name="targetAddress">given email address</param>
        /// <returns>
        /// true:   success
        /// false:  failed
        /// </returns>
        public bool SendEmail(string captcha, string type, string targetAddress, string emailFrom = "zh4055526@gmail.com")
        {

            EmailSettings emailSettings = (EmailSettings)ConfigurationManager.GetSection("EmailSettings");
            string username = emailSettings.Email.UserName;
            string password = emailSettings.Email.Password;
            string host = emailSettings.Email.Host;
            int port = emailSettings.Email.Port;

            using (MailMessage myMail = new MailMessage())
            {
                myMail.From = new MailAddress(emailFrom);
                myMail.To.Add(new MailAddress(targetAddress));

                //  figure out what kind of captcha to generate
                string aim;
                if (type == "forget")
                {
                    aim = "reset your password";
                }
                else if (type == "register")
                {
                    aim = "register new account";
                }
                else
                {
                    return false;
                }

                //  make email body:
                myMail.Subject = "Homepage - Your captcha to " + aim;
                myMail.SubjectEncoding = Encoding.UTF8;

                myMail.Body = "<h1>Homepage - Your captcha to " + aim + "</h1>";
                myMail.Body += "<b style='color: #C70'>" + captcha + "</b> is your captcha. Please enter in 10 min, or you need to get a new one.";
                myMail.Body += "<h4>Have a good time!</h4>";
                myMail.BodyEncoding = Encoding.UTF8;
                myMail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = host;
                    smtp.Port = port;
                    smtp.Credentials = new NetworkCredential(username, password);
                    smtp.EnableSsl = true;      //Gmail need SSL connection

                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //Gmail need to send by network

                    smtp.Send(myMail);

                    return true;
                }
            }            
        }
    }
}
