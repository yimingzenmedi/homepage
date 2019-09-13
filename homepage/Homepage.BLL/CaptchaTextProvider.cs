using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomepageWeb.BLL
{
    /// <summary>
    /// provide captcha text
    /// </summary>
    public class CaptchaTextProvider
    {
        /// <summary>
        /// generate captcha text
        /// </summary>
        /// <param name="length">length of the captcha to be generated</param>
        /// <returns>genetated captcha text</returns>
        public string GenerateCaptchaText(int length)
        {
            char[] s = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
            string captchaText = String.Empty;
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                captchaText += s[rd.Next(s.Length - 1)].ToString();
            }
            return captchaText;
        }
    }
}
