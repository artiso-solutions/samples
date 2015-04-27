namespace Service.Contracts
{
   using System.ServiceModel;

   [ServiceContract]
   public interface IServiceDispatcher
   {

      [OperationContract]
      string GetEndpoint(string clientVersion);

      [OperationContract]
      string GetDashboardEndpoint();
   }
}