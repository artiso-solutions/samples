using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace FischerTechnikServiceTests
{
   using FischerTechnikService;

   [TestClass]
    public class StartStopTests
    {
        [TestMethod]
        public void StartMotorAndPhotoSensor_Wait5Sek_Stop()
        {
            var service = new FischerTechnikService(new FischerTechnikLogic());
            service.StartListening();

            Thread.Sleep(5000);

            service.Stop();
        }

        [TestMethod]
        public void StopMotorAndPhotoSensor()
        {
            var service = new FischerTechnikService(new FischerTechnikLogic());
            service.Stop();
        }
    }
}
