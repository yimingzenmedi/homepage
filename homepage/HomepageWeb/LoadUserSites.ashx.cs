using HomepageWeb.BLL;
using System.Web;
using System.Web.SessionState;

namespace HomepageWeb
{
    /// <summary>
    /// LoadUserSites
    /// Load saved sites of this user when login
    /// </summary>
    public class LoadUserSites : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <return>
        /// 2: not login
        /// json string: saved data in json STRING, not object
        /// </return>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            //check login state:
            if (!new LoginStateManager().CheckLoginState(context))
            {
                context.Response.Write("2");
                context.Response.End();
            }

            string username = context.Session["loginAs"].ToString();
            UserSitesManager userSitesManager = new UserSitesManager();
            string userSitesListString = userSitesManager.GetUserSitesJsonStringByUsername(username);

            context.Response.Write(userSitesListString);

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