using System;
using System.Configuration;


/// <summary>
/// Classes for email account config: EmailSettings.
/// This config is to define settings of the email account 
/// used for sending captcha emails to users.
/// Settings can be changed in Web.config/EmailSettings/email. 
/// 
/// username - your username to login to your email (usually your email address)
/// password - your password to login to your email
/// host     - smtp server address of your email service provider (check from your email service provider)
/// port     - port of the smtp server (check from your email service provider)
/// </summary>
namespace HomepageWeb.Models
{

    /// <summary>
    /// node of <EmailSettings/>
    /// </summary>
    public class EmailSettings : ConfigurationSection
    {
        [ConfigurationProperty("email", IsRequired = true)]
        public EmailElement Email
        {
            get { return (EmailElement)this["email"]; }
        }
    }

    /// <summary>
    /// <email/> element. Settings are defined here.
    /// </summary>
    public class EmailElement : ConfigurationElement
    {
        [ConfigurationProperty("username", IsRequired = true)]
        public string UserName
        {
            get { return this["username"].ToString(); }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return this["password"].ToString(); }
        }

        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return this["host"].ToString(); }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return Convert.ToInt32(this["port"].ToString()); }
        }
    }
}
