namespace ServiceHostContainer.Configuration
{
    public class ServiceInformation
    {
        public ServiceInformation(string serviceName)
        {
            Name = serviceName;
            IsEnabled = true;
            StartAsync = true;
        }

        public string Name { get; private set; }

        public bool IsEnabled { get; set; }

        public bool StartAsync { get; set; }
    }
}