using System;
using DashboardContracts;

namespace Services.Service
{
   using System.ServiceModel;

   using Dashboard.Client;

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

      private readonly DashboardClient dashboardClient;

      public ClientService()
      {
         dashboardClient = new DashboardClient(logger);
         DashboardUpdatedVersion("Service", "v1");
      }
      public void DashboardUpdatedVersion(string component, string version)
      {
         
         dashboardClient.DashboardUpdatedVersion(component, version);
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