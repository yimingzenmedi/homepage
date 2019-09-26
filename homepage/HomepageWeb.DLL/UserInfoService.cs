using HomepageWeb.Models;
using System;
using System.Data.SqlClient;

namespace HomepageWeb.DAL
{
    /// <summary>
    /// Interact with the UserInfo table in the database
    /// </summary>
    public class UserInfoService
    {
        /// <summary>
        /// insert a new user into the database
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">md5 value of password</param>
        /// <param name="salt">random value as salt</param>
        /// <param name="email">md5 value of email</param>
        public int AddUserInfo(string username, string password, string salt, string email)
        {
            string sqlQuery = @"INSERT INTO [dbo].[UserInfo] ([Username], [Password], [Salt], [Email]) VALUES
                            (@Username, @Password, @Salt, @Email) ";

            SqlParameter[] param = 
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password),
                new SqlParameter("@Salt", salt),
                new SqlParameter("@Email", email),
            };

            return DBHelper.ExecuteNonQuery(sqlQuery, param);
        }



        /// <summary>
        /// get the object by giving username. If not exists, return null;
        /// This is for register
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="passsword">md5 value of password</param>
        /// <returns></returns>
        public UserInfo GetUserInfoByUsername(string username)
        {
            string sqlQuery = "SELECT * FROM [dbo].[UserInfo] WHERE [Username]=@username";
            SqlParameter[] param = 
            {
                new SqlParameter("@username", username),
            };
            SqlDataReader dr = DBHelper.ExecuteReader(sqlQuery, param);

            UserInfo userInfo = null;
            if (dr.Read())
            {
                userInfo = new UserInfo();
                userInfo.username = Convert.ToString(dr["Username"]);
                userInfo.password = Convert.ToString(dr["Password"]);
                userInfo.email = Convert.ToString(dr["Email"]);
                userInfo.salt = Convert.ToString(dr["Salt"]);
            }

            return userInfo;
        }


        /// <summary>
        /// update password of UserInfo in database
        /// </summary>
        /// <param name="userInfo">the password of the given object has been modified</param>
        /// <returns>number of influenced lines</returns>
        public int UpdatePasswordOfUserInfo(UserInfo userInfo)
        {
            string sqlQuery = "UPDATE [dbo].[UserInfo] SET [Password]=@password WHERE [Username]=@username and [Email]=@email";
            SqlParameter[] param = 
            {
                new SqlParameter("@password", userInfo.password),
                new SqlParameter("@username", userInfo.username),
                new SqlParameter("@email", userInfo.email),
            };
            return DBHelper.ExecuteNonQuery(sqlQuery, param);
        }



        /// <summary>
        /// NOT USEFULL - reserved for closing account
        /// 
        /// 
        /// delete provided userinfo from database
        /// </summary>
        /// <param name="userInfo">an object of userinfo, means this user have to login successfully</param>
        /// <returns></returns>
        //public void DeleteUserInfo(UserInfo userInfo)
        //{
        //    string sqlQueryForUserSites = "DELETE FROM [dbo].[UserSites] WHERE [Username]=@username";
        //    SqlParameter[] paramForUserSites = 
        //    {
        //        new SqlParameter("@Username", userInfo.username),
        //    };
        //
        //    string sqlQueryForUserInfo = "DELETE FROM [dbo].[UserInfo] WHERE [Username]=@username AND [Password]=@password AND [Salt]=@salt AND [Email]=@email";
        //    SqlParameter[] paramForUserInfo = 
        //    {
        //        new SqlParameter("@Username", userInfo.username),
        //        new SqlParameter("@Password", userInfo.password),
        //        new SqlParameter("@Salt", userInfo.salt),
        //        new SqlParameter("@Email", userInfo.email),
        //    };
        //
        //    DBHelper.ExecuteNonQuery(sqlQueryForUserSites, paramForUserSites);
        //    DBHelper.ExecuteNonQuery(sqlQueryForUserInfo, paramForUserInfo);
        //}
    }
}
