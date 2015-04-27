namespace Dashboard.Shell
{
   using System;
   using System.ServiceModel;

   using Dashboard.Service;

   using DashboardContracts;

   public class DashboardServiceHost
    {
        private readonly ServiceHost dashboardServiceHost;

        public DashboardServiceHost(IMainWindowViewModel mainViewModel)
        {
            var dashboardService = new Service(mainViewModel);
            dashboardServiceHost = new ServiceHost(dashboardService, new Uri("net.tcp://localhost:8001/services/"));
            dashboardServiceHost.Open();
        }
    }
}