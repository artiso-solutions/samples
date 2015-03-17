namespace ServiceHostContainer.Contracts
{
  /// <summary>
  /// The base class for hosted services.
  /// </summary>
  public abstract class HostedServiceBase : IHostedService
  {
    /// <summary>
    /// Called when the service starts. This might be an asynchronous call according to the service configuration.
    /// </summary>
    public virtual void OnStart()
    {
    }

    /// <summary>
    /// Called when the service stops.
    /// </summary>
    public virtual void OnStop()
    {
    }
  }
}