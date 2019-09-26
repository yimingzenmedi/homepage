namespace HomepageWeb.Models
{

    /// <summary>
    /// UserSite class - UserSites table in database
    /// </summary>
    public class UserSite
    {
        public string siteName {get;set;}
        public string siteUrl { get; set; }
        public string username { get; set; }
        public string icon { get; set; }
    }
}
