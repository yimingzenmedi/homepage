using System.Web;
using System.Web.SessionState;

using HomepageWeb.BLL;

namespace HomepageWeb
{
    /// <summary>
    /// GetCaptchaEmail
    /// Get captcha by email - for register and reset password
    /// The captcha email can be sent for registering or resetting the password 
    /// based on the type value passed.
    /// </summary>
    /// <return>
    /// 0: success
    /// 1: failed
    /// </return>
    public class GetCaptchaEmail : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string email = context.Request.QueryString["email"];
            string type = context.Request.QueryString["type"];
            string captchaText = new CaptchaTextProvider().GenerateCaptchaText(5);

            if (new EmailSender().SendEmail(captchaText, type, email))
            {
                context.Response.Write("0");
                context.Session.Timeout = 10;
                context.Session[type + "Captcha"] = captchaText;
            }
            else
            {
                context.Response.Write("1");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}