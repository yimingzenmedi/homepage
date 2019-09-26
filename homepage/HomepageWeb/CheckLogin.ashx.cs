using System.Web;
using System.Web.SessionState;
using HomepageWeb.BLL;
using System;

namespace HomepageWeb
{
    /// <summary>
    /// CheckLogin:    /// 
    /// Check the session and cookie, get the "loginAs" of both
    /// 
    /// If the values of both loginAs are the same, return 0
    /// If not, return 1
    /// </summary>
    public class CheckLogin : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (new LoginStateManager().CheckLoginState(context))
            {
                context.Response.Write("0");
            }
            else
            {
                context.Response.Write("1");
                context.Session["loginAs"] = null;
                context.Response.Cookies["loginAs"].Expires = DateTime.Now.AddDays(-2);
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