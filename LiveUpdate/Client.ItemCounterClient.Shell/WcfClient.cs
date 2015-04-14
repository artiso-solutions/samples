using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DashboardContracts;

namespace Shell
{
    public class WcfClient
    {
        public void DashboardUpdatedVersion(string component, string version)
        {
            var myBinding = new NetTcpBinding();
            EndpointAddress myEndpoint = new EndpointAddress("net.tcp://localhost:8001/services");

            ChannelFactory<IDashboardContract> myChannelFactory = new ChannelFactory<IDashboardContract>(myBinding, myEndpoint);
            IDashboardContract wcfDashboard = myChannelFactory.CreateChannel();
            wcfDashboard.NotifyUpdatedVersion(component, version);
        }

        public void NotifyFallback(string component, Exception exception)
        {
            var myBinding = new NetTcpBinding();
            EndpointAddress myEndpoint = new EndpointAddress("net.tcp://localhost:8001/services");

            ChannelFactory<IDashboardContract> myChannelFactory = new ChannelFactory<IDashboardContract>(myBinding, myEndpoint);
            IDashboardContract wcfDashboard = myChannelFactory.CreateChannel();
            wcfDashboard.NotifyFallback(component, exception.Message);

        }
    }
}
