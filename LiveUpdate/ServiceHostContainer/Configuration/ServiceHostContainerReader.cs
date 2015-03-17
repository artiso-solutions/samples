namespace ServiceHostContainer.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class ServiceHostContainerReader
    {
        public static ServiceHostContainerConfiguration ReadConfiguration()
        {
            var configuration = new ServiceHostContainerConfiguration();

            var serviceHostContainerSection = ConfigurationManager.GetSection(ServiceHostContainerSection.SectionName) as ServiceHostContainerSection;
            if (serviceHostContainerSection == null)
            {
                throw new ConfigurationErrorsException("Missing configuration of service host container.");
            }

            configuration.BaseAddresses = new List<Uri>(
                serviceHostContainerSection.BaseAddresses
                    .OfType<BaseAddressElement>()
                    .Select(e => new Uri(e.Uri))
                    );
          
            return configuration;
        }
    }
}