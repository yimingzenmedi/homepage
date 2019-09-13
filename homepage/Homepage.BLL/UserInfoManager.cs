using HomepageWeb.DAL;
using HomepageWeb.Models;

namespace HomepageWeb.BLL
{
    public class UserInfoManager
    {
        readonly UserInfoService userInfoService = new UserInfoService();


        /// <summary>
        /// to register.
        /// password and emal have not been encrypted.
        /// salt is generated and added here
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="rawPassword">unencrypted password</param>
        /// <param name="rawEmail">unencrypted email</param>
        /// <returns>false - failed; true - successful</returns>
        public bool Register(string username, string rawPassword, string rawEmail)
        {
            if (CheckUsernameExists(username))
            {
                return false;
            }
            else
            {
                string salt = (new SaltManager()).GenerateSalt();
                HashManager hashManager = new HashManager();
                string password = hashManager.HashWithSalt(salt, rawPassword);
                string email = hashManager.HashWithSalt(salt, rawEmail);
                userInfoService.AddUserInfo(username, password, salt, email);
                return true;
            }
        }


        /// <summary>
        /// to login
        /// password has not been encrypted.
        /// username has been checked, safe to use.
        /// </summary>
        /// <param name="username">checked username</param>
        /// <param name="rawPassword">unencrypted password</param>
        /// <returns>
        /// true: login successful
        /// false: login failed
        /// </returns>
        public bool Login(string username, string rawPassword)
        {
            UserInfo userInfo = userInfoService.GetUserInfoByUsername(username);
            if (userInfo == (null))
            {
                return false;
            }

            string salt = userInfo.salt;
            string encryptedPassword = new HashManager().HashWithSalt(salt, rawPassword);
            if (encryptedPassword.Equals(userInfo.password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// to check if the given username has been in database
        /// </summary>
        /// <param name="username">given username</param>
        /// <returns>if in: true; else false</returns>
        public bool CheckUsernameExists(string username)
        {
            UserInfo existedUserInfo = userInfoService.GetUserInfoByUsername(username);
            if (existedUserInfo == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// to check if the given email matchs the saved email.
        /// email has not been encrypted
        /// username has been checked, exists.
        /// </summary>
        /// <param name="username">existed username</param>
        /// <param name="rawEmail">encrypted email</param>
        /// <returns>if matched, return true; else false</returns>
        public bool MatchEmail(string username, string rawEmail)
        {
            UserInfo existedUserInfo = userInfoService.GetUserInfoByUsername(username);

            string salt = existedUserInfo.salt;
            string email = existedUserInfo.email;

            string encryptedEmail = new HashManager().HashWithSalt(salt, rawEmail);

            return encryptedEmail.Equals(email);
        }


        /// <summary>
        /// to reset password
        /// username has been checked, exists.
        /// password has not been encrypted.
        /// </summary>
        /// <param name="username">existed username</param>
        /// <param name="newRawPassword">new password that has not been encrypted</param>
        /// <returns>if 1 row is influenced: true; else: false </returns>
        public bool ResetPassword(string username, string newRawPassword)
        {
            UserInfo existedUserInfo = userInfoService.GetUserInfoByUsername(username);

            string salt = existedUserInfo.salt;

            string encryptedNewPassword = new HashManager().HashWithSalt(salt, newRawPassword);
            existedUserInfo.password = encryptedNewPassword;

            return userInfoService.UpdatePasswordOfUserInfo(existedUserInfo) == 1;
        }
    }
}
