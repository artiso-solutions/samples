namespace FischerTechnikService
{
   using System;

   public interface IFischerTechnikLogic
   {
      void StartMotor();

      void StartListenToPhotoSensor(Action<bool> signalChanged);

      void StopMotor();
   }
}