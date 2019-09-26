using System.Web;
using HomepageWeb.BLL;

namespace HomepageWeb
{
    /// <summary>
    /// Check if this username have existed
    /// </summary>
    public class CheckExist : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            UserInfoManager userInfoManager = new UserInfoManager();
            string username = context.Request.Form["username"];
            if (userInfoManager.CheckUsernameExists(username))
            {
                context.Response.Write("1");
            }
            else
            {
                context.Response.Write("0");
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