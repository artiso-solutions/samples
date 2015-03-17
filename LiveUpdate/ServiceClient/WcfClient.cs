namespace ItemCounterClient.MainView
{
   using System;
   using System.Reflection;
   using System.ServiceModel;
   using Service.Contracts;

   public class WcfClient : IServiceOperationsCallback
   {
      private readonly MainControl mainWindow;

      private IService itemCounterClient;

      public event EventHandler countChangedEvent;

      public WcfClient(MainControl mainWindow)
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

      public void SubscribeToEndPoint(string dispatcherEndpoint)
      {
         var myBinding = new NetTcpBinding();

         EndpointAddress myEndpoint = new EndpointAddress(dispatcherEndpoint);

         var myChannelFactory = new DuplexChannelFactory<IService>(this, myBinding, myEndpoint);
         itemCounterClient = myChannelFactory.CreateChannel();
         itemCounterClient.Subscribe();
      }

      public void UnSubscribeFromEndPoint()
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

      public void CountChanged(int count, int countBig, int countSmall)
      {
         mainWindow.CountTextBlock.Text = count.ToString();
      mainWindow.CountBigTextBlock.Text = countBig.ToString();
      mainWindow.CountSmallTextBlock.Text = countSmall.ToString();
    }

    public void StartStopChanged(bool on)
    {

      }
   }
}