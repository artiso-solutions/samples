using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHostContainer
{
  using System.ServiceModel;

  using global::ServiceHostContainer.Contracts;
  using Service.Contracts;

  [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
   public class ServiceDispatcher : IServiceDispatcher
   {
      private readonly Dictionary<string, ServiceHost> serviceHosts;

      public ServiceDispatcher(Dictionary<string, ServiceHost> serviceHosts)
      {
         this.serviceHosts = serviceHosts;
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
   }
}
