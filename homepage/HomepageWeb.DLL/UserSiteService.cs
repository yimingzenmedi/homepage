using HomepageWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HomepageWeb.DAL
{
    /// <summary>
    /// Interact with the UserSites table in the database
    /// </summary>
    public class UserSiteService
    {
        /// <summary>
        /// give a UserSite object and save to database
        /// </summary>
        /// <param name="userSite">a UserSite object</param>
        /// <returns>number of influenced row</returns>
        public int AddUserSite(UserSite userSite) 
        {
            string sqlQuery = @"INSERT INTO [dbo].[UserSites] ([Username], [Sitename], [Siteurl]) VALUES
                            (@username, @siteName, @siteUrl) ";

            SqlParameter[] param = 
            {
                new SqlParameter("@Username", userSite.username),
                new SqlParameter("@siteName", userSite.siteName),
                new SqlParameter("@siteUrl", userSite.siteUrl),
            };

            return DBHelper.ExecuteNonQuery(sqlQuery, param);
        }


        /// <summary>
        /// get all the UserSite objects belongs to the given username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>a List<UserSite> containing all the sites of this user</returns>
        public IEnumerable<UserSite> GetUserSitesByUsername(string username) 
        {
            string sqlQuery = "SELECT * FROM [dbo].[UserSites] WHERE [Username]=@username";
            SqlParameter[] param = 
            {
                new SqlParameter("@username", username),
            };
            DataTable dt = DBHelper.ExecuteDataTable(sqlQuery, param);
            List<UserSite> list = new List<UserSite>();
            foreach (DataRow row in dt.Rows)
            {
                UserSite userSite = new UserSite();
                userSite.username = (string)row["Username"];
                userSite.siteName = (string)row["Sitename"];
                userSite.siteUrl = (string)row["Siteurl"];
                list.Add(userSite);
            }
            return list;
        }

        /// <summary>
        /// get the UserSite objects with the given username, sitename and siteurl
        /// </summary>
        /// <param name="username"></param>
        /// <param name="siteName"></param>   
        /// <param name="siteUrl"></param>
        /// <returns>a UserSite object</returns>
        public UserSite GetUserSitesByUsernameSitenameSiteurl(string username, string siteName, string siteUrl)
        {
            string sqlQuery = "SELECT * FROM [dbo].[UserSites] WHERE [Username]=@username AND [Sitename]=@siteName AND [Siteurl]=@siteUrl";
            SqlParameter[] param =
            {
                new SqlParameter("@username", username),
                new SqlParameter("@siteName", siteName),
                new SqlParameter("@siteUrl", siteUrl),
            };
            SqlDataReader dr = DBHelper.ExecuteReader(sqlQuery, param);

            UserSite userSite = null;
            if (dr.Read())
            {
                userSite = new UserSite
                {
                    username = Convert.ToString(dr["Username"]),
                    siteName = Convert.ToString(dr["Sitename"]),
                    siteUrl = Convert.ToString(dr["Siteurl"])
                };
            }
            return userSite;
        }

        /// <summary>
        /// delete the given UserSite from database
        /// </summary>
        /// <param name="userSite"></param>
        /// <returns>number of influenced row</returns>
        public int DeleteUserSite(UserSite userSite) 
        {
            string sqlQuery = "DELETE FROM [dbo].[UserSites] WHERE [Username]=@username AND [Sitename]=@sitename AND [Siteurl]=@siteurl";
            SqlParameter[] param =
            {
                new SqlParameter("@username", userSite.username),
                new SqlParameter("@sitename", userSite.siteName),
                new SqlParameter("@siteurl", userSite.siteUrl),
            };
            return DBHelper.ExecuteNonQuery(sqlQuery, param);
        }
    }
}
