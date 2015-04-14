using System;
using System.ServiceModel;

namespace DashboardContracts
{
    [ServiceContract]
    public interface IDashboardContract
    {
        [OperationContract]
        void NotifyUpdatedVersion(string component, string currentVersion);

        [OperationContract]
        void NotifyFallback(string component, string exception);
    }
}
