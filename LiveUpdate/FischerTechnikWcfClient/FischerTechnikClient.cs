namespace FischerTechnikWcfClient
{
   using System;
   using System.ServiceModel;

   using FischerTechnikService.Contracts;

   [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
   public class FischerTechnikClient : ISignalChangeCallback, IDisposable
   {
      private IFischerTechnikService wcfClient1;

      public void ConnectToService()
      {
         var myBinding = new NetTcpBinding();

         EndpointAddress myEndpoint = new EndpointAddress("net.tcp://localhost:8000/services/Fischertechnik");

         ChannelFactory<IFischerTechnikService> myChannelFactory = new DuplexChannelFactory<IFischerTechnikService>(this, myBinding, myEndpoint);
         wcfClient1 = myChannelFactory.CreateChannel();
         

         Console.WriteLine(string.Format(" Is subscribed {0}", wcfClient1.Subscribe()));
         wcfClient1.Start();
         Console.Read();
      }

      public void Start()
      {
         wcfClient1.Start();
      }

      public void Stop()
      {
         wcfClient1.Stop();
      }

      public void SignalChanged(bool on)
      {
         Console.WriteLine(string.Format("Signal changed to '{0}'", on));
      }

      public void Dispose()
      {
         wcfClient1.Stop();
         wcfClient1.Unsubscribe();
      }
   }
}