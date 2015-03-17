namespace Service.Contracts
{
   using System.ServiceModel;

   [ServiceContract]
   public interface IService
   {
      [OperationContract]
      bool Start();

      [OperationContract]
      int GetCurrentCount();
   }
}
