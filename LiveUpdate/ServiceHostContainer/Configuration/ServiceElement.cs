namespace ServiceHostContainer.Configuration
{
    using System.Configuration;

    public class ServiceElement : ConfigurationElement
    {

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("isEnabled", IsRequired = false, DefaultValue = true)]
        public bool IsEnabled
        {
            get
            {
                return (bool)this["isEnabled"];
            }
            set
            {
                this["isEnabled"] = value;
            }
        }

        [ConfigurationProperty("startAsync", IsRequired = false, DefaultValue = true)]
        public bool StartAsync
        {
            get
            {
                return (bool)this["startAsync"];
            }
            set
            {
                this["startAsync"] = value;
            }
        }
    }
}