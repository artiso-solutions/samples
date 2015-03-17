namespace FischerTechnikService.Contracts
{
   using System.ServiceModel;

   public interface ISignalChangeCallback
   {
      [OperationContract(IsOneWay = true)]
      void SignalChanged(bool on);
   }
}