using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DashboardContracts;

namespace Dashboard.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service : IDashboardContract
    {
        private readonly IMainWindowViewModel viewModel;

        public Service(IMainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public void NotifyUpdatedVersion(string component, string currentVersion)
        {
            switch (component)
            {
                case "Service":
                    viewModel.CurrentServiceVersion += currentVersion + "/";
                    break;
                case "ClientA":
                    viewModel.CurrentClientAVersion = currentVersion;
                    break;
                case "ClientB":
                    viewModel.CurrentClientBVersion = currentVersion;
                    break;
                default:
                    throw new ArgumentException("Invalid component recognized in NotifyUpdatedVersion");
            }
        }

        public void NotifyFallback(string component, string currentVersion, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
