using System;
using System.Text;
using System.Security.Cryptography;

namespace HomepageWeb.BLL
{
    public class HashManager
    {

        /// <summary>
        /// get the md5 value with salt in 8 times loop
        /// </summary>
        /// <param name="salt">givin salt</param>
        /// <param name="data">data to be encrypted</param>
        /// <returns>md5 value with salt</returns>
        public string HashWithSalt(string salt, string data)
        {
            if (salt == null || salt.Trim() == string.Empty)
            {
                throw new Exception("Empty salt!");
            }

            if (data == null || data.Trim() == string.Empty)
            {
                throw new Exception("Empty data!");
            }

            int times = 8;
            string result = data;

            for (int i = 0; i < times; i++ )
            {
                string dataWithSalt;
                if (i % 2 == 0)
                {
                    dataWithSalt = result + salt;
                }
                else
                {
                    dataWithSalt = salt + result;
                }
                using (var md5 = MD5.Create())
                {
                    result = MD5Encrypt(dataWithSalt);
                }
            }

            return result;
        }


        /// <summary>
        /// get the encrypted value using md5
        /// </summary>
        /// <param name="data">data to be encryoted</param>
        /// <returns>md5 value</returns>
        private string MD5Encrypt(string data)
        {
            using (MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider())
            {
                byte[] hashedDataBytes;
                hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(data));
                StringBuilder tmp = new StringBuilder();
                foreach (byte i in hashedDataBytes)
                {
                    tmp.Append(i.ToString("x2"));
                }
                return tmp.ToString(); 
            }
        }        
    }
}
