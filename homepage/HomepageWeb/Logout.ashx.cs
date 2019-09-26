using System;
using System.Web;
using System.Web.SessionState;

namespace HomepageWeb
{
    /// <summary>
    /// Logout
    /// clear session and cookie
    /// </summary> 
    public class Logout : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Session["loginAs"] = null;
            context.Response.Cookies["loginAs"].Expires = DateTime.Now.AddDays(-2);
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