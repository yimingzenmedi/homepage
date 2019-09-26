using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using HomepageWeb.BLL;

namespace HomepageWeb
{
    /// <summary>
    /// UploadCustomBGImage 的摘要说明
    /// </summary>
    public class UploadCustomBGImage : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <return>
        /// -1: invalid params
        /// 0: ok
        /// 1: nothing changed
        /// 2: not login
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

            string siteName = context.Request.Form["siteName"];
            string iconBase64 = context.Request.Form["iconBase64"];
            string userName = context.Session["loginAs"].ToString();

            if (siteName == null || siteName.Trim() == string.Empty)
            {
                context.Response.Write("-1");
                context.Response.End();
            }
            if (iconBase64 == null || iconBase64.Trim() == string.Empty)
            {
                context.Response.Write("-1");
                context.Response.End();
            }

            UserSitesManager userSitesManager = new UserSitesManager();
            int result = userSitesManager.SetCustomBGImage(iconBase64, siteName, userName);
            if (result == 0)
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