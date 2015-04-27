namespace Dashboard.Client
{
   using System;
   using System.Collections.Concurrent;
   using System.ServiceModel;
   using System.Threading.Tasks;

   using DashboardContracts;

   using log4net;

   using Service.Contracts;

   public class DashboardClient
   {
      private readonly ILog logger;

      private const int QueueLimit = 50;

      private readonly ConcurrentQueue<Tuple<string, string>> updatedVersionQueue = new ConcurrentQueue<Tuple<string, string>>();
      private readonly ConcurrentQueue<Tuple<string, Exception>> notifyFallbackQueue = new ConcurrentQueue<Tuple<string, Exception>>();

      private volatile bool failedToConnectToService;

      private string dashboardComputerName = "localhost";

      public DashboardClient(ILog logger)
      {
         this.logger = logger;
      }

      public void DashboardUpdatedVersion(string component, string version)
      {
         updatedVersionQueue.Enqueue(new Tuple<string, string>(component, version));
         ResizeUpdatedVersionQueueIfNecessary();

         Task.Factory.StartNew(
            () =>
            {
               lock (this)
               {
                  try
                  {
                     if (failedToConnectToService)
                     {
                        return;
                     }

                     NotifyUpdatedVersion(true);

                  }
                  catch (Exception e)
                  {
                     failedToConnectToService = true;
                     logger.Warn("Dashboard is not available", e);
                     NotifyUpdatedVersion(false);
                  }

                  failedToConnectToService = false;
               }
            });
      }


      public void NotifyFallback(string component, Exception exception)
      {
         notifyFallbackQueue.Enqueue(new Tuple<string, Exception>(component, exception));
         ResizeNotifyFallbackQueueIfNecessary();

         Task.Factory.StartNew(
          () =>
          {
             lock (this)
             {
                try
                {
                   if (failedToConnectToService)
                   {
                      return;
                   }

                   NotifyFallback(true);
                }
                catch (Exception e)
                {
                   failedToConnectToService = true;
                   logger.Warn("Dashboard is not available", e);
                   NotifyFallback(false);
                }

                failedToConnectToService = false;
             }
          });
      }
      
      private void ResizeNotifyFallbackQueueIfNecessary()
      {
         if (notifyFallbackQueue.Count <= QueueLimit)
         {
            return;
         }

         Tuple<string, Exception> remove;
         while (!notifyFallbackQueue.TryDequeue(out remove))
         {
         }
      }

      private void ResizeUpdatedVersionQueueIfNecessary()
      {
         if (updatedVersionQueue.Count <= QueueLimit)
         {
            return;
         }

         Tuple<string, string> remove;
         while (!updatedVersionQueue.TryDequeue(out remove))
         {
         }
      }

      private void NotifyFallback(bool throwException)
      {
         while (notifyFallbackQueue.Count != 0)
         {
            try
            {
               Tuple<string, Exception> dequeue;
               while (!notifyFallbackQueue.TryPeek(out dequeue))
               {
               }

               var wcfDashboard = ConnectToDashboard();
               wcfDashboard.NotifyFallback(dequeue.Item1, dequeue.Item2.Message);
               while (!notifyFallbackQueue.TryDequeue(out dequeue))
               {
               }
            }
            catch
            {
               if (throwException)
               {
                  throw;
               }
            }
         }
      }

      private void NotifyUpdatedVersion(bool throwException)
      {
         while (updatedVersionQueue.Count != 0)
         {
            try
            {
               Tuple<string, string> dequeue;
               while (!updatedVersionQueue.TryPeek(out dequeue))
               {
               }

               var wcfDashboard = ConnectToDashboard();
               wcfDashboard.NotifyUpdatedVersion(dequeue.Item1, dequeue.Item2);
               while (!updatedVersionQueue.TryDequeue(out dequeue))
               {
               }
            }
            catch
            {
               if (throwException)
               {
                  throw;
               }
            }
         }
      }

      private static string GetEndpointFromDispatcher()
      {
         var myBinding = new NetTcpBinding();

         EndpointAddress myEndpoint = new EndpointAddress("net.tcp://localhost:8000/services/Dispatcher");

         ChannelFactory<IServiceDispatcher> myChannelFactory = new ChannelFactory<IServiceDispatcher>(myBinding, myEndpoint);
         IServiceDispatcher wcfDispatcher = myChannelFactory.CreateChannel();
         return wcfDispatcher.GetDashboardEndpoint();
      }
      private IDashboardContract ConnectToDashboard()
      {
         var myBinding = new NetTcpBinding();
         var identity = EndpointIdentity.CreateSpnIdentity("dummy");
         var endPoint = GetEndpointFromDispatcher();
         var dashboardEndPoint = new EndpointAddress(new Uri(endPoint), identity);

         var myChannelFactory = new ChannelFactory<IDashboardContract>(myBinding, dashboardEndPoint);
         var wcfDashboard = myChannelFactory.CreateChannel();
         return wcfDashboard;
      }
   }
}