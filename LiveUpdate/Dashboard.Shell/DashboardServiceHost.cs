using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DashboardContracts;

namespace Dashboard.Shell
{
    public class DashboardServiceHost
    {
        private ServiceHost dashboardServiceHost;

        public DashboardServiceHost(IMainWindowViewModel mainViewModel)
        {
            var dashboardService = new Service.Service(mainViewModel);
            dashboardServiceHost = new ServiceHost(dashboardService, new Uri("net.tcp://localhost:8001/services/"));
            dashboardServiceHost.Open();
        }
    }
}
