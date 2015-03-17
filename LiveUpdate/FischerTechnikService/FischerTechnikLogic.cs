namespace FischerTechnikService
{
   using System;
   using System.Threading;
   using System.Threading.Tasks;

   public class FischerTechnikLogic : IFischerTechnikLogic
   {
      private Action<bool> signalChanged;

      private FischerTechnikService fischerTechnikService;

      public FischerTechnikLogic()
      {
      }

    public void StartMotor()
    {
      FtApi.FtFunc.StartMotor(Config.ControllerConfig, 0, FtApi.Enums.MotorDirection.Left, 400);
      FtApi.FtFunc.StartMotor(Config.ControllerConfig, 1, FtApi.Enums.MotorDirection.Right, 500);
    }

      public void StartListenToPhotoSensor(Action<bool> signalChanged)
      {
         this.signalChanged = signalChanged;
         var photoSensorListenerTask = new Task(ListenToPhotoSensor);
         photoSensorListenerTask.Start();
      }


      private void ListenToPhotoSensor()
      {
         bool prevPhotoValue = false;
         while (true)
         {
            var currentPhotoValue = FtApi.FtFunc.GetDigitalIo(Config.ControllerConfig, 0);
            if (currentPhotoValue != prevPhotoValue)
            {
               signalChanged(currentPhotoValue);
               prevPhotoValue = currentPhotoValue;
            }

            Thread.Sleep(10);
         }
      }

      public void StopMotor()
      {
         FtApi.FtFunc.StopMotor(Config.ControllerConfig, 0);
         FtApi.FtFunc.StopMotor(Config.ControllerConfig, 1);
      }
   }
}