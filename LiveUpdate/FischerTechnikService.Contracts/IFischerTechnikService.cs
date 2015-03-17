using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FischerTechnikService.Contracts
{
   using System.ServiceModel;

   [ServiceContract(CallbackContract = typeof(ISignalChangeCallback))]
    public interface IFischerTechnikService
   {
      [OperationContract]
      void StartListening();

      [OperationContract]
      void Stop();

      [OperationContract]
      bool Subscribe();

      [OperationContract]
      bool Unsubscribe();
   }
}
