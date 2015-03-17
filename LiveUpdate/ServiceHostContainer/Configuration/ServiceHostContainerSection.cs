namespace ServiceHostContainer.Configuration
{
    using System.Configuration;

    ////<ServiceHostContainer>
    ////    <BaseAddresses>
    ////        <add uri="net.tcp://localhost:12345/services/" />
    ////    </BaseAddresses>
    ////    <Services>
    ////        <add name="Service1" isEnabled="true" />
    ////        <add name="Service2" isEnabled="false" />
    ////    </Services>
    ////</ServiceHostContainer>

    public class ServiceHostContainerSection : ConfigurationSection
    {
        public const string SectionName = "ServiceHostContainer";

        public const string BaseAddressProperty = "BaseAddresses";

        public const string ServicesProperty = "Services";

        [ConfigurationProperty(BaseAddressProperty)]
        [ConfigurationCollection(typeof(BaseAddressCollection), AddItemName = "add")]
        public BaseAddressCollection BaseAddresses
        {
            get
            {
                return (BaseAddressCollection)base[BaseAddressProperty];
            }
        }

        [ConfigurationProperty(ServicesProperty)]
        [ConfigurationCollection(typeof(BaseAddressCollection), AddItemName = "add")]
        public ServicesCollection Services
        {
            get
            {
                return (ServicesCollection)base[ServicesProperty];
            }
        }
    }
}
