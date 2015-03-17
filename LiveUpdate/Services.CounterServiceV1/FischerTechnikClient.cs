namespace FischerTechnikWcfClient
{
   using System;
   using System.ServiceModel;

   using FischerTechnikService.Contracts;

   using log4net;

   using Services.Service;

   [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
   public class FischerTechnikClient : ISignalChangeCallback, IDisposable
   {
      private readonly Action<bool> signalChangedAction;
      private ILog logger = LogManager.GetLogger(typeof(FischerTechnikClient));

      private IFischerTechnikService fischerTechnikService;

      private Action<bool> SignalChangedAction;

      public FischerTechnikClient(Action<bool> signalChangedAction)
      {
         this.signalChangedAction = signalChangedAction;
      }

      public void ConnectToService()
      {
         var myBinding = new NetTcpBinding();

         EndpointAddress myEndpoint = new EndpointAddress("net.tcp://localhost:8000/services/Fischertechnik");

         ChannelFactory<IFischerTechnikService> myChannelFactory = new DuplexChannelFactory<IFischerTechnikService>(this, myBinding, myEndpoint);
         fischerTechnikService = myChannelFactory.CreateChannel();

         logger.Info(string.Format(" Is subscribed {0}", fischerTechnikService.Subscribe()));
      }

      public void Start()
      {
         fischerTechnikService.StartListening();
      }

      public void Stop()
      {
         fischerTechnikService.Stop();
      }

      public void SignalChanged(bool on)
      {
         signalChangedAction.Invoke(on);
      }

      public void Dispose()
      {
         fischerTechnikService.Stop();
         fischerTechnikService.Unsubscribe();
      }
   }
}