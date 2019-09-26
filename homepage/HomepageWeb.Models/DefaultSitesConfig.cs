using System;
using System.Configuration;

/// <summary>
/// Classes for custom config: DefaultSites.
/// This config is to define default sites added to a new registered user.
/// Settings can be changed in Web.config/DefaultSites 
/// by editing, adding or removeing <add/> tags.
/// </summary>
namespace HomepageWeb.Models
{
    
    /// <summary>
    /// <DefaultSites/> node
    /// </summary>
    public class DefaultSites : ConfigurationSection
    {
        private static readonly ConfigurationProperty s_property
            = new ConfigurationProperty(string.Empty, typeof(DefaultSitesCollection), null,
                                            ConfigurationPropertyOptions.IsDefaultCollection);

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public DefaultSitesCollection KeyValues
        {
            get
            {
                return (DefaultSitesCollection)base[s_property];
            }
        }
    }


    /// <summary>
    /// Collection containing all the <add/> elements.
    /// Collection can be looped.
    /// By accessing DefaultSites.KeyValues, this collecton can be returned
    /// </summary>
    [ConfigurationCollection(typeof(DefaultSiteSetting))]
    public class DefaultSitesCollection : ConfigurationElementCollection        
    {

        public DefaultSitesCollection() : base(StringComparer.OrdinalIgnoreCase)    
        { }

        new public DefaultSiteSetting this[string name]
        {
            get
            {
                return (DefaultSiteSetting)base.BaseGet(name);
            }
        }

        // functions defined in interface: ConfigurationElementCollection

        protected override ConfigurationElement CreateNewElement()
        {
            return new DefaultSiteSetting();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DefaultSiteSetting)element).Key;
        }
    }


    /// <summary>
    /// Every <add/> element in the collection.
    /// Settings are saved in this class.
    /// </summary>
    public class DefaultSiteSetting : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return this["key"].ToString(); }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return this["value"].ToString(); }
        }
    }
}

