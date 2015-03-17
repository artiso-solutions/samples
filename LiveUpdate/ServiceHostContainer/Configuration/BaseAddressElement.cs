namespace ServiceHostContainer.Configuration
{
    using System.Configuration;

    public class BaseAddressElement : ConfigurationElement
    {
        [ConfigurationProperty("uri", IsRequired = true)]
        public string Uri
        {
            get
            {
                return (string)this["uri"];
            }
            set
            {
                this["uri"] = value;
            }
        }
    }
}