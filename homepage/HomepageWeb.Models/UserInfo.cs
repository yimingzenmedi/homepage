namespace HomepageWeb.Models
{
    /// <summary>
    /// UserInfo class - UserInfo table in database
    /// </summary>
    public class UserInfo
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string salt { get; set; }
    }
}
