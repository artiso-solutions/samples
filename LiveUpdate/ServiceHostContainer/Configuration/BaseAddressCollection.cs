namespace ServiceHostContainer.Configuration
{
    using System.Configuration;

    public class BaseAddressCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BaseAddressElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BaseAddressElement)element).Uri;
        }
    }
}