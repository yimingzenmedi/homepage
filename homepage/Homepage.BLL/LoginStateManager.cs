using System.Web;
using System.Web.SessionState;

namespace HomepageWeb.BLL
{
    public class LoginStateManager: IRequiresSessionState
    {
        /// <summary>
        /// check login state
        /// Check if the values of loginAs in session and cookie are the same
        /// </summary>
        /// <param name="context"></param>
        /// <returns>
        /// true: logged in 
        /// false: not logged in
        /// </returns>
        public bool CheckLoginState(HttpContext context)
        {
            if (context.Session["loginAs"] == null)
            {
                return false;
            }
            if ( context.Request.Cookies["loginAs"] == null)
            {
                return false;
            }

            string sessionValue = context.Session["loginAs"].ToString();
            string cookieValue = context.Request.Cookies["loginAs"].Value.ToString();

            if (sessionValue.Equals(cookieValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
