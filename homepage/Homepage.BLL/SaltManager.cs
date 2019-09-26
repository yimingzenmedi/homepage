using System;
using System.Security.Cryptography;

namespace HomepageWeb.BLL
{
    public class SaltManager
    {
        /// <summary>
        /// generate salt - 25 bit long
        /// </summary>
        /// <return>return the salt</return>
        public string GenerateSalt()
        {
            //char[] s = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm', 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };
            string s = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNMM";
            string result = "";
            int targetLength = 25;
            using (RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider())
            {
                while (result.Length != targetLength)
                {
                    byte[] oneByte = new byte[1];
                    rngCrypto.GetBytes(oneByte);
                    char character = (char)oneByte[0];
                    if (s.Contains(character.ToString()))
                    {
                        result += character;
                    }
                } 
            }

            return result;
        }

    }
}
