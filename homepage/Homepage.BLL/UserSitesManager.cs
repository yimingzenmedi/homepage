using HomepageWeb.DAL;
using HomepageWeb.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace HomepageWeb.BLL
{
    public class UserSitesManager : ConfigurationSection
    {
        /// <summary>
        /// Add a siteInfo to database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="siteName"></param>
        /// <param name="siteUrl"></param>
        /// <returns>
        /// 0: ok
        /// -1: internal error
        /// 1: nothing added
        /// </returns>
        public int AddUserSite(string username, string siteName, string siteUrl)
        {
            if (siteName == null || siteName.Trim() == string.Empty)
            {
                return -1;
            }
            if (siteUrl == null || siteUrl.Trim() == string.Empty)
            {
                return -1;
            }

            UserSite newUserSite = new UserSite
            {
                username = username,
                siteName = siteName,
                siteUrl = siteUrl
            };

            UserSiteService userSiteService = new UserSiteService();

            //check if this site has been saved to this user:
            UserSite savedUserSite = userSiteService.GetUserSitesByUsernameSitenameSiteurl(username, siteName, siteUrl);
            if (savedUserSite != null)
            {
                return 1;
            }
            //  add to database
            int result = userSiteService.AddUserSite(newUserSite);
            if (result == 1)
            {
                return 0;
            }
            else if (result == 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }


        /// <summary>
        /// Delete a siteInfo from database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="siteName"></param>
        /// <param name="siteUrl"></param>
        /// <returns>
        /// 0: ok
        /// -1: internal error
        /// 1: nothing deleted
        /// </returns>
        public int DeleteUserSite(string username, string siteName, string siteUrl)
        {
            if (siteName == null || siteName.Trim() == string.Empty)
            {
                return -1;
            }
            if (siteUrl == null || siteUrl.Trim() == string.Empty)
            {
                return -1;
            }

            UserSite userSiteToDelete = new UserSite
            {
                username = username,
                siteName = siteName,
                siteUrl = siteUrl
            };

            UserSiteService userSiteService = new UserSiteService();

            //  delete from database
            int result = userSiteService.DeleteUserSite(userSiteToDelete);
            if (result == 1)
            {
                return 0;
            }
            else if (result == 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }


        /// <summary>
        /// Get all userSites by username
        /// Read all the userSites in an IEnumerable (List),
        /// turn this IEnumerable (List) into a json string
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>
        /// json STRING of the list
        /// </returns>
        public string GetUserSitesJsonStringByUsername(string username)
        {
            UserSiteService userSiteService = new UserSiteService();
            IEnumerable<UserSite> userSitesList = userSiteService.GetUserSitesByUsername(username);
            JsonSerializer jsonSerializer = new JsonSerializer();
            StringWriter stringWriter = new StringWriter();
            jsonSerializer.Serialize(new JsonTextWriter(stringWriter), userSitesList);
            string result = stringWriter.GetStringBuilder().ToString();
            return result;
        }


        /// <summary>
        /// add default sites to database of this username
        /// used for new registered users
        /// read web.config to get the sites
        /// Settings for this part is defined in Web.config/DefaultSites - a custom section
        /// </summary>
        /// <param name="username"></param>
        /// <returns>number of sites that is added successfully</returns>
        public int AddDefaultSites(string username)
        {
            int result = 0;
            UserSiteService userSiteService = new UserSiteService();

            DefaultSites defaultSites = (DefaultSites)ConfigurationManager.GetSection("DefaultSites");
            foreach (DefaultSiteSetting site in defaultSites.KeyValues)
            {
                UserSite defaultUserSite = new UserSite
                {
                    username = username,
                    siteName = site.Key,
                    siteUrl = site.Value,
                };
                result += userSiteService.AddUserSite(defaultUserSite);
            }

            return result;
        }






        /// <summary>
        /// NOT USEFULL - this function can be used to add all sites of a user to database
        /// 
        /// accept a username and a JSON string,
        /// turn the JSON string into JSON objects,
        /// loop every JSON objects and add them into database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="sitesString"></param>
        /// <returns></returns>
        //public int AddUserSites(string username, string sitesString)
        //{
        //    JObject jObj = (JObject)JsonConvert.DeserializeObject(sitesString);
        //    UserSiteService userSiteService = new UserSiteService();

        //    try
        //    {
        //        foreach (var item in jObj)
        //        {
        //            string siteName = item.Key.ToString();
        //            string siteUrl = item.Value.ToString();

        //            UserSite newUserSite = new UserSite();
        //            newUserSite.username = username;
        //            newUserSite.siteName = siteName;
        //            newUserSite.siteUrl = siteUrl;

        //            if (siteName == null || siteName.Trim() == string.Empty)
        //            {
        //                return 1;
        //            }
        //            if (siteUrl == null || siteUrl.Trim() == string.Empty)
        //            {
        //                return 1;
        //            }

        //            userSiteService.AddUserSite(newUserSite);
        //        }

        //        return 0;
        //    }
        //    catch
        //    {
        //        return 1;
        //    }

        //}
    }
}
