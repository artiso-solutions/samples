using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FischerTechnikService
{
    public class Config
    {
        public static FtApi.ControllerConfiguration ControllerConfig = FtApi.FtFunc.InitializeController("COM3", 0);
    }
}
