using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var identity = EndpointIdentity.CreateSpnIdentity("dummy");
            EndpointAddress myEndpoint = new EndpointAddress(new Uri("net.tcp://car0005:8001/services"), identity);

            ChannelFactory<IDashboardContract> myChannelFactory = new ChannelFactory<IDashboardContract>(myBinding, myEndpoint);
            IDashboardContract wcfDashboard = myChannelFactory.CreateChannel();
            wcfDashboard.NotifyUpdatedVersion(component, version);
        }

        public void NotifyFallback(string component, Exception exception)
        {
            var myBinding = new NetTcpBinding();
            var identity = EndpointIdentity.CreateSpnIdentity("dummy");
            EndpointAddress myEndpoint = new EndpointAddress(new Uri("net.tcp://car0005:8001/services"), identity);

            ChannelFactory<IDashboardContract> myChannelFactory = new ChannelFactory<IDashboardContract>(myBinding, myEndpoint);
            IDashboardContract wcfDashboard = myChannelFactory.CreateChannel();
            wcfDashboard.NotifyFallback(component, exception.Message);

        }
    }
}
