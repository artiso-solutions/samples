namespace ServiceHostContainer
{
   using System.Collections.Generic;
   using System.Linq;
   using System.ServiceModel;

   using Service.Contracts;

   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
   public class ServiceDispatcher : IServiceDispatcher
   {
      private readonly Dictionary<string, ServiceHost> serviceHosts = new Dictionary<string, ServiceHost>();

      private readonly string dashboardEndpoint;

      public ServiceDispatcher(Dictionary<string, ServiceHost> serviceHosts, string dashboardEndPoint)
      {
         dashboardEndpoint = dashboardEndPoint;
      }

      public void AddServiceHost(string version, ServiceHost serviceHost)
      {
         this.serviceHosts[version] = serviceHost;
      }

      public string GetEndpoint(string clientVersion)
      {
         var hostKey = serviceHosts.Keys.FirstOrDefault(key => key.EndsWith(clientVersion));
         if (hostKey == null)
         {
            return string.Empty;
         }

         return serviceHosts[hostKey].Description.Endpoints.First().ListenUri.ToString();
      }

      public string GetDashboardEndpoint()
      {
         
         return dashboardEndpoint;
      }
   }
}
