using System;

namespace FischerTechnikService
{
   using System.Threading;
   using System.Threading.Tasks;

   public class FischerTechnikTestLogic : IFischerTechnikLogic
   {
      private Action<bool> signalChanged;
      public void StartMotor()
      {

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
         int count = 0;
         while (true)
         {
            prevPhotoValue = !prevPhotoValue;
            signalChanged(prevPhotoValue);
            if (!prevPhotoValue)
            {
               count++;
               if (count == 3)
               {
                  count = 0;
                  Thread.Sleep(90);
               }
               else
               {
                  Thread.Sleep(35);
               }
            }

            Thread.Sleep(35);
         }
      }
      public void StopMotor()
      {

      }
   }
}
