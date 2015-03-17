namespace ServiceHostContainer.Contracts
{
    public interface IHostedServiceMetadata
    {
        string ServiceName { get; }

        string Version { get;  }

        int Order { get; }
    }
}