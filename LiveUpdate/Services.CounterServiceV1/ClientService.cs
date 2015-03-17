namespace Services.Service
{
   using System.ServiceModel;

   using FischerTechnikWcfClient;

   using log4net;

   using global::Service.Contracts;

   using ServiceHostContainer.Contracts;

   [HostedServiceExport(typeof(ClientService), "Service", 100)]
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
   public class ClientService : HostedServiceBase, IService
   {
      private readonly ILog logger = LogManager.GetLogger(typeof(ClientService));

      private FischerTechnikClient fischerTechnikWcfClient;

      private int itemsCount;

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