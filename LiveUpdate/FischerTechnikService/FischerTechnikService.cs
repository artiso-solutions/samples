using System.Collections.Generic;

namespace FischerTechnikService
{
   using System.ServiceModel;

   using global::FischerTechnikService.Contracts;

   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
   public class FischerTechnikService : IFischerTechnikService
   {
      private static readonly List<ISignalChangeCallback> subscribers = new List<ISignalChangeCallback>();

      private readonly IFischerTechnikLogic fischerTechnikLogic;

      public FischerTechnikService(IFischerTechnikLogic fischerTechnikLogic)
      {
         this.fischerTechnikLogic = fischerTechnikLogic;
      }

      public void StartListening()
      {
         fischerTechnikLogic.StartListenToPhotoSensor(OnSignalChanged);
      }

      public void Stop()
      {
         fischerTechnikLogic.StopMotor();
      }

      public bool Subscribe()
      {
         try
         {
            var callback = OperationContext.Current.GetCallbackChannel<ISignalChangeCallback>();
            if (!subscribers.Contains(callback))
               subscribers.Add(callback);
            return true;
         }
         catch
         {
            return false;
         }
      }

      public bool Unsubscribe()
      {
         try
         {
            var callback = OperationContext.Current.GetCallbackChannel<ISignalChangeCallback>();
            if (subscribers.Contains(callback))
               subscribers.Remove(callback);
            return true;
         }
         catch
         {
            return false;
         }
      }

      void OnSignalChanged(bool on)
      {
         subscribers.ForEach(delegate(ISignalChangeCallback callback)
         {
            if (((ICommunicationObject)callback).State == CommunicationState.Opened)
            {
               callback.SignalChanged(on);
            }
            else
            {
               subscribers.Remove(callback);
            }
         });
      }
   }
}