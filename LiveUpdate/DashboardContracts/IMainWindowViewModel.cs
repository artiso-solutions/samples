using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardContracts
{
    public interface IMainWindowViewModel
    {
        string CurrentClientAVersion { get; set; }
        string CurrentClientBVersion { get; set; }
        string CurrentServiceVersion { get; set; }
        string UpdateClientAVersion { get; set; }
        string UpdateClientBVersion { get; set; }
        string UpdateServiceVersion { get; set; }
    }
}
