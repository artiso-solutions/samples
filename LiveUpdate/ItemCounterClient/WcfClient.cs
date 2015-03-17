namespace ItemCounterClient
{
   using System;
   using System.Reflection;
   using System.ServiceModel;

   using Service.Contracts;

   using ServiceHostContainer.Contracts;

   public class WcfClient : IServiceOperationsCallback
   {
      private readonly MainWindow mainWindow;

      private IService itemCounterClient;

      public WcfClient(MainWindow mainWindow)
      {
         this.mainWindow = mainWindow;
      }

      public string GetEndpointFromDispatcher()
      {
         var myBinding = new NetTcpBinding();

         EndpointAddress myEndpoint = new EndpointAddress("net.tcp://localhost:8000/services/Dispatcher");

         ChannelFactory<IServiceDispatcher> myChannelFactory = new ChannelFactory<IServiceDispatcher>(myBinding, myEndpoint);
         IServiceDispatcher wcfDispatcher = myChannelFactory.CreateChannel();
         return wcfDispatcher.GetEndpoint(Assembly.GetExecutingAssembly().GetName().Version.ToString());
      }

      public void SubsrcibeToEndPoint(string dispatcherEndpoint)
      {
         var myBinding = new NetTcpBinding();

         EndpointAddress myEndpoint = new EndpointAddress(dispatcherEndpoint);

         var myChannelFactory = new DuplexChannelFactory<IService>(this, myBinding, myEndpoint);
         itemCounterClient = myChannelFactory.CreateChannel();
         itemCounterClient.Subscribe();
      }

      public void UnSubsrcibeFromEndPoint()
      {
         itemCounterClient.Unsubscribe();
      }

        public void StartEngine()
      {
         itemCounterClient.OnStartEngine();
      }

      public void StopEngine()
      {
         itemCounterClient.OnStopEngine();
      }

      public void CountChanged(int count)
      {
         mainWindow.CountTextBlock.Text = count.ToString();
      }

      public void StartStopChanged(bool on)
      {
         if (on)
         {
            mainWindow.StartStopButton.Content = "Stop";
         }
         else
         {
            mainWindow.StartStopButton.Content = "Start";
         }

      }
   }
}