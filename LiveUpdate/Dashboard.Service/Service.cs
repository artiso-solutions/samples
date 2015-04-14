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
            string message = "Component {0} is now running on version {1}";
            switch (component)
            {
                case "Service":
                    if (!String.IsNullOrEmpty(viewModel.CurrentServiceVersion))
                    {
                        viewModel.CurrentServiceVersion += " / ";
                    }
                    viewModel.CurrentServiceVersion += currentVersion;
                    viewModel.Messages.Insert(0,new DashboardMessage() {Message = String.Format(message,"Service", currentVersion), Timestamp = DateTime.Now, Type = MessageTypes.Info});
                    break;
                case "ClientA":
                    viewModel.CurrentClientAVersion = currentVersion;
                    viewModel.Messages.Insert(0,new DashboardMessage() { Message = String.Format(message, "Client A", currentVersion), Timestamp = DateTime.Now, Type = MessageTypes.Info});
                    break;
                case "ClientB":
                    viewModel.CurrentClientBVersion = currentVersion;
                    viewModel.Messages.Insert(0,new DashboardMessage() { Message = String.Format(message, "Client B", currentVersion), Timestamp = DateTime.Now, Type = MessageTypes.Info});
                    break;
                default:
                    throw new ArgumentException("Invalid component recognized in NotifyUpdatedVersion");
            }
        }

        public void NotifyFallback(string component, string exceptionMessage)
        {
            string message = String.Format("Error on {0}: {1}. Executing fallback", component, exceptionMessage);
            viewModel.Messages.Insert(0,new DashboardMessage() { Message = message, Timestamp=DateTime.Now, Type=MessageTypes.Error});
        }
    }
}
