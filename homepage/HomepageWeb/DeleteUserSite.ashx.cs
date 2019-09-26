using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using HomepageWeb.BLL;
namespace HomepageWeb
{
    /// <summary>
    /// DeleteUserSite:
    /// delete a site of a user from database 
    /// -1: internal error
    /// 1: nothing deleted, this site does not exist
    /// 2: not login
    /// 0: ok
    /// </summary>
    public class DeleteUserSite : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (!new LoginStateManager().CheckLoginState(context))
            {
                context.Response.Write("2");
                context.Response.End();
            }

            string siteName = context.Request.QueryString["siteName"].ToString();
            string siteUrl = context.Request.QueryString["siteUrl"].ToString();
            string username = context.Session["loginAs"].ToString();

            UserSitesManager userSitesManager = new UserSitesManager();

            context.Response.Write(userSitesManager.DeleteUserSite(username, siteName, siteUrl));
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