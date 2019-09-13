using HomepageWeb.BLL;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace HomepageWeb
{
    /// <summary>
    /// Register handler
    /// </summary>
    public class Register : IHttpHandler, IRequiresSessionState
    {

        /// <summary>
        /// Register processor
        /// </summary>
        /// <param name="context"></param>
        /// <return>
        /// -1: invalid username formate;
        /// -2: invalid password formate;
        /// -3: invalid repeat formate;
        /// -4: invalid email formate;
        /// -5: invalid captcha;
        /// 0: ok; 
        /// 1: username existed;
        /// </return>

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string username = context.Request.Form["username"];
            string password = context.Request.Form["password"];
            string repeat = context.Request.Form["repeat"];
            string email = context.Request.Form["email"];
            string captcha = context.Request.Form["captcha"];

            // check the sent values 

            if (username == null || username.Trim() == string.Empty || username.Length > 50)
            {
                context.Response.Write("-1");
                context.Response.End();
            }
            if (password == null || password.Trim() == string.Empty || password.Length < 6 || password.Length > 20)
            {
                context.Response.Write("-2");
                context.Response.End();
            }
            if (repeat == null || repeat.Trim() == string.Empty || password.Length < 6 || password.Length > 20 || !repeat.Equals(password))
            {
                context.Response.Write("-3");
                context.Response.End();
            }
            if (email == null || email.Trim() == string.Empty || !Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                context.Response.Write("-4");
                context.Response.End();
            }
            if (captcha == null || captcha.Trim() == string.Empty || !captcha.Equals(context.Session["registerCaptcha"]))
            {
                context.Response.Write("-5");
                context.Response.End();
            }

            UserInfoManager userInfoManager = new UserInfoManager();
            
            // Check if the username exists
            if (userInfoManager.CheckUsernameExists(username)) 
            {
                context.Response.Write("1");
                context.Response.End();
            }

            // unique username:         yes
            // password length ok:      yes
            // email valid:             yes
            // captcha compare pass:    yes
            // ready to save:           yes

            if (userInfoManager.Register(username, password, email))
            {
                (new UserSitesManager()).AddDefaultSites(username);
                context.Response.Write("0");
                context.Session.Remove("registerCaptcha");
            }
            else
            {
                context.Response.Write("1");        // others may register this name before you
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