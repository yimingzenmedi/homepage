using HomepageWeb.BLL;
using System.Web;
using System.Web.SessionState;

namespace HomepageWeb
{
    /// <summary>
    /// AddUserSite:
    /// add a new site to database for a user
    /// -1: internal error
    /// 1: nothing added, this site has existed
    /// 2: not login
    /// 0: ok
    /// </summary>
    public class AddUserSite : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //  check login state:
            if (!new LoginStateManager().CheckLoginState(context))
            {
                context.Response.Write("2");
                context.Response.End();
            }
            string siteName = context.Request.Form["siteName"].ToString();
            string siteUrl = context.Request.Form["siteUrl"].ToString();
            string username = context.Session["loginAs"].ToString();

            UserSitesManager userSitesManager = new UserSitesManager();

            context.Response.Write(userSitesManager.AddUserSite(username, siteName, siteUrl));
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