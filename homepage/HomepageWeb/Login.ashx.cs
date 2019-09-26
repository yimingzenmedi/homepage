using System.Web;
using System.Web.SessionState;
using HomepageWeb.BLL;
using System;

namespace HomepageWeb
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// Login processor
        /// </summary>
        /// <param name="context"></param>
        /// <return>
        /// code:
        ///     -1: invalid username formate;
        ///     -2: invalid password formate;
        ///     -3: invalid captcha;
        ///     1: incorrect username or password;
        ///     0: ok; 
        /// dataset:
        ///     in json, saved user sites
        ///     only return when code == 0
        /// </return>

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string username = context.Request.Form["username"];
            string password = context.Request.Form["password"];
            string captcha = context.Request.Form["captcha"].ToLower();

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
            if (captcha == null || captcha.Trim() == string.Empty || !captcha.Equals(context.Session["loginCaptcha"]))
            {
                context.Response.Write("-3");
                context.Response.End();
            }

            UserInfoManager userInfoManager = new UserInfoManager();
            if (userInfoManager.Login(username, password))
            {
                context.Response.Write("0");
                context.Session.Timeout = 60 * 3;
                context.Session["loginAs"] = username;

                HttpCookie cookie = new HttpCookie("loginAs");
                cookie.Expires = DateTime.Now.AddHours(3);
                cookie.Value = username;
                context.Response.AppendCookie(cookie);
                context.Session.Remove("loginCaptcha");
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