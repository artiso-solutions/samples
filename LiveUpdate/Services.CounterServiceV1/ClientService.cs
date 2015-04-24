using System;
using DashboardContracts;

namespace Services.Service
{
   using System.ServiceModel;

   using FischerTechnikWcfClient;

   using log4net;

   using global::Service.Contracts;

   using ServiceHostContainer.Contracts;

   [HostedServiceExport(typeof(ClientService), "CounterService", 100)]
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
   public class ClientService : HostedServiceBase, IService
   {
      private readonly ILog logger = LogManager.GetLogger(typeof(ClientService));

      private FischerTechnikClient fischerTechnikWcfClient;

      private int itemsCount;

       public ClientService()
       {
            DashboardUpdatedVersion("Service", "v1");
       }
        public void DashboardUpdatedVersion(string component, string version)
        {
            var myBinding = new NetTcpBinding();
            var identity = EndpointIdentity.CreateSpnIdentity("dummy");
            EndpointAddress myEndpoint = new EndpointAddress(new Uri("net.tcp://car0005:8001/services"), identity);

            ChannelFactory<IDashboardContract> myChannelFactory = new ChannelFactory<IDashboardContract>(myBinding,
                myEndpoint);
            IDashboardContract wcfDashboard = myChannelFactory.CreateChannel();
            wcfDashboard.NotifyUpdatedVersion(component, version);
            logger.Info(String.Format("Send version {0} to dashboard service", version));
        }

        public void ConnectToFischerTechnikService()
      {
         fischerTechnikWcfClient = new FischerTechnikClient(OnSignalChanged);
         fischerTechnikWcfClient.ConnectToService();
         logger.Info("Sucessfully connected to FischerTechnik Service");
      }

      public int GetCurrentCount()
      {
         return itemsCount;
      }

      public bool Start()
      {
         if (fischerTechnikWcfClient == null)
         {
            ConnectToFischerTechnikService();
         }

         return true;
      }

      private void OnSignalChanged(bool obj)
      {
         if (obj)
         {
            itemsCount++;
         }
      }
   }
}