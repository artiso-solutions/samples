using DashboardContracts;

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

      private int count;

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

      public void Start(string endPoint)
      {
         var myBinding = new NetTcpBinding();

         EndpointAddress myEndpoint = new EndpointAddress(endPoint);

         var myChannelFactory = new ChannelFactory<IService>(myBinding, myEndpoint);
         itemCounterClient = myChannelFactory.CreateChannel();
         itemCounterClient.Start();

         Task.Factory.StartNew(() =>
         {
            while (true)
            {
               var count = GetCurrentCount();
               if (count != this.count)
               {
                  this.count = count;
                  CountChanged(count);
               }
               Thread.Sleep(100);
            }
         });
      }

      public int GetCurrentCount()
      {
         return itemCounterClient.GetCurrentCount();
      }

      private void CountChanged(int count)
      {
         var handler = OnCountChanged;
         if (handler != null)
         {
            handler(this, new CountChangedEventHandlerArgs(count));
         }
      }
   }

   public delegate void CountChangedEventHandler(object sender, CountChangedEventHandlerArgs args);

   public class CountChangedEventHandlerArgs : EventArgs
   {
      public CountChangedEventHandlerArgs(int currentCount)
      {
         CurrentCount = currentCount;
      }

      public int CurrentCount { get; private set; }
   }
}