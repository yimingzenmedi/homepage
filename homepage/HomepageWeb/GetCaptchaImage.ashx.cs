using System.Web;
using System.Web.SessionState;

using HomepageWeb.BLL;


namespace HomepageWeb
{
    /// <summary>
    /// GetCaptchaImage
    /// Get captcha in image - for login
    /// </summary>
    public class GetCaptchaImage : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/Png";
            CaptchaTextProvider captchaTextProvider = new CaptchaTextProvider();
            string captchaText = captchaTextProvider.GenerateCaptchaText(5);
            context.Session.Timeout = 10;
            context.Session["loginCaptcha"] = captchaText;
            context.Response.BinaryWrite(new CaptchaImageProvider().DrawCaptchaImage(captchaText));
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