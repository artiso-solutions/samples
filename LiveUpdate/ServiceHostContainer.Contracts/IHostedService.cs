namespace ServiceHostContainer.Contracts
{
    public interface IHostedService
    {
        void OnStart();

        void OnStop();
    }
}