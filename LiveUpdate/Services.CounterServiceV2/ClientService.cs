using DashboardContracts;

namespace Services.Service
{
   using System;
   using System.Reflection;
   using System.ServiceModel;

   using CommunicationClient;

   using FischerTechnikWcfClient;

   using log4net;

   using global::Service.Contracts;

   using ServiceHostContainer.Contracts;

   [HostedServiceExport(typeof(ClientService), "CounterService", 100)]
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
   public class ClientService : HostedServiceBase, IService
   {
      #region Fields

      private readonly Counts counts = new Counts();

      private FischerTechnikClient fischerTechnikWcfClient;

      private readonly ILog logger = LogManager.GetLogger(typeof(ClientService));

      private DateTime lastSignalOn = DateTime.Now;

      private int timeThreshold = 120;

        #endregion

        #region Constructors and Destructors
        public ClientService()
        {
            DashboardUpdatedVersion("Service", "v2");
        }

        #endregion

        #region Public Methods and Operators
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

        public Counts GetAllCounts()
      {
         return counts;
      }

      public int GetCurrentCount()
      {
         return counts.CurrentCount;
      }

      public bool Start()
      {
         if (fischerTechnikWcfClient == null)
         {
            InitializeCount();
            ConnectToFischerTechnikService();
         }

         return true;
      }

      #endregion

      #region Methods

      private void ConnectToFischerTechnikService()
      {
         fischerTechnikWcfClient = new FischerTechnikClient(OnSignalChanged);
         fischerTechnikWcfClient.ConnectToService();
         logger.Info("Sucessfully connected to FischerTechnik Service");
      }

      private void InitializeCount()
      {
         var wcfClient = new WcfClient();
         string dispatcherEndpoint = wcfClient.GetEndpointFromDispatcher("1.0.0.0");
         if (!string.IsNullOrEmpty(dispatcherEndpoint))
         {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            logger.InfoFormat("Service {0} subcribes to service version 1.0.0.0 to get the count", version);
            wcfClient.Start(dispatcherEndpoint);
            counts.CurrentCount = wcfClient.GetCurrentCount();
         }
      }

      private void OnSignalChanged(bool obj)
      {
         if (obj)
         {
            counts.CurrentCount++;
            if (DateTime.Now > lastSignalOn.AddMilliseconds(timeThreshold))
            {
               counts.CurrentBig++;
            }
            else
            {
               counts.CurrentSmall++;
            }

            lastSignalOn = DateTime.Now;
         }
      }

      #endregion
   }
}