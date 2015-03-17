namespace CommunicationClient
{
   using System;
   using System.Reflection;
   using System.ServiceModel;
   using System.Threading;
   using System.Threading.Tasks;

   using Service.Contracts;

   public class WcfClient
   {
      private IService itemCounterClient;

      private Counts counts = new Counts();

      public event CountChangedEventHandler OnCountChanged;

      public string GetEndpointFromDispatcher()
      {
         return GetEndpointFromDispatcher(Assembly.GetExecutingAssembly().GetName().Version.ToString());
      }

      public string GetEndpointFromDispatcher(string version)
      {
         var myBinding = new NetTcpBinding();

         EndpointAddress myEndpoint = new EndpointAddress("net.tcp://localhost:8000/services/Dispatcher");

         ChannelFactory<IServiceDispatcher> myChannelFactory = new ChannelFactory<IServiceDispatcher>(myBinding, myEndpoint);
         IServiceDispatcher wcfDispatcher = myChannelFactory.CreateChannel();
         return wcfDispatcher.GetEndpoint(version);
      }

      public void Start(string dispatcherEndpoint)
      {
         var myBinding = new NetTcpBinding();

         EndpointAddress myEndpoint = new EndpointAddress(dispatcherEndpoint);

         var myChannelFactory = new ChannelFactory<IService>(myBinding, myEndpoint);
         itemCounterClient = myChannelFactory.CreateChannel();
         itemCounterClient.Start();

         Task.Factory.StartNew(() =>
         {
            while (true)
            {
               var counts = itemCounterClient.GetAllCounts();
               if (counts.CurrentCount != this.counts.CurrentCount)
               {
                  this.counts = counts;
                  CountChanged(counts);
               }
               Thread.Sleep(100);
            }
         });
      }

      public void CountChanged(Counts counts)
      {
         var handler = OnCountChanged;
         if (handler != null)
         {
            handler(this, new CountChangedEventHandlerArgs(counts.CurrentCount, counts.CurrentBig, counts.CurrentSmall));
         }
      }
   }

   public delegate void CountChangedEventHandler(object sender, CountChangedEventHandlerArgs args);

   public class CountChangedEventHandlerArgs : EventArgs
   {
      public CountChangedEventHandlerArgs(int currentCount, int countBig, int countSmall)
      {
         CurrentCount = currentCount;
         CurrentBig = countBig;
         CurrentSmall = countSmall;
      }

      public int CurrentCount { get; private set; }

      public int CurrentBig { get; private set; }

      public int CurrentSmall { get; private set; }
   }
}